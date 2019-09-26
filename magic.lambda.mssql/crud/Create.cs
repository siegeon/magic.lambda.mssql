/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using magic.node;
using magic.signals.contracts;
using magic.lambda.mssql.utilities;
using magic.lambda.mssql.crud.builders;

namespace magic.lambda.mssql.crud
{
    [Slot(Name = "mssql.create")]
    public class Create : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            var builder = new SqlCreateBuilder(input, signaler);
            var sqlNode = builder.Build();

            // Checking if this is a "build only" invocation.
            if (builder.IsGenerateOnly)
            {
                input.Value = sqlNode.Value;
                input.Clear();
                input.AddRange(sqlNode.Children.ToList());
                return;
            }

            // Executing SQL, now parametrized.
            Executor.Execute(sqlNode, signaler.Peek<SqlConnection>("mssql-connection"), signaler, (cmd) =>
            {
                // Notice, create SQL returns last inserted ID!
                input.Value = cmd.ExecuteScalar();
                input.Clear();
            });
        }
    }
}
