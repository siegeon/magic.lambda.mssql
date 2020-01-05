//#define RUN_REAL
/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */


/*
 * These unit tests requires the following database table to exist somewhere.
 * And it also requires the "_connection" field of the class to be pointing to
 * that database containing the "Demo" table.
 * 
 * Notice! To run these tests, uncomment the first line in the file, to make
 * sure the compiler includes the C# code while compiling your tests suite.



CREATE TABLE [dbo].[Demo] (
  [Id] INT IDENTITY (1, 1) NOT NULL,
  [text] NCHAR (100) NULL,
  PRIMARY KEY CLUSTERED ([Id] ASC)
);


 */

#if RUN_REAL
using System.Linq;
using Xunit;
using magic.node.extensions;
using magic.node.expressions;

namespace magic.lambda.mssql.tests
{
    public class MsSQLTestsRealDB
    {
        // TODO: Modify this connection string if you intend to run these tests.
        const string _connection = "Server=localhost;Database=foo;Trusted_Connection=True;";

        [Fact]
        public void SelectFromDemo_01()
        {
            var lambda = Common.Evaluate(string.Format(@"
mssql.connect:""{0}""
   mssql.read
      table:Demo", _connection));
        }

        [Fact]
        public void InsertAndSelectFromDemo_01()
        {
            var lambda = Common.Evaluate(string.Format(@"
mssql.connect:""{0}""
   mssql.create
      table:Demo
      values
         text:Howdy world
   mssql.read
      table:Demo
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
mssql.connect:""{0}""
   mssql.create
      table:Demo
      values
         text:Howdy world
   mssql.read
      table:Demo
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
mssql.connect:""{0}""
   mssql.create
      table:Demo
      values
         text:Howdy world
   mssql.update
      table:Demo
      where
         and
            Id:x:@mssql.create
      values
         text:Jo dudes!
   mssql.read
      table:Demo
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
mssql.connect:""{0}""
   mssql.transaction.create
      mssql.create
         table:Demo
         values
            text:Non existing
      mssql.transaction.rollback
   mssql.scalar:""select count(*) from Demo where text = 'Non existing'""", _connection));
            Assert.Equal(0, lambda.Children.First().Children.Skip(1).First().Value);
        }

        [Fact]
        public void InsertTransactionRollbackImplicitly()
        {
            var lambda = Common.Evaluate(string.Format(@"
mssql.connect:""{0}""
   mssql.transaction.create
      mssql.create
         table:Demo
         values
            text:Non existing
mssql.connect:""{0}""
   mssql.scalar:""select count(*) from Demo where text = 'Non existing'""", _connection));
            Assert.Equal(0, lambda.Children.Skip(1).First().Children.First().Value);
        }

        [Fact]
        public void InsertTransactionCommitExplicitly()
        {
            var lambda = Common.Evaluate(string.Format(@"
mssql.connect:""{0}""
   mssql.transaction.create
      mssql.create
         table:Demo
         values
            text:Non existing - yet does
      mssql.transaction.commit
   mssql.scalar:""select count(*) from Demo where text = 'Non existing - yet does'""", _connection));
            Assert.True(lambda.Children.First().Children.Skip(1).First().Get<int>() > 0);
        }
    }
}
#endif
