//#define RUN_REAL
/*
 * Magic, Copyright(c) Thomas Hansen 2019, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */


/*
 * These unit tests requires the following database table to exist somewhere.
 * And it also requires the "_connection" field of the class to be pointing to
 * that database containing the "demo" table.
 * 
 * Notice! To run these tests, uncomment the first line in the file, to make
 * sure the compiler includes the C# code while compiling your tests suite.



CREATE TABLE `foo`.`demo` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `text` VARCHAR(45) NULL,
  PRIMARY KEY (`id`));



 */

#if RUN_REAL
using System.Linq;
using Xunit;
using magic.node.extensions;
using magic.node.expressions;

namespace magic.lambda.mysql.tests
{
    public class MsSQLTestsRealDB
    {
        // TODO: Modify this connection string if you intend to run these tests.
        const string _connection = "Server=127.0.0.1;Database=foo;Uid=root;Pwd=ThisIsNotANicePassword;SslMode=Preferred;";

        [Fact]
        public void SelectFromDemo_01()
        {
            var lambda = Common.Evaluate(string.Format(@"
mysql.connect:""{0}""
   mysql.read
      table:demo", _connection));
        }

        [Fact]
        public void InsertAndSelectFromDemo_01()
        {
            var lambda = Common.Evaluate(string.Format(@"
mysql.connect:""{0}""
   mysql.create
      table:demo
      values
         text:Howdy world
   mysql.read
      table:demo
      where
         and
            text:Howdy world
", _connection));
            var result = new Expression("../*/1/*/*/text").Evaluate(lambda);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void InsertAndSelectLimitFromDemo_01()
        {
            var lambda = Common.Evaluate(string.Format(@"
mysql.connect:""{0}""
   mysql.create
      table:demo
      values
         text:Howdy world
   mysql.read
      table:demo
      where
         and
            text:Howdy world
      limit:5
", _connection));
            var result = new Expression("../*/1/*/*/text").Evaluate(lambda);
            Assert.True(result.Count() <= 5);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void InsertAndUpdateExpressionClause_01()
        {
            var lambda = Common.Evaluate(string.Format(@"
mysql.connect:""{0}""
   mysql.create
      table:demo
      values
         text:Howdy world
   mysql.update
      table:demo
      where
         and
            Id:x:@mysql.create
      values
         text:Jo dudes!
   mysql.read
      table:demo
      where
         and
            text:Jo dudes!
      limit:5
", _connection));
            var result = new Expression("../*/1/*/*/text").Evaluate(lambda);
            Assert.True(result.Count() <= 5);
        }

        [Fact]
        public void InsertTransactionRollbackExplicitly()
        {
            var lambda = Common.Evaluate(string.Format(@"
mysql.connect:""{0}""
   mysql.transaction.create
      mysql.create
         table:demo
         values
            text:Non existing
      mysql.transaction.rollback
   mysql.scalar:""select count(*) from demo where text = 'Non existing'""", _connection));
            Assert.Equal(0, lambda.Children.First().Children.Skip(1).First().Get<int>());
        }

        [Fact]
        public void InsertTransactionRollbackImplicitly()
        {
            var lambda = Common.Evaluate(string.Format(@"
mysql.connect:""{0}""
   mysql.transaction.create
      mysql.create
         table:demo
         values
            text:Non existing
mysql.connect:""{0}""
   mysql.scalar:""select count(*) from demo where text = 'Non existing'""", _connection));
            Assert.Equal(0, lambda.Children.Skip(1).First().Children.First().Get<int>());
        }

        [Fact]
        public void InsertTransactionCommitExplicitly()
        {
            var lambda = Common.Evaluate(string.Format(@"
mysql.connect:""{0}""
   mysql.transaction.create
      mysql.create
         table:demo
         values
            text:Non existing - yet does
      mysql.transaction.commit
   mysql.scalar:""select count(*) from demo where text = 'Non existing - yet does'""", _connection));
            Assert.True(lambda.Children.First().Children.Skip(1).First().Get<int>() > 0);
        }
    }
}
#endif
