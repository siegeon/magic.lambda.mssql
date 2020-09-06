
# Magic Lambda MS SQL Server data adapters

[![Build status](https://travis-ci.org/polterguy/magic.lambda.mssql.svg?master)](https://travis-ci.org/polterguy/magic.lambda.mssql)

These are the MS SQL Server data adapters for [Magic](https://github.com/polterguy/magic). They allow you to provide a semantic
lambda strucutre to its slots, which in turn will dynamically create a MS SQL dialectic SQL statement for you, for all basic
types of SQL statements. In addition, it provides slots to open a MS SQL database connection, and such, to allow you to
declare your own SQL statements, to be executed towards a MS SQL database. Slots this project encapsulates are as follows.

* __[mssql.connect]__ - Connects to a database, either taking an entire connection string, or a reference to a configuration connection string.
* __[mssql.create]__ - Creates a single redorc in the specified table.
* __[mssql.delete]__ - Deletes a single record in the specified table.
* __[mssql.read]__ - Reads multiple records from the specified table.
* __[mssql.update]__ - Updates a single record in the specified table.
* __[mssql.select]__ - Executes an arbitrary SQL statement, and returns results of reader as lambda object to caller.
* __[mssql.scalar]__ - Executes an arbitrary SQL statement, and returns the result as a scalar value to caller.
* __[mssql.execute]__ - Executes an aribitrary SQL statement.
* __[mssql.execute-batch]__ - Executes a _"batch"_ of SQL statements, where each statement is separated by the word _"GO"_.
* __[mssql.transaction.create]__ - Creates a new transaction, that will be explicitly rolled back as execution leaves scope, unless __[mssql.transaction.commit]__ is explicitly called before leaving scope.
* __[mssql.transaction.commit]__ - Explicitly commits an open transaction.
* __[mssql.transaction.rollback]__ - Explicitly rolls back an open transaction.

Most of the above slots also have async (wait.) overloads.

**Notice** - If you use any of the CRUD slots from above, the whole idea is that you can polymorphistically use the
same lambda object, towards any of the underlaying database types, and the correct specific syntax for your particular
database vendor's SQL syntax will be automatically generated.

This allows you to transparently use the same lambda object, towards any of the supported database types, without
having to change it in any ways.

## [mssql.create], [mssql.read], [mssql.update] and [mssql.delete]

All of these slots have the _exact same syntax_ for all supported data adapters, which you can read about in the
link below. Just make sure you start out your CRUD slot invocations with `mssql.` instead of `sql.` to use
them towards a Microsoft SQL Server database. You will also need to open a database connection before you invoke these slots,
unless you're only interested in generating its specific SQL command text, and not actually execute the SQL.

* [Magic Data Common](https://github.com/polterguy/magic.lambda.mysql)

## [mssql.connect]

This slot will open a database connection for you. You can pass in a complete connection string (not recommended),
or only the database name if you wish. If you pass in only the database name, the generic connection string for Microsoft
SQL Server from your _"appsettings.json"_ file will be used, substituting its `{database}` parts with the database of
your choice.

Inside of this, which actually is a lambda **[eval]** invocation, you can use any of the other slots, requiring
an existing and open database connection to function. You can see an example below.

```
mssql.connect:northwind
   mssql.read
      table:customers
```

## [mssql.select]

This slot allows you to pass in any arbitrary SQL you wish, and it will evaluate to a `DataReader`, and return
all records as a lambda object. You can find an example below.

```
mysql.connect:northwind
   mysql.select:select top 100 * from customers
```

Notice, this slot requires Microsoft SQL server syntax type of SQL, and will not in any ways transpile towards
your specific underlaying database type. If you can, you should rather use **[mssql.read]** for instance, to
avoid tying yourself down to a specific database vendor's SQL dialect.

## License

Although most of Magic's source code is Open Source, you will need a license key to use it.
[You can obtain a license key here](https://servergardens.com/buy/).
Notice, 7 days after you put Magic into production, it will stop working, unless you have a valid
license for it.

* [Get licensed](https://servergardens.com/buy/)
