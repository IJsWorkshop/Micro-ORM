using static Micro_ORM_v3.MicroClass;
using static Micro_ORM_v3.Micro_Orm;
using System.Diagnostics;
using System;

namespace TestApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            // get data with no parameters
            var result = Read<Person>("SELECT * FROM dev_.person"); // id, description, name, age, instruction

            // get data with parameters
            var p = new MicroParameters();
            p.Parameters.Add( new MicroParameter { ParameterName = "@id", ParameterType = System.Data.DbType.Int32, ParameterValue = "5" } );
            var res = Read<Person>("SELECT * FROM dev_.person Where id=@id;", p);

            // show data 
            foreach (var r in result)
            {
                Debug.WriteLine($"{r.id}, {r.description}, {r.name}, {r.age}, {r.instruction}");
            }

            Debug.WriteLine(" ");

            // Delete
            var p2 = new MicroParameters();
            p2.Parameters.Add(new MicroParameter { ParameterName = "@id", ParameterType = System.Data.DbType.Int32, ParameterValue = "15" });
            var rowsDeleted = Delete("DELETE FROM person WHERE id=@id;", p2);
            Debug.WriteLine($"rows deleted = {rowsDeleted}");

            // Update
            var p3 = new MicroParameters();
            p3.Parameters.Add(new MicroParameter { ParameterName = "@id", ParameterType = System.Data.DbType.String, ParameterValue = "1" });
            p3.Parameters.Add(new MicroParameter { ParameterName = "@name", ParameterType = System.Data.DbType.String, ParameterValue = "Ian" });
            var rowsUpdated = Update("UPDATE dev_.person SET name=@name WHERE id=@id;", p3);
            Debug.WriteLine($"rows updated = {rowsUpdated}");

            // Insert
            var p4 = new MicroParameters();
            p4.Parameters.Add(new MicroParameter { ParameterName = "@name", ParameterType = System.Data.DbType.String, ParameterValue = "Ian" });
            p4.Parameters.Add(new MicroParameter { ParameterName = "@description", ParameterType = System.Data.DbType.String, ParameterValue = "sfdsfsdfsdfsdfsdf" });
            p4.Parameters.Add(new MicroParameter { ParameterName = "@age", ParameterType = System.Data.DbType.Int32, ParameterValue = "50" });
            p4.Parameters.Add(new MicroParameter { ParameterName = "@instruction", ParameterType = System.Data.DbType.String, ParameterValue = "instruction blah" });
            var rowsInserted = Insert("INSERT INTO dev_.person (name,description,age,instruction) VALUES (@name,@description,@age,@instruction);", p4);
            Debug.WriteLine($"rows inserted = {rowsInserted}");



            Console.ReadLine();
        }
    }
}
