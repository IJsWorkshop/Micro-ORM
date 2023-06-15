using System.Text.RegularExpressions;
using static Micro_ORM.MicroAttributes;
using static Micro_ORM.MicroClass;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Data;

namespace Micro_ORM
{
    public static class oOo
    {
        // Hack to get column names from query string
        public static List<string> GetMapOrder(this string mappingSource)
        => new Regex(@"^(SELECT )([a-zA-Z0-9,. ]+)\s\b(FROM)", RegexOptions.IgnoreCase)
           .Match(mappingSource)
           .Groups[2]
           .Value
           .Replace(" ", "")
           .Split(",")
           .ToList();

        public static List<Type> GetActiveClassFromAssembly()
        {
            List<Type> activeClassList = new List<Type>();

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            // Get assembly types we are after
            foreach (Type t in types)
            {
                var attrs = Attribute.GetCustomAttributes(t);
                foreach (var attr in attrs)
                {
                    if (attr is ActiveClass aC)
                    {
                        if (aC.Active)
                        {
                            // meets qualifing requirement
                            activeClassList.Add(t);
                            Debug.WriteLine(t.ToString());
                        }
                    }
                }
            }
            return activeClassList;
        }

        public static Dictionary<string, PropertyInfo> GetClassProperties<T>()=>typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.IsDefined(typeof(T), true)).ToDictionary(k => k.Name, k => k);

        public static IDbConnection Open()
        {
            // create connection 
            IDbConnection OpenedConnection = new MySqlConnection();

            // get connection details
            IConnection connectionConfig = new Connection();

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
                        OpenedConnection = new MySqlConnection(connectionConfig.ConnectionString);
                        OpenedConnection.Open();
                        break;
                    }

                }
                catch (Exception connectionError)
                {
                    Console.WriteLine($"Connection problem - attempting to connect ... (attempt:{connectionConfig.Attempt}) : {connectionError}");
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

        public static List<T> Pull<T>(this IDbConnection dbConn, IQuery query)
        {
            var collection = new List<T>();

            var command = dbConn.CreateCommand();

            command.CommandText = query.Query;

            using (var dr = command.ExecuteReader())
            {
                // only here for debugging
                object? propTmp = null;

                try
                {
                    // get map order properties from class of T
                    var MapOrder = GetClassProperties<T>();

                    // create instance of T
                    T o = Activator.CreateInstance<T>();

                    // loop to end of reader
                    while (dr.Read())
                    {
                        // leave here untill tested -> var instanceDictionary = o.GetType().GetProperties().ToDictionary(k => k.Name, k => k);
                        foreach (var prop in MapOrder)
                        {
                            propTmp = prop;

                            // if datareader value not null add to instance of T
                            if (!object.Equals(dr[prop.Key], DBNull.Value))
                            {
                                // substitute instance property with datareader value
                                MapOrder[prop.Key].SetValue(o, dr[prop.Key], null);
                            }
                        }

                        // add instance to List
                        collection.Add(o);
                    }
                }
                catch (Exception mapError)
                {
                    Console.WriteLine($"Mapped property {query.TableName} - {propTmp} - {typeof(T).ToString()}");
                    Console.WriteLine($"Mapping error : {mapError}");
                }

                // close datareader
                dr.Close();

                // auto close connection
                command.AutoCloseConnection();
            }

            return collection;
        }

        public static int Post(this IDbConnection dbConn, IQuery query) 
        {
            int o = 0;

            var command = dbConn.CreateCommand();

            command.CommandText = query.Query;

            if (query.Parameters.Length > 0)
            {
                command.Parameters.Clear();

                foreach (var item in query.Parameters) command.Parameters.Add(item);
            }

            var result = command.ExecuteScalar();

            o = result != DBNull.Value ? Convert.ToInt32(result) : -1;

            command.AutoCloseConnection();

            return o; 
        }

        public static void AutoCloseConnection(this IDbCommand command)
        {
            if (command.Connection != null && command.Connection.State != ConnectionState.Closed)
            {
                command.Connection.Close();
                command.Connection = null;
            }
        }

    }
}
