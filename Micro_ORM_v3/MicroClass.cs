using static Micro_ORM_v3.MicroAttributes;
using System.Collections.Generic;
using System.Data;

namespace Micro_ORM_v3
{
    public class MicroClass
    {
        public class Connection : IConnection
        {
            public string ConnectionString { get; set; } = string.Empty;
            public int MaxAttempts { get; set; } = 3;
            public int Attempt { get; set; } = 0;
        }

        public interface IConnection
        {
            string ConnectionString { get; set; }
            int MaxAttempts { get; set; }
            int Attempt { get; set; }
        }

        public class MicroParameters : IMicroParameters
        {
            public IList<IMicroParameter> Parameters { get; set; } = new List<IMicroParameter>();
        }

        public interface IMicroParameters
        {
            IList<IMicroParameter> Parameters { get; set; }
        }

        public class MicroParameter : IMicroParameter
        {
            public string ParameterName { get; set; }
            public DbType ParameterType { get; set; }
            public string ParameterValue { get; set; }
        }

        public interface IMicroParameter
        {
            string ParameterName { get; set; }
            DbType ParameterType { get; set; }
            string ParameterValue { get; set; }
        }

        [ActiveClass]
        public class Person
        {
            [PrimaryKey]
            public int id { get; set; } = default;
            public string description { get; set; } = default;
            public string name { get; set; } = default;
            public int age { get; set; } = default;
            public string instruction { get; set; } = default;
        }

    }
}
