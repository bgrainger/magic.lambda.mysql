
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

## Transaction slots

In addition, this project also gives you 3 database transaction slots, that you can see below.

* __[mysql.transaction.create]__ - Creates a new database transaction. Notice, unless explicitly committed, the transaction will be rolled back as your lambda goes out of scope.
* __[mysql.transaction.commit]__ - Commits the top level transaction.
* __[mysql.transaction.rollback]__ - Rolls back the top level transaction.

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
* >
* >=
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

Although most of Magic's source code is publicly available, Magic is _not_ Open Source or Free Software.
You have to obtain a valid license key to install it in production, and I normally charge a fee for such a
key. You can [obtain a license key here](https://gaiasoul.com/license-magic/).
Notice, 5 hours after you put Magic into production, it will stop functioning, unless you have a valid
license for it.

* [Get licensed](https://gaiasoul.com/license-magic/)
