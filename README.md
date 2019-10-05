
# Magic Lambda MySQL data adapters

[![Build status](https://travis-ci.org/polterguy/magic.lambda.mysql.svg?master)](https://travis-ci.org/polterguy/magic.lambda.mysql)

These are the MySQL data adapters for [Magic](https://github.com/polterguy/magic). They allow you to provide a semantic lambda strucutre
to its slots, which in turn will dynamically create a MySQL dialectic SQL statement for you, for all basic types of SQL statements.
In addition, it provides slots to open a MySQL database connection, and such, to allow you to declare your own SQL statements, to
be executed towards a MySQL database. An example of usage can be found below in Hyperlambda format.

```
mysql.read
   generate:bool:true
   table:SomeTable
   columns
      Foo:bar
      Howdy:World
   limit:10
   offset:100
```

The above will result in the following SQL statement.

```sql
select `Foo`,`Howdy` from `SomeTable` limit 10 offset 100
```

Where of course a large part of the point being that the structure for the above, is the exact same as the structure
for creating a similar MS SQL Server SQL statement, except with a different slot name.

Below you can see the slots provided by this project.

* __[mysql.connect]__ - Connects to a MySQL database.
* __[mysql.execute]__ - Executes some SQL towards the currently _"top level"_ open MySQL database connection as `ExecuteNonQuery`.
* __[mysql.scalar]__ - Executes some SQL towards the currently _"top level"_ open MySQL database connection as `ExecuteScalar`.
* __[mysql.select]__ - Executes some SQL towards the currently _"top level"_ open MySQL database connection as `ExecuteRead` and returns a node structure representing its result.

In addition to the above _"low level"_ slots, there are also some slightly more _"high level slots"_, allowing you to think rather in terms 
of generic CRUD arguments, that does not require you to supply SQL, but rather a syntax tree, such as the code example above is an example of.
These slots are listed below.

* __[mysql.create]__ - Create from CRUD
* __[mysql.delete]__ - Delete from CRUD
* __[mysql.read]__ - Read from CRUD
* __[mysql.update]__ - Update from CRUD

The above slots follows the same similar generic type of syntax, and can also easily be interchanged with the SQL Server counterparts,
arguably abstracting away the underlaying database provider more or less completely - Assuming you're only interested in CRUD
operations, that are not too complex in nature.

### Transaction slots

In addition, this project also gives you 3 database transaction slots, that you can see below.

* __[mysql.transaction.create]__ - Creates a new database transaction. Notice, unless explicitly committed, the transaction will be rolled back as your lambda goes out of scope.
* __[mysql.transaction.commit]__ - Commits the top level transaction.
* __[mysql.transaction.rollback]__ - Rolls back the top level transaction.

## License

Magic is licensed as Affero GPL. This means that you can only use it to create Open Source solutions.
If this is a problem, you can contact at thomas@gaiasoul.com me to negotiate a proprietary license if
you want to use the framework to build closed source code. This will allow you to use Magic in closed
source projects, in addition to giving you access to Microsoft SQL Server adapters, to _"crudify"_
database tables in MS SQL Server. I also provide professional support for clients that buys a
proprietary enabling license.
