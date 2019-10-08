#define RUN_REAL
/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using Xunit;
using magic.node.extensions;
using magic.node.expressions;

namespace magic.lambda.mssql.tests
{
    public class MsSQLTestsRealDB
    {
#if RUN_REAL

        [Fact]
        public void SelectFromDemo_01()
        {
            var lambda = Common.Evaluate(@"
mssql.connect:""Server=localhost;Database=foo;Trusted_Connection=True;""
   mssql.read
      table:Demo");
        }

        [Fact]
        public void InsertAndSelectFromDemo_01()
        {
            var lambda = Common.Evaluate(@"
mssql.connect:""Server=localhost;Database=foo;Trusted_Connection=True;""
   mssql.create
      table:Demo
      values
         text:Howdy world
   mssql.read
      table:Demo
      where
         and
            text:Howdy world
");
            var result = new Expression("../*/1/*/*/text").Evaluate(lambda);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void InsertAndSelectLimitFromDemo_01()
        {
            var lambda = Common.Evaluate(@"
mssql.connect:""Server=localhost;Database=foo;Trusted_Connection=True;""
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
");
            var result = new Expression("../*/1/*/*/text").Evaluate(lambda);
            Assert.True(result.Count() <= 5);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void InsertAndUpdateExpressionClause_01()
        {
            var lambda = Common.Evaluate(@"
mssql.connect:""Server=localhost;Database=foo;Trusted_Connection=True;""
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
");
            var result = new Expression("../*/1/*/*/text").Evaluate(lambda);
            Assert.True(result.Count() <= 5);
        }

        [Fact]
        public void InsertTransactionRollbackExplicitly()
        {
            var lambda = Common.Evaluate(@"
mssql.connect:""Server=localhost;Database=foo;Trusted_Connection=True;""
   mssql.transaction.create
      mssql.create
         table:Demo
         values
            text:Non existing
      mssql.transaction.rollback
   mssql.scalar:""select count(*) from Demo where text = 'Non existing'""");
            Assert.Equal(0, lambda.Children.First().Children.Skip(1).First().Value);
        }

        [Fact]
        public void InsertTransactionRollbackImplicitly()
        {
            var lambda = Common.Evaluate(@"
mssql.connect:""Server=localhost;Database=foo;Trusted_Connection=True;""
   mssql.transaction.create
      mssql.create
         table:Demo
         values
            text:Non existing
mssql.connect:""Server=localhost;Database=foo;Trusted_Connection=True;""
   mssql.scalar:""select count(*) from Demo where text = 'Non existing'""");
            Assert.Equal(0, lambda.Children.Skip(1).First().Children.First().Value);
        }

        [Fact]
        public void InsertTransactionCommitExplicitly()
        {
            var lambda = Common.Evaluate(@"
mssql.connect:""Server=localhost;Database=foo;Trusted_Connection=True;""
   mssql.transaction.create
      mssql.create
         table:Demo
         values
            text:Non existing - yet does
      mssql.transaction.commit
   mssql.scalar:""select count(*) from Demo where text = 'Non existing - yet does'""");
            Assert.True(lambda.Children.First().Children.Skip(1).First().Get<int>() > 0);
        }
#endif
    }
}
