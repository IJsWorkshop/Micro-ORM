using static Micro_ORM.MicroClass;

namespace Micro_ORM
{
    public class Micro<T> : Crud<T> where T : class
    {
        public Micro() { }

        public override List<T> Pull<T>(IQuery queryConfig) => oOo.Open().Pull<T>(queryConfig);

        public override int Post(IQuery queryConfig) => oOo.Open().Post(queryConfig);

    }

    public abstract class Crud<T>
    {
        // Read
        public abstract List<T> Pull<T>(IQuery queryConfig);

        // Update, Insert, Delete
        public abstract int Post(IQuery queryConfig);
    }
}