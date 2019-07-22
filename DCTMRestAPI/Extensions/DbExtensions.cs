using DCTMRestAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DCTMRestAPI.Extensions
{
    public class DbExtensions
    {
        public static List<T> ExecSQL<T>(string query, DCTrackContext context)
        {
            using (context)
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    context.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        List<T> list = new List<T>();
                        T obj = default(T);
                        while (result.Read())
                        {
                            obj = Activator.CreateInstance<T>();
                            foreach (PropertyInfo prop in obj.GetType().GetProperties())
                            {
                                if (!object.Equals(result[prop.Name], DBNull.Value))
                                {
                                    prop.SetValue(obj, result[prop.Name], null);
                                }
                            }
                            list.Add(obj);
                        }
                        return list;

                    }
                }
            }
        }

        public static DataTable ExecDataSet(string query, DCTrackContext context)
        {
            using (context)
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    context.Database.OpenConnection();
                    DataTable data = new DataTable();

                    using (var result = command.ExecuteReader())
                    {
                        if (result.HasRows)
                        {
                            if (result.FieldCount > 0)
                            {
                                for (int i = 0; i < result.FieldCount; i++)
                                {
                                    Type t = result.GetFieldType(i);
                                    string colName;
                                    if (!string.IsNullOrEmpty(result.GetName(i)))
                                        colName = result.GetName(i);
                                    else
                                        colName = "col" + i.ToString();
                                    data.Columns.Add(colName, t);
                                }
                                while (result.Read())
                                {
                                    var newRow = data.Rows.Add();
                                    for(int i=0;i< result.FieldCount; i++)
                                    {
                                        newRow[i] = result[i];
                                    }
                                }
                            }
                        }

                    }
                    return data;

                }
            }
        }

        

    }


}
