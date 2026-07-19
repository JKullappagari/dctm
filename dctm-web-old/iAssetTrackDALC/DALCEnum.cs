namespace iAssetTrack.DALC
{
    public enum DALCOperation
    {
        Insert = 1,
        Update = 2,
        Delete = 3
    }

    public enum DALCResultType
    {
        DataSet = 1,
        Scalar = 2,
        DataReader = 3,
        None = 4
    }
}