/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Data.SqlClient;
using System.Threading.Tasks;
using magic.node;
using magic.signals.contracts;
using magic.lambda.mssql.utilities;
using magic.lambda.mssql.crud.builders;

namespace magic.lambda.mssql.crud
{
    /// <summary>
    /// [mssql.update] slot for updating a record in some table.
    /// </summary>
    [Slot(Name = "mssql.update")]
    public class Update : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Parsing and creating SQL.
            if (Common.ParseNode<SqlUpdateBuilder>(signaler, input, out Node sqlNode))
                return;

            // Executing SQL, now parametrized.
            Executor.Execute(sqlNode, signaler.Peek<SqlConnection>("mssql.connect"), (cmd) =>
            {
                input.Value = cmd.ExecuteNonQuery();
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
            if (Common.ParseNode<SqlUpdateBuilder>(signaler, input, out Node sqlNode))
                return;

            // Executing SQL, now parametrized.
            await Executor.ExecuteAsync(sqlNode, signaler.Peek<SqlConnection>("mssql.connect"), async (cmd) =>
            {
                input.Value = await cmd.ExecuteNonQueryAsync();
                input.Clear();
            });
        }
    }
}
