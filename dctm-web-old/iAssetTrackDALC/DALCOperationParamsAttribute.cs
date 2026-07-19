using System;
using System.Data;

namespace iAssetTrack.DALC
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Property, Inherited = false,AllowMultiple = true)]
    public class DALCOperationParamsAttribute : Attribute
    {
        private DALCOperation? operation;
        private string paramName;
        private ParameterDirection direction;
        private DbType dbType;
        private int size;
        
        public DALCOperationParamsAttribute() : base()
        {
            this.direction = System.Data.ParameterDirection.Input;
            this.paramName = "";
            this.operation = null;
            this.dbType = DbType.String;
        }                

        public object DataType
        {
            get
            {
                return this.dbType;
            }
            set
            {
                this.dbType = (DbType)value;
            }
        }
        
        public string ParameterName
        {
            get
            {
                return this.paramName;
            }
            set
            {
                this.paramName = value;
            }
        }
        
        public DALCOperation DALCOperation
        {
            get
            {
                return (DALCOperation)this.operation;
            }
            set
            {
                this.operation = value;
            }
        }

        public object ParameterDirection
        {
            get
            {
                return this.direction;
            }
            set
            {
                this.direction = (ParameterDirection)value;
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
}
