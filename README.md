# Micro-ORM

Basic micro object relationship mapping tool.

This was created as a learning tool to help understand and simplify the use of requesting and modifying a database.

There are much better ORM's out there for example use Dapper, Entity Framework the list goes on.

Supports basic querying of the database and CRUD features.

If you want simple then this is the tool for you.

Examples of use:-

I have a test database 'dev_' with a table called 'person'

// get all data with no parameters
var result = Read<Person>("SELECT * FROM dev_.person"); // id, description, name, age, instruction

// get data with parameters
var p = new MicroParameters();
p.Parameters.Add( new MicroParameter { ParameterName = "@id", ParameterType = System.Data.DbType.Int32, ParameterValue = "5" } );
var result = Read<Person>("SELECT * FROM dev_.person Where id=@id;", p);

// Delete a record
var p2 = new MicroParameters();
p2.Parameters.Add(new MicroParameter { ParameterName = "@id", ParameterType = System.Data.DbType.Int32, ParameterValue = "15" });
var rowsDeleted = Delete("DELETE FROM person WHERE id=@id;", p2);
Debug.WriteLine($"rows deleted = {rowsDeleted}");

// Update a Record
var p3 = new MicroParameters();
p3.Parameters.Add(new MicroParameter { ParameterName = "@id", ParameterType = System.Data.DbType.String, ParameterValue = "1" });
p3.Parameters.Add(new MicroParameter { ParameterName = "@name", ParameterType = System.Data.DbType.String, ParameterValue = "Ian" });
var rowsUpdated = Update("UPDATE dev_.person SET name=@name WHERE id=@id;", p3);
Debug.WriteLine($"rows updated = {rowsUpdated}");

// Insert a new Record
var p4 = new MicroParameters();
p4.Parameters.Add(new MicroParameter { ParameterName = "@name", ParameterType = System.Data.DbType.String, ParameterValue = "Dave" });
p4.Parameters.Add(new MicroParameter { ParameterName = "@description", ParameterType = System.Data.DbType.String, ParameterValue = "Totaly awesome guy" });
p4.Parameters.Add(new MicroParameter { ParameterName = "@age", ParameterType = System.Data.DbType.Int32, ParameterValue = "21" });
p4.Parameters.Add(new MicroParameter { ParameterName = "@instruction", ParameterType = System.Data.DbType.String, ParameterValue = "Point this man at any challenge" });
var rowsInserted = Insert("INSERT INTO dev_.person (name,description,age,instruction) VALUES (@name,@description,@age,@instruction);", p4);
Debug.WriteLine($"rows inserted = {rowsInserted}");

-- Future Release --
Automatic SQL building based from parameters injected.

