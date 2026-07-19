using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace iAssetTrack.DALC
{
    public class DALCMetaDataHelper
    {

        public static DataSet GetDataSet(String baseTableName, String paramValue)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbConnection conn = db.CreateConnection();
            conn.Open();

            String sqlWithArg = String.Format(GetSQLString(baseTableName), paramValue);

            DbCommand command = db.GetSqlStringCommand(sqlWithArg);
            return db.ExecuteDataSet(command);
            
        }


        public static DataSet GetTableMetaData(String baseTableName)
        {

            DALCCommandHelper criteria = new DALCCommandHelper("iAssetTrack_Sp_MetaData_GetTableColumns", DALCResultType.DataSet);
            criteria.AddInParameter("@pTableName", DbType.String, baseTableName);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return (ds);

        }


        public static String GetTableNameFromAttributeId(int attributeId)
        {
            DALCCommandHelper criteria = new DALCCommandHelper("iAssetTrack_Sp_MetaData_GetTableNameByAttributeId", DALCResultType.DataSet);
            criteria.AddInParameter("@pAttributeId", DbType.Int32, attributeId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();

            string tableName = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                tableName = ds.Tables[0].Rows[0]["TableName"].ToString();
            }

            return (tableName);
        }


        public static DataSet GetColumnDetails(int attributeId)
        {
            DALCCommandHelper criteria = new DALCCommandHelper("iAssetTrack_Sp_MetaData_GetColumnDetails", DALCResultType.DataSet);
            criteria.AddInParameter("@pAttributeId", DbType.Int32, attributeId);
            DataSet ds = (DataSet)criteria.ExecuteCommand();
            return (ds);
        }

        public static String GetSQLString(String baseTableName)
        {
            String columns = "";
            String tables = "";
            String joins = "";
            String where = "";
            GetPartialSQLString(baseTableName, ref columns, ref tables, ref joins, ref where, true);

            char[] del = new char[1] { ',' };

            String[] columnList = columns.Split(del, StringSplitOptions.RemoveEmptyEntries);
            columns = String.Join(",", columnList);
            String[] tableList = tables.Split(del, StringSplitOptions.RemoveEmptyEntries);
            tables = String.Join(",", tableList);
            String[] joinList = joins.Split(del, StringSplitOptions.RemoveEmptyEntries);
            joins = String.Join(" ", joinList);

            return "SELECT " + columns + " FROM " + baseTableName + " " + joins + " " + where;
        }


        private static void GetPartialSQLString(String tableName, ref String columns, ref String tables, ref String joins, ref String where, bool includeWhere)
        {

            tables = tables + "," + tableName;

            DataSet dsTable = GetTableMetaData(tableName);

            if (dsTable.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsTable.Tables[0].Rows)
                {
                    String columnName = dr["ColumnName"].ToString();
                    String columnFQName = tableName + "." + columnName;

                    Boolean isPrimaryKey = Convert.ToBoolean(dr["IsPrimaryKey"].ToString());

                    columns = columns + "," + columnFQName;

                    if (isPrimaryKey && includeWhere)
                    {
                        where = " WHERE " + columnFQName + " = {0}";
                    }

                    if (!dr.IsNull("ReferenceAttrID"))
                    {
                        int refAttrId = Convert.ToInt32(dr["ReferenceAttrID"].ToString());
                        string refTableName = GetTableNameFromAttributeId(refAttrId);

                        DataSet dsColumn = GetColumnDetails(refAttrId);
                        if (dsColumn.Tables[0].Rows.Count > 0)
                        {
                            DataRow drColumn = dsColumn.Tables[0].Rows[0];
                            String refColumnName = drColumn["ColumnName"].ToString();
                            String refColumnFQName = refTableName + "." + refColumnName;

                            joins = joins + "INNER JOIN " + refTableName + " ON " + refColumnFQName + " = " + columnFQName;

                            if (refTableName != "")
                            {
                                GetPartialSQLString(refTableName, ref columns, ref tables, ref joins, ref where, false);
                            }
                        }

                    }

                }
            }
        }

    }
}
