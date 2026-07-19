using System;


namespace iAssetTrack.DALC
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DALCOperationSPAttribute : Attribute
    {
        string insertSP, updateSP, deleteSP, upositionUpdateSP;

        public DALCOperationSPAttribute() : base()
        {
            this.insertSP = "";
            this.updateSP = "";
            this.deleteSP = "";
            this.upositionUpdateSP = "";
        }
        
        public string InsertSP
        {
            get
            {
                return this.insertSP;
            }
            set
            {
                this.insertSP = value;
            }
        }



        public string UpdateSP
        {
            get
            {
                return this.updateSP;
            }
            set
            {
                this.updateSP = value;
            }
        }

        public string DeleteSP
        {
            get
            {
                return this.deleteSP;
            }
            set
            {
                this.deleteSP = value;
            }
        }

        public string BladeUpdateSP
        {
            get
            {
                return this.updateSP;
            }
            set
            {
                this.updateSP = value;
            }
        }

        public string UPositionUpdateSPC
        {
            get
            {
                return this.upositionUpdateSP;
            }
            set
            {
                this.upositionUpdateSP = value;
            }
        }
       
    }
}
