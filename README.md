# Micro-ORM

<summary>Basic micro object relationship mapping tool.</summary>

<summary>This was created as a learning tool to help understand and simplify the use of requesting and modifying a database.</summary>

<summary>There are much better ORM's out there for example use Dapper, Entity Framework the list goes on.</summary>

<summary>Supports basic querying of the database and CRUD features.</summary>

| Feature |              Description              |
| ------- | ------------------------------------- |
|  Read   | Request information from a table      |
| Update  | Update a record                       |
| Insert  | Insert a record                       |
| Delete  | Delete a record                       |

<summary>If you want simple then this is the tool for you.</summary>

<h2>Examples of use:-</h2>

I have a test database 'dev_' with a table called 'person'

Get all data with no parameters
```C#
var result = Read<Person>("SELECT * FROM dev_.person");
```
Get data with parameters
```C#
var p = new MicroParameters();
p.Parameters.Add( new MicroParameter { ParameterName = "@id", ParameterType = System.Data.DbType.Int32, ParameterValue = "5" } );
var result = Read<Person>("SELECT * FROM dev_.person Where id=@id;", p);
```
Delete a record
```C#
1)
var p2 = new MicroParameters();
p2.Parameters.Add(new MicroParameter { ParameterName = "@id", ParameterType = System.Data.DbType.Int32, ParameterValue = "15" });
var rowsDeleted = Delete("DELETE FROM person WHERE id=@id;", p2);
Debug.WriteLine($"rows deleted = {rowsDeleted}");
2)
var rowsDeleted = Delete("DELETE FROM person WHERE id=15;");

```
Update a Record
```C#
var p3 = new MicroParameters();
p3.Parameters.Add(new MicroParameter { ParameterName = "@id", ParameterType = System.Data.DbType.String, ParameterValue = "1" });
p3.Parameters.Add(new MicroParameter { ParameterName = "@name", ParameterType = System.Data.DbType.String, ParameterValue = "Brian" });
var rowsUpdated = Update("UPDATE dev_.person SET name=@name WHERE id=@id;", p3);
Debug.WriteLine($"rows updated = {rowsUpdated}");
```
Insert a new Record
```C#
var p4 = new MicroParameters();
p4.Parameters.Add(new MicroParameter { ParameterName = "@name", ParameterType = System.Data.DbType.String, ParameterValue = "Dave" });
p4.Parameters.Add(new MicroParameter { ParameterName = "@description", ParameterType = System.Data.DbType.String, ParameterValue = "Totaly awesome guy" });
p4.Parameters.Add(new MicroParameter { ParameterName = "@age", ParameterType = System.Data.DbType.Int32, ParameterValue = "21" });
p4.Parameters.Add(new MicroParameter { ParameterName = "@instruction", ParameterType = System.Data.DbType.String, ParameterValue = "Point this man at any challenge" });
var rowsInserted = Insert("INSERT INTO dev_.person (name,description,age,instruction) VALUES (@name,@description,@age,@instruction);", p4);
Debug.WriteLine($"rows inserted = {rowsInserted}");

```
<p>You must add your database connection details for this to work - you will find the connection string located in the Micro_ORM.cs file</p>

```C#
ConnectionString = "Server=127.0.0.1;Port=3306;Database=dev_;Uid=root;Pwd=password;"
```

<p>You can set this up for any type of database connector, it is presently set for MySqlConnection</p>


<h2>-- Future Release --</h2>
Automatic SQL building based from parameters injected.
