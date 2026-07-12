using System;
using System.Data;
using DCTMRestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DCTMRestAPI.Extensions
{
    public static class DbExtensions
    {
        /// <summary>
        /// Executes a parameterized SQL query and materializes the result set into a <see cref="DataTable"/>.
        /// Always pass user-supplied values through <paramref name="parameters"/> (never string concatenation)
        /// to avoid SQL injection. The <paramref name="context"/> is owned by the DI container and must NOT be
        /// disposed here; the connection it opens is closed again before returning.
        /// </summary>
        public static DataTable ExecDataSet(
            string query,
            DCTrackContext context,
            params (string Name, object Value)[] parameters)
        {
            var data = new DataTable();

            using var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;

            foreach (var (name, value) in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = name;
                parameter.Value = value ?? DBNull.Value;
                command.Parameters.Add(parameter);
            }

            context.Database.OpenConnection();
            try
            {
                using var result = command.ExecuteReader();
                if (result.HasRows && result.FieldCount > 0)
                {
                    for (int i = 0; i < result.FieldCount; i++)
                    {
                        var colName = string.IsNullOrEmpty(result.GetName(i)) ? $"col{i}" : result.GetName(i);
                        data.Columns.Add(colName, result.GetFieldType(i));
                    }

                    while (result.Read())
                    {
                        var newRow = data.Rows.Add();
                        for (int i = 0; i < result.FieldCount; i++)
                            newRow[i] = result[i];
                    }
                }
            }
            finally
            {
                context.Database.CloseConnection();
            }

            return data;
        }
    }
}
