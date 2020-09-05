
# Magic Lambda MySQL data adapters

[![Build status](https://travis-ci.org/polterguy/magic.lambda.mysql.svg?master)](https://travis-ci.org/polterguy/magic.lambda.mysql)

This is the MySQL data adapter for [Magic](https://github.com/polterguy/magic). This project allows you to provide a semantic
lambda structure to its slots, which in turn will dynamically create a MySQL dialect SQL statement for you, for all basic
types of CRUD SQL statements. In addition, it provides slots to open a MySQL database connection, and such, to allow you to
declare your own SQL statements, to be executed towards a MySQL database. Slots this project contains are as follows.

* __[mysql.connect]__ - Connects to a database, either taking an entire connection string, or a reference to a configuration connection string.
* __[mysql.create]__ - Creates a single record in the specified table.
* __[mysql.read]__ - Reads multiple records from the specified table.
* __[mysql.update]__ - Updates a single record in the specified table.
* __[mysql.delete]__ - Deletes a single record in the specified table.
* __[mysql.select]__ - Executes an arbitrary SQL statement, and returns results of reader as lambda object to caller.
* __[mysql.scalar]__ - Executes an arbitrary SQL statement, and returns the result as a scalar value to caller.
* __[mysql.execute]__ - Executes an aribitrary SQL statement.
* __[mysql.transaction.create]__ - Creates a new transaction, that will be explicitly rolled back as execution leaves scope, unless __[mysql.transaction.commit]__ is explicitly called before leaving scope.
* __[mysql.transaction.commit]__ - Explicitly commits an open transaction.
* __[mysql.transaction.rollback]__ - Explicitly rolls back an open transaction.

Most of the above slots also have async (wait.) overloads.

**Notice** - If you use any of the CRUD slots from above, the whole idea is that you can polymorphistically use the
same lambda object, towards any of the underlaying database types, and the correct specific syntax for your particular
database vendor's SQL syntax will be automatically generated.

This allows you to transparently use the same lambda object, towards any of the supported database types, without
having to change it in any ways.

## [mysql.create], [mysql.read], [mysql.update] and [mysql.delete]

All of these slots have the _exact same syntax_ for all supported data adapters, which you can read about in the
link below. Just make sure you start out your CRUD slot invocations with `mysql.` instead of `sql.` to use
them towards a MySQL database. You will also need to open a database connection before you invoke these slots,
unless you're only interested in generating its specific SQL command text, and not actually execute the SQL.

* [Magic Data Common](https://github.com/polterguy/magic.data.common)

## [mysql.connect]

This slot will open a database connection for you. You can pass in a complete connection string (not recommended),
or only the database name if you wish. If you pass in only the database name, the generic connection string for MySQL
from your _"appsettings.json"_ file will be used, substituting its `{database}` parts with the database of your choice.

Inside of this, which actually is a lambda **[eval]** invocation, you can use any of the other slots, requiring
an existing and open database connection to function. You can see an example below.

```
mysql.connect:sakila
   mysql.read
      table:actor
```

## [mysql.select]

This slot allows you to pass in any arbitrary SQL you wish, and it will evaluate to a `DataReader`, and return
all records as a lambda object. You can find an example below.

```
mysql.connect:sakila
   mysql.select:select * from actor limit 10
```

## License

Although most of Magic's source code is Open Source, you will need a license key to use it.
[You can obtain a license key here](https://servergardens.com/buy/).
Notice, 7 days after you put Magic into production, it will stop working, unless you have a valid
license for it.

* [Get licensed](https://servergardens.com/buy/)
