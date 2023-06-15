using static Micro_ORM_v3.MicroClass;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Data;
using System.Linq;
using System;

namespace Micro_ORM_v3
{
    public static class Micro_Orm
    {
        
        public static List<T> Read<T>(string queryString, IMicroParameters parameters = null)
        {
            var collection = new List<T>();

            using (var dbConn = Open())
            using (var command = dbConn.CreateCommand())
            {
                command.CommandText = queryString;

                // any parameters if not null
                if (parameters != null) CreateParameters(command, parameters);

                using (var dr = command.ExecuteReader())
                {
                    try
                    {
                        // get properties to map - cached here do not put in loop
                        var MapTo = GetClassProperties<T>();

                        // loop datareader and map
                        while (dr.Read())
                        {
                            collection.Add(dr.MapDataReaderToInstance<T>(MapTo));
                        }
                    }
                    catch (Exception mapError)
                    {
                        var t = typeof(T).ToString();
                        Console.WriteLine($"Mapping err in: {t} check property exists in class or database \r\n error: {mapError}");
                    }

                    // close datareader
                    dr.Close();
                    // auto close connection
                    command.AutoCloseConnection();
                }
            }
            return collection;
        }

        public static int Insert(string query, IMicroParameters parameters = null) => Execute(query, parameters);

        public static int Update(string query, IMicroParameters parameters = null) => Execute(query, parameters);

        public static int Delete(string query, IMicroParameters parameters = null) => Execute(query, parameters);

        static IDbConnection Open()
        {
            // create connection 
            IDbConnection OpenedConnection = null;

            // get connection details
            IConnection connectionConfig = new Connection
            {
                ConnectionString = "Server=127.0.0.1;Port=3306;Database=dev_;Uid=root;Pwd=password;"
            };

            while (connectionConfig.Attempt <= connectionConfig.MaxAttempts)
            {
                try
                {
                    if (OpenedConnection == null)
                    {
                        OpenedConnection = new MySqlConnection(connectionConfig.ConnectionString);
                        OpenedConnection.Open();
                        connectionConfig.Attempt = 10;
                        break;
                    }

                    if (OpenedConnection.State != ConnectionState.Open)
                    {
                        OpenedConnection.Close();
                        OpenedConnection = null;
                        OpenedConnection = new MySqlConnection(connectionConfig.ConnectionString);
                        OpenedConnection.Open();
                        break;
                    }
                }
                catch (Exception connectionError)
                {
                    Debug.WriteLine($"Connection problem - attempting to connect ... (attempt:{connectionConfig.Attempt}) : {connectionError.Message}");
                }
                finally
                {
                    connectionConfig.Attempt++;
                }
            }

            if (OpenedConnection?.State != ConnectionState.Open)
            {
                Console.WriteLine($"Error connecting to the server");
            }

            return OpenedConnection;
        }

        static int Execute(string query, IMicroParameters parameters = null)
        {
            int o = -1;

            using (var dbConn = Open())
            using (var command = dbConn.CreateCommand())
            {
                command.CommandText = query;

                // adds parameters 
                if (parameters != null)
                if (parameters.Parameters.Count > 0)
                {
                    CreateParameters(command, parameters);
                }

                o = command.ExecuteNonQuery();

                command.AutoCloseConnection();
            }
            return o;
        }

        static void CreateParameters(IDbCommand command, IMicroParameters micro)
        {
            if (micro.Parameters.Count > 0)
            {
                foreach (var para in micro.Parameters)
                {
                    IDbDataParameter p = command.CreateParameter();
                    p.ParameterName = para.ParameterName;
                    p.Value = para.ParameterValue;
                    p.DbType = para.ParameterType;
                    command.Parameters.Add(p);
                }
            }
        }

        static void AutoCloseConnection(this IDbCommand command)
        {
            if (command.Connection != null && command.Connection.State != ConnectionState.Closed)
            {
                command.Connection.Close();
                command.Connection = null;
            }
        }

        static List<PropertyInfo> GetClassProperties<T>() => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();

        static T MapDataReaderToInstance<T>(this IDataReader dr, IList<PropertyInfo> pi)
        {
            // create instance of T
            T o = Activator.CreateInstance<T>();

            // loop fields/columns in datareader and map to instance
            for (int ndx = 0; ndx < dr.FieldCount; ndx++)
            {
                for (int i = 0; i < pi.Count; i++)
                {
                    if (dr.GetName(ndx).Equals(pi[i].Name))
                    {
                        pi[i].SetValue(o, dr[ndx], null);
                    }
                }
            }
            return o;
        }

    }
}
