
# Magic Lambda MS SQL Server data adapters

[![Build status](https://travis-ci.org/polterguy/magic.lambda.mssql.svg?master)](https://travis-ci.org/polterguy/magic.lambda.mssql)

These are the MS SQL Server data adapters for [Magic](https://github.com/polterguy/magic). They allow you to provide a semantic
lambda strucutre to its slots, which in turn will dynamically create a MS SQL dialectic SQL statement for you, for all basic
types of SQL statements. In addition, it provides slots to open a MS SQL database connection, and such, to allow you to
declare your own SQL statements, to be executed towards a MS SQL database. An example of usage can be found below in
Hyperlambda format.

```
mssql.read
   generate:bool:true
   table:SomeTable
   columns
      Foo:bar
      Howdy:World
```

The above will result in the following SQL statement.

```sql
select "Foo","Howdy" from "SomeTable"
```

Where of course a large part of the point being that the structure for the above, is the exact same as the structure
for creating a similar MySQL SQL statement, except with a different slot name.

Below is a list of the slots provided by this project.

* __[mssql.connect]__ - Connects to a MS SQL database.
* __[mssql.execute]__ - Executes some SQL towards the currently _"top level"_ open MS SQL database connection as `ExecuteNonQuery`.
* __[mssql.scalar]__ - Executes some SQL towards the currently _"top level"_ open MS SQL database connection as `ExecuteScalar`.
* __[mssql.select]__ - Executes some SQL towards the currently _"top level"_ open MS SQL database connection as `ExecuteRead` and returns a node structure representing its result.

In addition to the above _"low level"_ slots, there are also some slightly more _"high level slots"_, allowing you to think rather in terms 
of generic CRUD arguments, that does not require you to supply SQL, but rather a syntax tree, such as the code example above is an example of.
These slots are listed below.

* __[mssql.create]__ - Create from CRUD
* __[mssql.delete]__ - Delete from CRUD
* __[mssql.read]__ - Read from CRUD
* __[mssql.update]__ - Update from CRUD

The above slots follows the same similar generic type of syntax, and can also easily be interchanged with the MySQL counterparts,
arguably abstracting away the underlaying database provider more or less completely - Assuming you're only interested in CRUD
operations, that are not too complex in nature.

## License

Magic is licensed as Affero GPL. This means that you can only use it to create Open Source solutions.
If this is a problem, you can contact at thomas@gaiasoul.com me to negotiate a proprietary license if
you want to use the framework to build closed source code. This will allow you to use Magic in closed
source projects, in addition to giving you access to Microsoft SQL Server adapters, to _"crudify"_
database tables in MS SQL Server. I also provide professional support for clients that buys a
proprietary enabling license.
