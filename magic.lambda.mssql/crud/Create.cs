/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Data.SqlClient;
using System.Threading.Tasks;
using magic.node;
using com = magic.data.common;
using magic.signals.contracts;
using magic.lambda.mssql.crud.builders;

namespace magic.lambda.mssql.crud
{
    /// <summary>
    /// [mssql.create] slot for creating a new record in some table.
    /// </summary>
    [Slot(Name = "mssql.create")]
    public class Create : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Parsing and creating SQL.
            var exe = com.SqlBuilder.Parse<SqlCreateBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            com.Executor.Execute(
                exe, 
                signaler.Peek<SqlConnection>("mssql.connect"),
                signaler.Peek<com.Transaction>("mssql.transaction"),
                (cmd) =>
            {
                // Notice, create SQL returns last inserted ID!
                input.Value = cmd.ExecuteScalar();
                input.Clear();
            });
        }

        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            // Parsing and creating SQL.
            var exe = com.SqlBuilder.Parse<SqlCreateBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            await com.Executor.ExecuteAsync(
                exe, 
                signaler.Peek<SqlConnection>("mssql.connect"),
                signaler.Peek<com.Transaction>("mssql.transaction"),
                async (cmd) =>
            {
                // Notice, create SQL returns last inserted ID!
                input.Value = await cmd.ExecuteScalarAsync();
                input.Clear();
            });
        }
    }
}
