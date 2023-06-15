using static Micro_ORM_v3.MicroClass;
using System.Collections.Generic;

namespace Micro_ORM_v3
{
    public class Micro : Crud
    {
        public Micro() { }

        public override List<T> Read<T>(string query, IList<IMicroParameter> parameters) => Read<T>(query, parameters);

        public override int Update(string query, IList<IMicroParameter> parameters) => Update(query, parameters);

        public override int Delete(string query, IList<IMicroParameter> parameters) => Delete(query, parameters);

        public override int Insert(string query, IList<IMicroParameter> parameters) => Insert(query, parameters);

    }


    // Basic CRUD operations
    public abstract class Crud
    {
        // Read
        public abstract List<T> Read<T>(string query, IList<IMicroParameter> parameters);

        // update
        public abstract int Update(string query, IList<IMicroParameter> parameters);

        // insert
        public abstract int Delete(string query, IList<IMicroParameter> parameters);

        // Delete
        public abstract int Insert(string query, IList<IMicroParameter> parameters);

    }
}
