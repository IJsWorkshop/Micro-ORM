using System.Data;

namespace Micro_ORM
{
    public class MicroClass
    {
        public class Connection : IConnection
        {
            public string? ConnectionString { get; set; } = string.Empty;
            public int MaxAttempts { get; set; } = 3;
            public int Attempt { get; set; } = 0;
        }

        public interface IConnection
        {
            string? ConnectionString { get; set; }
            int MaxAttempts { get; set; }
            int Attempt { get; set; }
        }

        public class QueryConfig : IQuery
        {
            public string Query { get; set; } = string.Empty;
            public string TableName { get; set; } = string.Empty;
            public IDbDataParameter[] Parameters { get; set; } = { };
        }

        public interface IQuery
        {
            string Query { get; set; }
            string TableName { get; set; }
            IDbDataParameter[] Parameters { get; set; }
        }

        public class ExecuteCommand : IExecuteCommand
        {
            public Action Execute { get; set; }
        }

        public interface IExecuteCommand 
        {
            Action Execute { get; set; }
        }

    }
}
