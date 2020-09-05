
# Magic Lambda MySQL data adapters

[![Build status](https://travis-ci.org/polterguy/magic.lambda.mysql.svg?master)](https://travis-ci.org/polterguy/magic.lambda.mysql)

These are the MySQL data adapters for [Magic](https://github.com/polterguy/magic). This adapter allow you to provide a semantic
lambda structure to its slots, which in turn will dynamically create a MySQL dialect SQL statement for you, for all basic
types of SQL statements. In addition, it provides slots to open a MySQL database connection, and such, to allow you to
declare your own SQL statements, to be executed towards a MySQL database. Slots this project encapsulates are as follows.

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

## [mysql.read]

This event allows you to read records in CRUD style, implying you don't need to create your own SQL, but the slot will automatically
create your SQL for you, avoding things such as SQL insertion attacks automatically for you, etc.

```
/*
 * Connecting to sakila database
 */
mysql.connect:[sakila]

   /*
    * Selecting only phone from address table
    */
   mysql.read
      table:address
      columns
         phone
```

Notice, if you want to inspect the SQL that is generated, you can pass in **[generate]** and set its value to boolean true.

## Conditional select, update, delete

The __[mysql.delete]__, __[mysql.read]__ and __[mysql.update]__ slots can be given relatively complex where conditions, where you apply
conditions to these as a __[where]__ node. This will become a part of the SQL _"where"_ clause, where each condition by default will
be _"AND'ed"_ together, but this too can be changed by adding YALOA that declares your logical operator. For instance, to select
all records that have a `value` of _5_ and an `content` of _"foo"_ you can do something like the following

```
mysql.read
   table:SomeTable
   where
      value:int:5
      content:foo
```

The above invocation will return all records that have _both_ a _"value"_ of _"5"_, and a _"content"_ of _"foo"_. You can optionally apply
a logical operator to it, to change it to becoming an _"OR"_ SQL where clause, by adding an _"OR"_ node in between the __[where]__ and
the values, such as the following illustrates.

```
mysql.read
   table:SomeTable
   where
      or
         value:int:5
         content:foo
```

The above will return all records that have _either_ a value of _"5"_ OR a _"content"_ of _"foo"_. To understand how the above logic works,
it might be useful to play around with the _"Evaluator"_ in the Magic frontend, and make sure you add __[generate]__ and set its value
to boolean _"true"_, which will return the resulting SQL, instead of actually evaluating the SQL.

Both select, delete and read slots have the same logic when it comes to creating _"where"_ clauses and attaching these to your resulting SQL.

## More complex operators

In addition to the above, you can also supply any operators, in two ways in fact, which becomes your comparison operators. Below is its most
simple example.

```
mysql.read
   table:SomeTable
   where
      and
         !=
            value:int:5
            content:foo
```

The above will return all records which does _not_ have a _"value"_ of 5, nor a _"content"_ of _"foo"_. Supported operators are as follows.

* !=
* <
* \>
* \>=
* <=
* =
* like

In addition to the above operators, you can supply an _"in"_ operator, which is structurally different, though similar in logic.
Below is an example.

```
mysql.read
   table:SomeTable
   where
      or
         in
            value
               :long:5
               :long:7
```

The above will return all records where its _"value_" is either equal to 5 or 7. You can create as many _"in"_ values as you wish, but corrrently
only integer (long, int types of columns) are supported.

All operators are also supported as _"column-name.operator-name"_ type of arguments, such as the following illustrates.

```
mysql.read
   table:SomeTable
   where
      and
         foo1.like:query%
         foo2.mt:int:5
         foo3.lt:int:5
         foo4.mteq:int:5
         foo5.lteq:int:5
         foo6.neq:int:5
         foo7.eq:int:5
```

_"eq"_ implies _"equals"_, _"lt"_ implies _"less than"_, _"mt"_ implies _"more than"_, _"neq"_ implies _"not equal to"_, etc. All combinations
of previous said words, becomes the equivalent combinatory operator.

## License

Although most of Magic's source code is Open Source, you will need a license key to use it.
[You can obtain a license key here](https://servergardens.com/buy/).
Notice, 7 days after you put Magic into production, it will stop working, unless you have a valid
license for it.

* [Get licensed](https://servergardens.com/buy/)
