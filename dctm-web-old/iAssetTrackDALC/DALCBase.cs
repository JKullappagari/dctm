using System.Reflection;
using System.Data;
using System.Data.Common;
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace iAssetTrack.DALC
{
    [Serializable]
    public class DALCBase<T>
    {
        private int userID;
        private string insertSP, updateSP, deleteSP;
        private T entity;

        private DALCCommandHelper dalcHelper;

        private Database db;

        private Dictionary<string, PropertyInfo> outputPropertyName;

        public DALCBase(T entityParam)
        {
            this.entity = entityParam;
        }

        private void CollectParameters(DALCOperation operation)
        {
            this.outputPropertyName = new Dictionary<string, PropertyInfo>();

            string paramName = "";
            DbType type = DbType.String;
            //object value = null;
            int size = 0;
            ParameterDirection direction = ParameterDirection.Input;

            foreach (PropertyInfo p in this.entity.GetType().GetProperties())
            {
                DALCOperationParamsAttribute[] attrib =
                    (DALCOperationParamsAttribute[])p.GetCustomAttributes(typeof(DALCOperationParamsAttribute), false);

                if (attrib.Length > 0)
                {
                    for (int i = 0; i <= attrib.Length - 1; i++)
                    {
                        if (attrib[i].DALCOperation == operation)
                        {
                            paramName = attrib[i].ParameterName;
                            type = (DbType)attrib[i].DataType;
                            size = attrib[i].ParameterSize;
                            direction = (ParameterDirection)attrib[i].ParameterDirection;

                            if (direction == ParameterDirection.Output || direction == ParameterDirection.InputOutput)
                            {
                                if (!this.outputPropertyName.ContainsKey(attrib[i].ParameterName))
                                    this.outputPropertyName.Add(attrib[i].ParameterName, p);
                            }

                            if (direction == ParameterDirection.Input)
                            {
                                this.dalcHelper.AddInParameter(paramName, type, p.GetValue(this.entity, null));
                            }
                            else
                            {
                                if (direction == ParameterDirection.Output)
                                    this.dalcHelper.AddOutParameter(paramName, type, p.GetValue(this.entity, null), size);
                                else
                                    this.dalcHelper.AddParameter(paramName, type, p.GetValue(this.entity, null), size, direction);
                            }

                            break;
                        }
                    }
                }
            }
        }

        protected Database DB
        {
            get
            {
                return this.db;
            }
            set
            {
                this.db = value;
            }
        }

        private void PrepareCommand(DALCOperation operation)
        {
            if (this.db == null)
                this.dalcHelper.ExecuteCommand();
            else
                this.dalcHelper.ExecuteCommand(db);

            if (this.dalcHelper.OutputParameters.Count > 0)
            {
                object retval;

                foreach (string key in this.dalcHelper.OutputParameters.Keys)
                {
                    retval = this.dalcHelper.OutputParameters[key];
                    ((PropertyInfo)this.outputPropertyName[key]).SetValue(this.entity, retval, null);
                }
            }
        }

        private void ExecuteCRUD(DALCOperation operation)
        {
            this.PrepareCommand(operation);
        }

        private void InsertData()
        {
            this.CollectParameters(DALCOperation.Insert);
            this.ExecuteCRUD(DALCOperation.Insert);
        }

        private void UpdateData()
        {
            this.CollectParameters(DALCOperation.Update);
            this.ExecuteCRUD(DALCOperation.Update);
        }

        private void DeleteData()
        {
            this.CollectParameters(DALCOperation.Delete);
            this.ExecuteCRUD(DALCOperation.Delete);

        }

        public static bool SaveBatchData(List<T> entity, DALCOperation operation, int userID)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection conn = db.CreateConnection();
            DbTransaction trans = null;
            DALCBase<object> dalc;
            bool retval = false;
            try
            {
                conn.Open();
                trans = conn.BeginTransaction();

                foreach (object ent in entity)
                {
                    dalc = new DALCBase<object>(ent);
                    dalc.DB = db;
                    dalc.SaveData(operation, userID);
                }
                trans.Commit();
                retval = true;
            }
            catch
            {
                if (trans != null)
                    trans.Rollback();
                retval = false;
                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
            }

            return retval;
        }

        public void SaveData(DALCOperation operation, int pUserID)
        {
            this.userID = pUserID;

            DALCOperationSPAttribute[] attrib =
                (DALCOperationSPAttribute[])
                this.entity.GetType().GetCustomAttributes(typeof(DALCOperationSPAttribute), false);

            //Stored procedure attribute is not declared
            if (attrib.Length == 0)
            {
                return;
            }

            //If declared, get all of the attribs
            this.insertSP = attrib[0].InsertSP;
            this.updateSP = attrib[0].UpdateSP;
            this.deleteSP = attrib[0].DeleteSP;



            //Call private member respectively
            if (operation == DALCOperation.Insert)
            {
                this.dalcHelper = new DALCCommandHelper(this.insertSP, DALCResultType.None);
                this.InsertData();
            }
            if (operation == DALCOperation.Update)
            {
                this.dalcHelper = new DALCCommandHelper(this.updateSP, DALCResultType.None);
                this.UpdateData();
            }
            if (operation == DALCOperation.Delete)
            {
                this.dalcHelper = new DALCCommandHelper(this.deleteSP, DALCResultType.None);
                this.DeleteData();
            }
        }
    }
}
