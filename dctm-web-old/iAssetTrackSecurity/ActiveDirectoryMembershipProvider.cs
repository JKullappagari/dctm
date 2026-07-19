/*
File Name:      ADConfigurationSettings.cs
Description:	Class to define methods for reading and persisting AD configuration settings.
Date created:	23 Jul 2007

Modification History:
*********************
CR		Name			            Date			Description
New		Sivashanmugam, Muniappan    23/07/2007		File has been created.
Changed	Sivashanmugam, Muniappan    03/10/2007		File has been modified to add new classes.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;

[Flags]
public enum ADS_USER_FLAG_ENUM
{
    ADS_UF_SCRIPT = 1,                                          // 0x1
    ADS_UF_ACCOUNTDISABLE = 2,                                  // 0x2
    ADS_UF_HOMEDIR_REQUIRED = 8,                                // 0x8
    ADS_UF_LOCKOUT = 16,                                        // 0x10
    ADS_UF_PASSWD_NOTREQD = 32,                                 // 0x20
    ADS_UF_PASSWD_CANT_CHANGE = 64,                             // 0x40
    ADS_UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 128,               // 0x80
    ADS_UF_TEMP_DUPLICATE_ACCOUNT = 256,                        // 0x100
    ADS_UF_NORMAL_ACCOUNT = 512,                                // 0x200
    ADS_UF_INTERDOMAIN_TRUST_ACCOUNT = 2048,                    // 0x800
    ADS_UF_WORKSTATION_TRUST_ACCOUNT = 4096,                    // 0x1000
    ADS_UF_SERVER_TRUST_ACCOUNT = 8192,                         // 0x2000
    ADS_UF_DONT_EXPIRE_PASSWD = 65536,                          // 0x10000
    ADS_UF_MNS_LOGON_ACCOUNT = 131072,                          // 0x20000
    ADS_UF_SMARTCARD_REQUIRED = 262144,                         // 0x40000
    ADS_UF_TRUSTED_FOR_DELEGATION = 524288,                     // 0x80000
    ADS_UF_NOT_DELEGATED = 1048576,                             // 0x100000
    ADS_UF_USE_DES_KEY_ONLY = 2097152,                          // 0x200000
    ADS_UF_DONT_REQUIRE_PREAUTH = 4194304,                      // 0x400000
    ADS_UF_PASSWORD_EXPIRED = 8388608,                          // 0x800000
    ADS_UF_TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 16777216    // 0x1000000
}

namespace iAssetTrack.Security
{
    #region Custom Namespaces

    using a = iAssetTrack.Security.ConfigAttributeConstants;
    using l = iAssetTrack.Security.ConfigElementConstants;
    using t = iAssetTrack.Security.MembershipAttributeConstants;
    using e = iAssetTrack.Security.MembershipErrorConstants;

    #endregion

    public class ActiveDirectoryMembershipProvider
    {
        #region Fields

        private DomainCollection _domains = new DomainCollection();
        private String _defaultDomain = String.Empty;

        #endregion

        #region Properties

        public DomainCollection Domains
        {
            get { return _domains; }
        }

        public string DefaultDomain
        {
            get { return _defaultDomain; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the settings from configuartion file originating from root node.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        public void LoadValuesFromConfigurationXml(XmlNode node)
        {
            _defaultDomain = node.Attributes[a.defaultDomain].Value;

            foreach (XmlNode child in node.ChildNodes)
            {
                if ((child.NodeType == XmlNodeType.Element) && (child.NodeType != XmlNodeType.Whitespace))
                {
                    if (child.Name == l.domains) GetDomains(child, _defaultDomain);
                }
            }
        }

        /// <summary>
        /// Adds domains as specified in configuration file to DomainCollection.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private void GetDomains(XmlNode node, String defaultDomain)
        {
            foreach (XmlNode domain in node.ChildNodes)
            {
                switch (domain.Name)
                {
                    case l.add:
                        _domains.Add(new Domain(domain, defaultDomain));
                        break;

                    case l.remove:
                        _domains.Remove(domain.Attributes[a.name].Value);
                        break;

                    case l.clear:
                        _domains.Clear();
                        break;
                }
            }
        }

        #endregion
    }

    public class Domain : IMembershipProvider
    {
        #region Fields

        string _name = String.Empty;
        string _domainName = String.Empty;
        bool _defaultDomain = false;
        private ADServerCollection _servers = new ADServerCollection();
        private const String SearchSingleUserByID = "(&(&(objectClass=user)(objectCategory=person))(|(sAMAccountName={0})))";
        private const String SearchSingleUserByClass = "(&(objectClass=user)(sAMAccountName={0}))";

        /// <summary>
        /// Defines the properties to load from the user object.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private readonly String[] _propertiesToLoad = 
        {
            t.samAccountName,       // samAccountName
            t.givenName,            // givenName
            t.displayName,          // displayName
            t.initials,             // initials             
            t.mail,                 // mail
            t.pwdLastSet,           // pwdLastSet
            t.objectGuid,           // objectGuid
            t.userAccountControl,   // userAccountControl
            t.userPrincipalName     // userPrincipalName
        };
        
        #endregion

        #region COM Imports

        /// <summary>
        /// COM Import to define IADsLargeInteger interface for handling large integer type.
        /// </summary>
        [ComImport]
        [Guid("9068270B-0939-11D1-8BE1-00C04FD8D503")]
        [InterfaceType(ComInterfaceType.InterfaceIsDual)]
        internal interface IADsLargeInteger
        {
            [DispId(0x00000002)]
            int HighPart { get; set;}
            [DispId(0x00000003)]
            int LowPart { get; set;}
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Overridden Constructor for Domain class which loads domain servers as specified in configuration file.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        public Domain(XmlNode node, String defaultDomain)
        {
            _name = node.Attributes[a.name].Value;
            _domainName = node.Attributes[a.domainName].Value;
            _defaultDomain = (defaultDomain == _name)? true : false;

            foreach (XmlNode child in node.ChildNodes)
            {
                if ((child.NodeType == XmlNodeType.Element) && (child.NodeType != XmlNodeType.Whitespace))
                {
                    if (child.Name == l.servers) GetServers(child);
                }
            }
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
        }

        public string DomainName
        {
            get { return _domainName; }
        }

        public bool DefaultDomain
        {
            get { return _defaultDomain; }
        }

        public ADServerCollection Servers
        {
            get { return _servers; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Adds domain servers as specified in configuration file to ServerCollection.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private void GetServers(XmlNode node)
        {
            foreach (XmlNode server in node.ChildNodes)
            {
                switch (server.Name)
                {
                    case l.add:
                        _servers.Add(new ADServer(server.Attributes));
                        break;

                    case l.remove:
                        _servers.Remove(server.Attributes[a.name].Value);
                        break;

                    case l.clear:
                        _servers.Clear();
                        break;
                }
            }
        }

        /// <summary>
        /// Gets the attribute value by reading appropriate Directory attributes.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private object GetAttributeValue(String name, Hashtable attributes)
        {
            ADS_USER_FLAG_ENUM flag;
            if (attributes[name] == null)
            {
                switch (name)
                {
                    case t.userAccountDisabled:
                        flag = (ADS_USER_FLAG_ENUM)attributes[t.userAccountControl];
                        if ((flag & ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE) ==
                            ADS_USER_FLAG_ENUM.ADS_UF_ACCOUNTDISABLE)
                            return true;
                        else
                            return false;
                    case t.accountLocked:
                        flag = (ADS_USER_FLAG_ENUM)attributes[t.userAccountControl];
                        if ((flag & ADS_USER_FLAG_ENUM.ADS_UF_LOCKOUT) ==
                            ADS_USER_FLAG_ENUM.ADS_UF_LOCKOUT)
                            return true;
                        else
                            return false;
                    case t.pwdNeverExpires:
                        flag = (ADS_USER_FLAG_ENUM)attributes[t.userAccountControl];
                        if ((flag & ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD) ==
                            ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD)
                            return true;
                        else
                            return false;
                    default:
                        return attributes[name];
                }
            }
            return attributes[name];
        }

        /// <summary>
        /// Sets the propertyName in properties' with given propertyValue.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Jul 2007</createdOn>
        private void SetProperty(String propertyName, ref Hashtable properties, object propertyValue)
        {
            if (propertyValue != null)
            {
                if (propertyValue is String)
                {
                    if (!propertyValue.Equals(""))
                    {
                        properties[propertyName] = propertyValue;
                    }
                    else
                    {
                        if (properties.Contains(propertyName))
                            properties[propertyName] = "";
                    }
                }
                else
                {
                    properties[propertyName] = propertyValue;
                }
            }
            else
            {
                if (properties.Contains(propertyName))
                    properties[propertyName] = null;
            }
        }
        
        /// <summary>
        /// Fills the attributes in from SearchResult after applying the queryFilter.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private void FillAttributes(SearchResult result, ref Hashtable attributes)
        {
            foreach (String propertyName in result.Properties.PropertyNames)
            {
                ResultPropertyValueCollection propertyValues = result.Properties[propertyName];

                object propertyValue;
                if (propertyValues.Count == 1)
                    propertyValue = propertyValues[0];
                else
                {
                    object[] values = new object[propertyValues.Count];
                    propertyValues.CopyTo(values, 0);
                    propertyValue = values;
                }

                if (propertyValue is IADsLargeInteger)
                {
                    IADsLargeInteger int64Val = (IADsLargeInteger)propertyValue;
                    attributes[propertyName] = int64Val.HighPart * 0x100000000 + int64Val.LowPart;
                }
                else
                    attributes[propertyName] = propertyValue;
            }

            if (attributes.ContainsKey(t.objectClass))
            {
                object[] objectClass = (object[])attributes[t.objectClass];
                attributes[t.objectClass] = (String)objectClass[objectClass.Length - 1];
            }
        }

        /// <summary>
        /// Copies the properties to load from _propertiesToLoad private variable.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private String[] GetPropertiesToLoad()
        {
            String[] propertiesToLoad;

            propertiesToLoad = new String[_propertiesToLoad.Length];
            _propertiesToLoad.CopyTo(propertiesToLoad, 0);
            return propertiesToLoad;
        }

        /// <summary>
        /// Performs a subtree search to locate user object using FindOne method.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private Hashtable FindSingleObject(String queryFilter, String[] propertiesToLoad)
        {
            DirectoryEntry entry = null;
            DirectorySearcher search = null;
            try
            {
                entry = Bind(new String[] { t.distinguishedName }, AuthenticationTypes.FastBind);
                search = new DirectorySearcher(entry);
                search.Filter = queryFilter;
                search.PropertiesToLoad.AddRange(propertiesToLoad);
                search.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                SearchResult result = search.FindOne();

                if (result != null)
                {
                    Hashtable attributes = new Hashtable(propertiesToLoad.Length, StringComparer.OrdinalIgnoreCase);
                    FillAttributes(result, ref attributes);
                    return attributes;
                }
                return null;
            }
            catch (COMException c)
            {
                throw new Exception(c.Message);
            }
            finally
            {
                if (entry != null) { entry.Close(); entry.Dispose(); }
                if (search != null) search.Dispose();
            }
        }

        /// <summary>
        /// Returns the User Proncipal Name as given by [Domain Name\sAMaccountName]
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private String GetUserPrincipalName(String userName)
        {
            return String.Join(@"\", new String[] { this.DomainName, userName });
        }

        private DirectoryEntry Bind(String[] propertiesToLoad, AuthenticationTypes authType)
        {
            return Bind(null, null, propertiesToLoad, authType);
        }

        /// <summary>
        /// Binds user. If user credentials are null, service account is used to find user container.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private DirectoryEntry Bind(String userName, String password, String[] propertiesToLoad, 
            AuthenticationTypes authType)
        {
            DirectoryEntry entry = null;
            object obj = null;

            if ((userName == null) ^ (password == null))
                throw new InvalidOperationException("Invalid userName or password null parameter.");  

            foreach (ADServer server in this.Servers)
            {
                String ldapPath = server.ConnectionString;
                try
                {
                    if (userName == null) userName = server.ConnectionUsername;
                    if (password == null) password = server.ConnectionPassword;

                    entry = new DirectoryEntry();
                    entry.Path = ldapPath;
                    entry.Username = userName;
                    entry.Password = password;
                    obj = entry.NativeObject;

                    if (authType != AuthenticationTypes.None)
                        entry.AuthenticationType = authType;

                    if (propertiesToLoad != null)
                        entry.RefreshCache(propertiesToLoad);
                    else
                        entry.RefreshCache();

                    return entry;
                }
                catch (COMException)
                {
                    // Skip and continue with the next available server.
                }
            }
            if (entry == null) throw new Exception();
            return entry;
        }

        /// <summary>
        /// Gets the AD server connection by logging on to the server by using network credentials of user.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private DirectoryConnection GetConnection(String server, NetworkCredential credential,
            bool useSSL)
        {
            try
            {
                LdapConnection connection = new LdapConnection(server);
                if (useSSL)
                    connection.SessionOptions.SecureSocketLayer = true;
                else
                    connection.SessionOptions.Sealing = true;

                connection.Bind(credential);
                return connection;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a formatted password in Unicode encoding to set user password property while changing. 
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        private static byte[] GetPasswordData(string password)
        {
            string formattedPassword;
            formattedPassword = String.Format("\"{0}\"", password);
            return (Encoding.Unicode.GetBytes(formattedPassword));
        }

        private String EscapeFromAsteriskChar(String criteria)
        {
            return criteria.Replace(@"*", @"\2a");
        }

        private String EscapeFromNullChar(String criteria)
        {
            if (criteria.Trim().Length == 0)
            {
                return @"\00";
            }
            return criteria;
        }

        private String EscapeQueryFilterCriteria(String criteria)
        {
            if (criteria.Length == 0)
            {
                criteria = criteria.Replace(@"\", @"\00");
            }
            else
            {
                criteria = criteria.Replace(@"\", @"\5c");
                criteria = criteria.Replace(@"/", @"\2f");
                criteria = criteria.Replace(@"(", @"\28");
                criteria = criteria.Replace(@")", @"\29");
            }
            return criteria;
        }
        
        #endregion

        #region IMembershipProvider Members

        /// <summary>
        /// Validates user in given domain instance given by userName and password. Binding will be done using user
        /// credentials.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        public bool ValidateUser(string userName, string password)
        {
            DirectoryEntry entry = null;
            try
            {
                entry = Bind(GetUserPrincipalName(userName), password, new String[] { a.name }, 
                    AuthenticationTypes.ServerBind);
                entry.RefreshCache(new String[] { a.name });
            }
            catch
            {
                return false;
            }
            finally
            {
                entry.CommitChanges();
                if (entry != null) { entry.Close(); entry.Dispose(); }
            }
            return true;
        }

        /// <summary>
        /// Changes user password in given domain instance given by userName, currentPassword and newPassword.
        /// </summary>
        /// <author>Sivashanmugam, Muniappan</author>
        /// <createdOn>03 Oct 2007</createdOn>
        public void ChangePassword(string userName, string currentPassword, string newPassword)
        {
            DirectoryConnection connection = null;
            bool _changePassword = false;

            if ((userName == null) ^ (currentPassword == null))
                throw new InvalidOperationException("Invalid userName or password null parameter.");  

            try
            {
                NetworkCredential credential = new NetworkCredential(userName, currentPassword, this.DomainName);
                Hashtable attributes = FindSingleObject(
                    String.Format(SearchSingleUserByClass, EscapeFromNullChar(EscapeFromAsteriskChar(EscapeQueryFilterCriteria(userName)))),
                    new String[] { }
                    );

                foreach (ADServer server in this.Servers)
                {
                    try
                    {
                        connection = GetConnection(server.Name, credential, false);

                        DirectoryAttributeModification deleteMod = new DirectoryAttributeModification();
                        deleteMod.Name = "unicodePwd";
                        deleteMod.Add(GetPasswordData(currentPassword));
                        deleteMod.Operation = DirectoryAttributeOperation.Delete;

                        DirectoryAttributeModification addMod = new DirectoryAttributeModification();
                        addMod.Name = "unicodePwd";
                        addMod.Add(GetPasswordData(newPassword));
                        addMod.Operation = DirectoryAttributeOperation.Add;

                        String distinguishedName = (String)attributes[t.distinguishedName];
                        ModifyRequest request = new ModifyRequest(distinguishedName, deleteMod, addMod);
                        DirectoryResponse response = connection.SendRequest(request);

                        _changePassword = true;
                    }
                    catch
                    {
                        // Skip and continue with the next available server.
                    }
                }
                if (!_changePassword) throw new Exception();
            }
            catch (Exception)
            {
                throw new MembershipException(e.ADERR_PWDCHANGE_FAILURE_CANNOT_CHANGE_PWD);
            }
            finally
            {
                IDisposable disposable = connection as IDisposable;
                if (disposable != null) disposable.Dispose();
            }
        }

        /// <summary>
        /// Gets user given by userName and returns MembershipUser object. Binding is done using service account 
        /// credentials.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public MembershipUser GetUser(string userName)
        {
            try
            {
                String[] propertiesToLoad = GetPropertiesToLoad();
                Hashtable attributes = FindSingleObject(
                    String.Format(SearchSingleUserByID, EscapeFromNullChar(EscapeFromAsteriskChar(EscapeQueryFilterCriteria(userName)))),
                    propertiesToLoad
                    );

                bool userAccountDisabled = (bool)GetAttributeValue(t.userAccountDisabled, attributes);
                SetProperty(t.userAccountDisabled, ref attributes, userAccountDisabled);

                bool accountLocked = (bool)GetAttributeValue(t.accountLocked, attributes);
                SetProperty(t.accountLocked, ref attributes, accountLocked);

                long pwdLastSet = (long)GetAttributeValue(t.pwdLastSet, attributes);
                SetProperty(t.pwdLastSet, ref attributes, DateTime.FromFileTime(pwdLastSet));

                bool pwdNeverExpires = (bool)GetAttributeValue(t.pwdNeverExpires, attributes);
                SetProperty(t.pwdNeverExpires, ref attributes, pwdNeverExpires);

                return new MembershipUser(attributes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }

    [Serializable]
    public class DomainCollection : DictionaryBase
    {
        #region Methods

        internal void Add(Domain value)
        {
            Dictionary.Add(value.Name, value);
        }

        #endregion

        #region DictionaryBase Members

        internal void Add(string key, object value) { }

        new public void Clear() { }

        new public IEnumerator GetEnumerator()
        {
            return new DomainEnumerator(Dictionary);
        }

        internal void Remove(string key) { }

        public bool Contains(string name)
        {
            return Dictionary.Contains(name);
        }

        public Domain this[string name]
        {
            get { return (Domain)Dictionary[name]; }
        }

        #endregion
    }

    [Serializable]
    public class DomainEnumerator : IEnumerator
    {
        private IDictionaryEnumerator _enumerator;

        #region Constructor

        public DomainEnumerator(IDictionary dictionary)
        {
            _enumerator = dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerator Members

        public void Reset()
        {
            _enumerator.Reset();
        }

        public object Current
        {
            get
            {
                return (Domain)((DictionaryEntry)_enumerator.Current).Value;
            }
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        #endregion
    }

    public class ADServer
    {
        #region Fields

        String _name = String.Empty;
        String _connectionString = String.Empty;
        String _connectionUsername = String.Empty;
        String _connectionPassword = String.Empty;
        
        #endregion

        #region Constructors

        public ADServer(XmlAttributeCollection attributes)
        {
            _name = attributes[a.name].Value;
            _connectionString = attributes[a.connectionString].Value;
            _connectionUsername = attributes[a.connectionUsername].Value;
            _connectionPassword = attributes[a.connectionPassword].Value;
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public string ConnectionUsername
        {
            get { return _connectionUsername; }
        }

        public string ConnectionPassword
        {
            get { return _connectionPassword; }
        }

        #endregion
    }

    [Serializable]
    public class ADServerCollection : DictionaryBase
    {
        #region Methods

        internal void Add(ADServer value)
        {
            Dictionary.Add(value.Name, value);
        }

        #endregion

        #region DictionaryBase Members

        internal void Add(string key, object value) { }

        new public void Clear() { }

        new public IEnumerator GetEnumerator()
        {
            return new ADServerEnumerator(Dictionary);
        }

        internal void Remove(string key) { }

        public bool Contains(string name)
        {
            return Dictionary.Contains(name);
        }

        public ADServer this[string name]
        {
            get { return (ADServer)Dictionary[name]; }
        }

        #endregion
    }

    [Serializable]
    public class ADServerEnumerator : IEnumerator
    {
        private IDictionaryEnumerator _enumerator;

        #region Constructor

        public ADServerEnumerator(IDictionary dictionary)
        {
            _enumerator = dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerator Members

        public void Reset()
        {
            _enumerator.Reset();
        }

        public object Current
        {
            get
            {
                return (ADServer)((DictionaryEntry)_enumerator.Current).Value;
            }
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        #endregion
    }

     /// <summary>
    /// Internal ConfigAttributeConstants class to define constants for AD configuration settings.
    /// </summary>
    internal class ConfigAttributeConstants
    {
        internal const string name = "name";
        internal const string domainName = "domainName";
        internal const string defaultDomain = "defaultDomain";
        internal const string connectionString = "connectionString";
        internal const string connectionUsername = "connectionUsername";
        internal const string connectionPassword = "connectionPassword";
    }

    /// <summary>
    /// Internal ConfigElementConstants class to define constants for AD configuration elements.
    /// </summary>
    internal class ConfigElementConstants
    {
        internal const string domains = "domains";
        internal const string servers = "servers";
        internal const string add = "add";
        internal const string remove = "remove";
        internal const string clear = "clear";
    }
}
