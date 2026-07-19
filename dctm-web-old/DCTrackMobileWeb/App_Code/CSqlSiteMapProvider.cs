using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.Configuration;

namespace App_Code
{
    /// <summary>
    /// Summary description for SqlSiteMapProvider
    /// </summary>
    [SqlClientPermission(SecurityAction.Demand, Unrestricted = true)]
    public class CSqlSiteMapProvider : StaticSiteMapProvider
    {
        public static string PasswordExpired;
        private const string _errmsg1 = "Missing node ID";
        private const string _errmsg2 = "Duplicate node ID";
        private const string _errmsg3 = "Missing parent ID";
        private const string _errmsg4 = "Invalid parent ID";
        private const string _errmsg5 = "Empty or missing connectionStringName";
        private const string _errmsg6 = "Missing connection string";
        private const string _errmsg7 = "Empty connection string";
        private const string _errmsg8 = "Invalid sqlCacheDependency";
        public const string _cacheDependencyName = "__SiteMapCacheDependency";
        private const string _sqlSPSiteMap = "iAssetTrack_Sp_GetSiteMap";
        private const string _sqlSPSiteMapCache = "iAssetTrack_Sp_SiteMapCache";

        private string _connect;              // Database connection string
        private string _database, _table;     // Database info for SQL Server 7/2000 cache dependency
        private bool _2005dependency = false; // Database info for SQL Server 2005 cache dependency
        private int _indexID, _indexTitle, _indexUrl, _indexDesc, _indexRoles, _indexParent, _indexMenuImageURL;
        private Dictionary<int, SiteMapNode> _nodes = new Dictionary<int, SiteMapNode>(16);
        private readonly object _lock = new object();
        private SiteMapNode _root;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {

            System.Diagnostics.Trace.WriteLine("Initialize Called", "SqlSiteMapProvider TRACE");

            //base.Initialize(name, attributes);

            // Verify that config isn't null
            if (config == null)
                throw new ArgumentNullException("config");

            // Assign the provider a default name if it doesn't have one
            if (String.IsNullOrEmpty(name))
                name = "SqlSiteMapProvider";

            // Add a default "description" attribute to config if the
            // attribute doesn't exist or is empty
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "SQL site map provider");
            }

            // Call the base class's Initialize method
            base.Initialize(name, config);

            // Initialize _connect
            string connect = config["connectionStringName"];

            if (String.IsNullOrEmpty(connect))
                throw new ProviderException(_errmsg5);

            config.Remove("connectionStringName");

            if (WebConfigurationManager.ConnectionStrings[connect] == null)
                throw new ProviderException(_errmsg6);

            _connect = WebConfigurationManager.ConnectionStrings[connect].ConnectionString;

            if (String.IsNullOrEmpty(_connect))
                throw new ProviderException(_errmsg7);

            // Initialize SQL cache dependency info
            string dependency = config["sqlCacheDependency"];

            if (!String.IsNullOrEmpty(dependency))
            {
                if (String.Equals(dependency, "CommandNotification", StringComparison.InvariantCultureIgnoreCase))
                {
                    SqlDependency.Start(_connect);
                    _2005dependency = true;
                }
                else
                {
                    // If not "CommandNotification", then extract database and table names
                    string[] info = dependency.Split(new char[] { ':' });
                    if (info.Length != 2)
                        throw new ProviderException(_errmsg8);

                    _database = info[0];
                    _table = info[1];
                }

                config.Remove("sqlCacheDependency");
            }

            // SiteMapProvider processes the securityTrimmingEnabled
            // attribute but fails to remove it. Remove it now so we can
            // check for unrecognized configuration attributes.

            if (config["securityTrimmingEnabled"] != null)
                config.Remove("securityTrimmingEnabled");

            // Throw an exception if unrecognized attributes remain
            if (config.Count > 0)
            {
                string attr = config.GetKey(0);
                if (!String.IsNullOrEmpty(attr))
                    throw new ProviderException("Unrecognized attribute: " + attr);
            }


        }






        public override SiteMapNode BuildSiteMap()
        {
            //throw new Exception("The method or operation is not implemented.");

            lock (_lock)
            {
                if (CSqlSiteMapProvider.PasswordExpired.ToLower().CompareTo("true") != 0)
                {
                    // Return immediately if this method has been called before
                    if (_root != null)
                        return _root;

                    // Query the database for site map nodes
                    SqlConnection connection = new SqlConnection(_connect);

                    try
                    {
                        SqlCommand command = new SqlCommand("iAssetTrack_Sp_GetSiteMap", connection);
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        _indexID = reader.GetOrdinal("SiteMapId");
                        _indexUrl = reader.GetOrdinal("Url");
                        _indexTitle = reader.GetOrdinal("Title");
                        _indexDesc = reader.GetOrdinal("Description");
                        _indexRoles = reader.GetOrdinal("RoleName");
                        _indexParent = reader.GetOrdinal("Parent");
                        _indexMenuImageURL = reader.GetOrdinal("MenuImageURL");

                        int prevSiteMapId = -1;
                        SiteMapNode parentNode = null;
                        SiteMapNode currentNode = null;

                        if (reader.Read())
                        {
                            // Create the root SiteMapNode and add it to the site map
                            _root = CreateSiteMapNodeFromDataReader(reader);
                            currentNode = _root;

                            prevSiteMapId = reader.GetInt32(_indexID);
                            while (reader.Read())
                            {
                                int currSitetMapId = reader.GetInt32(_indexID);
                                if (prevSiteMapId == currSitetMapId)
                                {
                                    string role = reader.IsDBNull(_indexRoles) ? null : reader.GetString(_indexRoles).Trim();
                                    if (!String.IsNullOrEmpty(role))
                                    {
                                        currentNode.Roles.Add(role);
                                    }
                                }
                                else
                                {
                                    AddNode(currentNode, parentNode);
                                    currentNode = CreateSiteMapNodeFromDataReader(reader);
                                    prevSiteMapId = currSitetMapId;
                                    parentNode = GetParentNodeFromDataReader(reader);
                                }
                            }
                            AddNode(currentNode, parentNode);

                            CheckSiteMapCache();

                            //    // Use the SQL cache dependency
                            //    if (dependency != null)
                            //    {
                            //        HttpRuntime.Cache.Insert(_cacheDependencyName, new object(), dependency,
                            //            Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                            //            new CacheItemRemovedCallback(OnSiteMapChanged));
                            //    }
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }

                    // Return the root SiteMapNode
                }
                return _root;
            }


        }


        /// <summary>
        /// This must be called in Session_Start and insert if the cache is not found
        /// </summary>
        public void CheckSiteMapCache()
        {
            SqlConnection connection = new SqlConnection(_connect);
            // Create a SQL cache dependency if requested
            SqlCacheDependency dependency = null;

            if (_2005dependency)
            {
                SqlCommand cacheCheckCommand = new SqlCommand("iAssetTrack_Sp_SiteMapCache", connection);
                cacheCheckCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dependency = new SqlCacheDependency(cacheCheckCommand);
            }
            else if (!String.IsNullOrEmpty(_database) && !string.IsNullOrEmpty(_table))
                dependency = new SqlCacheDependency(_database, _table);

            // Use the SQL cache dependency
            if (dependency != null)
            {
                if (HttpContext.Current.Cache.Get(App_Code.CSqlSiteMapProvider._cacheDependencyName) == null)
                {

                    // Insert the Cache
                    HttpRuntime.Cache.Insert(_cacheDependencyName, new object(), dependency,
                        Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable,
                        new CacheItemRemovedCallback(OnSiteMapChanged));


                }
            }
        }


        public static void RefreshSiteMap()
        {
            HttpRuntime.Cache.Remove(App_Code.CSqlSiteMapProvider._cacheDependencyName);
        }


        protected override SiteMapNode GetRootNodeCore()
        {
            //throw new Exception("The method or operation is not implemented.");

            lock (_lock)
            {
                BuildSiteMap();
                return _root;
            }
        }

        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            lock (_lock)
            {
                bool isAccessible = false;

                if (node.Roles.Contains("*"))
                {
                    //isAccessible = true;
                    return true;
                }

                //iAssetTrack.BAL.UserBAL userBAL = new iAssetTrack.BAL.UserBAL();
                //int iUserId = Int32.Parse(context.Session["UserId"].ToString());
                //userBAL.UserID = iUserId;

                //DataSet ds = userBAL.retrieveAssignGroup();

                //foreach (String role in node.Roles)
                //{
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        if (role.Equals(ds.Tables[0].Rows[i]["Group"].ToString()))
                //        {
                //            isAccessible = true;
                //            break;
                //        }
                //    }

                //    if (isAccessible) break;

                //}

                String[] saGroups = (String[])context.Session["Groups"];

                for (int i = 0; i < saGroups.Length; i++)
                {
                    if (node.Roles.Contains(saGroups[i]))
                    {
                        return true;
                        //isAccessible = true;
                        //break;
                    }
                }

                return isAccessible;
            }

            //return base.IsAccessibleToUser(context, node);
        }

        #region Helper Methods
        // Helper methods
        private SiteMapNode CreateSiteMapNodeFromDataReader(DbDataReader reader)
        {
            // Make sure the node ID is present
            if (reader.IsDBNull(_indexID))
                throw new ProviderException(_errmsg1);

            // Get the node ID from the DataReader
            int id = reader.GetInt32(_indexID);

            // Make sure the node ID is unique
            if (_nodes.ContainsKey(id))
            {
                throw new ProviderException(_errmsg2);
            }

            // Get title, URL, description, and roles from the DataReader
            string title = reader.IsDBNull(_indexTitle) ? null : reader.GetString(_indexTitle).Trim();
            string url = reader.IsDBNull(_indexUrl) ? null : reader.GetString(_indexUrl).Trim();
            string description = reader.IsDBNull(_indexDesc) ? null : reader.GetString(_indexDesc).Trim();
            //string roles = reader.IsDBNull(_indexRoles) ? null : reader.GetString(_indexRoles).Trim();

            // If roles were specified, turn the list into a string array
            /*
            string[] rolelist = null;
            if (!String.IsNullOrEmpty(roles))
                rolelist = roles.Split(new char[] { ',', ';' }, 512);*/

            // Create a SiteMapNode
            // Modified by Rajesh - Do not add the roles here. It is added for each row in the result set.
            //SiteMapNode node = new SiteMapNode(this, id.ToString(), url, title, description, rolelist, null, null, null);

            String menuImageURL = reader.IsDBNull(_indexMenuImageURL) ? null : reader.GetString(_indexMenuImageURL).Trim();

            NameValueCollection customAttr = new NameValueCollection();

            if (!String.IsNullOrEmpty(menuImageURL))
            {
                customAttr.Add("MenuImageURL", menuImageURL);
            }

            SiteMapNode node = new SiteMapNode(this, id.ToString(), url, title, description, null, customAttr, null, null);

            string role = reader.IsDBNull(_indexRoles) ? null : reader.GetString(_indexRoles).Trim();
            node.Roles = new List<String>();
            if (!String.IsNullOrEmpty(role))
            {
                node.Roles.Add(role);
            }

            // Record the node in the _nodes dictionary
            _nodes.Add(id, node);


            // Return the node        
            return node;
        }

        private SiteMapNode GetParentNodeFromDataReader(DbDataReader reader)
        {
            // Make sure the parent ID is present
            if (reader.IsDBNull(_indexParent))
                throw new ProviderException(_errmsg3);

            // Get the parent ID from the DataReader
            int pid = reader.GetInt32(_indexParent);

            // Make sure the parent ID is valid
            if (!_nodes.ContainsKey(pid))
                throw new ProviderException(_errmsg4);

            // Return the parent SiteMapNode
            return _nodes[pid];
        }

        void OnSiteMapChanged(string key, object item, CacheItemRemovedReason reason)
        {
            lock (_lock)
            {
                if (key == _cacheDependencyName && reason == CacheItemRemovedReason.DependencyChanged)
                {
                    // Refresh the site map
                    Clear();
                    _nodes.Clear();
                    _root = null;
                }
                else if (key == _cacheDependencyName && reason == CacheItemRemovedReason.Removed)
                {
                    // Refresh the site map
                    Clear();
                    _nodes.Clear();
                    _root = null;
                }

            }
        }
        #endregion
    }
}
