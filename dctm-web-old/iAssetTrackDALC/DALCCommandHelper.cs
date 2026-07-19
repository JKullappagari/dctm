using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace iAssetTrack.DALC
{
    [Serializable]
    public class DALCCommandHelper
    {
        private string storedProcName;
        private DALCResultType resultType;
        private object[] seqParameters;
        private List<namedParameter> namedParameters;
        private Dictionary<string, object> outputParameters;

        private struct namedParameter
        {
            private string pName;
            private object paramvalue;
            private DbType type;
            private ParameterDirection direction;
            private int size;

            public string ParameterName
            {
                get
                {
                    return this.pName;
                }
                set
                {
                    this.pName = value;
                }
            }

            public object ParameterValue
            {
                get
                {
                    return this.paramvalue;
                }
                set
                {
                    this.paramvalue = value;
                }
            }

            public DbType ParameterType
            {
                get
                {
                    return this.type;
                }
                set
                {
                    this.type = value;
                }
            }

            public ParameterDirection Direction
            {
                get
                {
                    return this.direction;
                }
                set
                {
                    this.direction = value;
                }
            }

            public int ParameterSize
            {
                get
                {
                    return this.size;
                }
                set
                {
                    this.size = value;
                }
            }
        }

        public Dictionary<string, object> OutputParameters
        {
            get
            {
                return this.outputParameters;
            }
        }

        public DALCCommandHelper(string spName, DALCResultType resultType)
        {
            this.storedProcName = spName;
            this.resultType = resultType;
        }

        public DALCCommandHelper(string spName, DALCResultType resultType, params object[] seqParameters)
            : this(spName, resultType)
        {
            this.seqParameters = seqParameters;
        }

        public void AddParameterSeqParams(params object[] seqParameters)
        {
            if (this.namedParameters != null)
            {
                throw new Exception("Cannot initialized sequence and named parameters at the sametime");
            }

            this.seqParameters = seqParameters;
        }


        public void AddParameter(string pName, DbType type, object value, int size, ParameterDirection direction)
        {
            if (this.seqParameters != null)
            {
                throw new Exception("Cannot initialized sequence and named parameters at the sametime");
            }

            if (this.namedParameters == null)
            {
                this.namedParameters = new List<namedParameter>();
            }

            namedParameter p = new namedParameter();
            p.Direction = direction;
            p.ParameterName = pName;
            p.ParameterType = type;
            p.ParameterValue = value;
            p.ParameterSize = size;
            this.namedParameters.Add(p);
        }


        public void AddInParameter(string pName, DbType type, object value)
        {
            this.AddParameter(pName, type, value, 0, ParameterDirection.Input);
        }

        public void AddOutParameter(string pName, DbType type, object value, int size)
        {
            this.AddParameter(pName, type, value, size, ParameterDirection.Output);
        }

        public static void ExecuteBatchCommands(List<DALCCommandHelper> commandHelpers)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection conn = db.CreateConnection();
            conn.Open();
            DbTransaction trans = conn.BeginTransaction();
            try
            {
                foreach (DALCCommandHelper cmd in commandHelpers)
                {
                    cmd.ExecuteCommand(db);
                    
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (conn != null)
                    conn.Dispose();
            }
        }

        public object ExecuteCommand(Database db)
        {
            //Database db = DatabaseFactory.CreateDatabase();
            DbCommand cmd;
            object commandResult = null;

            if (this.namedParameters == null)
            {
                if(this.seqParameters == null)
                    cmd = db.GetStoredProcCommand(this.storedProcName);
                else
                    cmd = db.GetStoredProcCommand(this.storedProcName, this.seqParameters);
            }
            else
            {
                cmd = db.GetStoredProcCommand(this.storedProcName);
                foreach (namedParameter p in this.namedParameters)
                {
                    if (p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput)
                        db.AddInParameter(cmd, p.ParameterName, p.ParameterType, p.ParameterValue);
                    else
                        db.AddOutParameter(cmd, p.ParameterName, p.ParameterType, p.ParameterSize);

                    cmd.Parameters[p.ParameterName].Direction = p.Direction;
                }
            }

            cmd.CommandTimeout = 180000;
            if (this.resultType == DALCResultType.DataSet)
            {
                commandResult = db.ExecuteDataSet(cmd);                
            }
            if (this.resultType == DALCResultType.DataReader)
            {
                commandResult = db.ExecuteReader(cmd);
            }
            if (this.resultType == DALCResultType.Scalar)
            {
                commandResult = db.ExecuteScalar(cmd);
            }
            if (this.resultType == DALCResultType.None)
            {
                commandResult = db.ExecuteNonQuery(cmd);
            }

            if (this.namedParameters != null)
            {
                if (this.outputParameters == null)
                {
                    this.outputParameters = new Dictionary<string, object>();
                }
                foreach (namedParameter p in this.namedParameters)
                {
                    if (p.Direction == ParameterDirection.Output || p.Direction == ParameterDirection.InputOutput)
                    {
                        this.outputParameters.Add(p.ParameterName, db.GetParameterValue(cmd, p.ParameterName));
                    }
                }
            }


            return commandResult;

        }

        public object ExecuteCommand()
        {
            Database db = DatabaseFactory.CreateDatabase();
            return this.ExecuteCommand(db);
        }
    }
}
