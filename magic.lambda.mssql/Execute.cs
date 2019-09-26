/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using magic.node;
using magic.signals.contracts;
using magic.lambda.mssql.utilities;

namespace magic.lambda.mssql
{
    [Slot(Name = "mssql.execute")]
    public class Execute : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            Executor.Execute(input, signaler.Peek<SqlConnection>("mssql-connection"), signaler, (cmd) =>
            {
                input.Value = cmd.ExecuteNonQuery();
            });
        }
    }
}
