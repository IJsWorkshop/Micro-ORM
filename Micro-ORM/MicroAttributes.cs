namespace Micro_ORM
{
    public class MicroAttributes
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class ActiveClass : Attribute
        {
            public bool Active { get; set; } = true;
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class MyProperty : Attribute
        {
            public bool Include { get; set; } = true;
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class PrimaryKey : Attribute { }


        [AttributeUsage(AttributeTargets.Property)]
        public class ForeignKey : Attribute
        {
            public string FkTableName { get; set; } = string.Empty;

            public Type? FkTableType { get; set; }
        }
    }
}
