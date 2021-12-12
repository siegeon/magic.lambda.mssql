/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Threading.Tasks;
using magic.node;
using magic.signals.contracts;
using magic.lambda.mssql.helpers;
using help = magic.data.common.helpers;
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
            var exe = help.SqlBuilder.Parse<SqlUpdateBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            help.Executor.Execute(
                exe,
                signaler.Peek<SqlConnectionWrapper>("mssql.connect").Connection,
                signaler.Peek<help.Transaction>("mssql.transaction"),
                (cmd, _) =>
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
            var exe = help.SqlBuilder.Parse<SqlUpdateBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            await help.Executor.ExecuteAsync(
                exe,
                signaler.Peek<SqlConnectionWrapper>("mssql.connect").Connection,
                signaler.Peek<help.Transaction>("mssql.transaction"),
                async (cmd, _) =>
            {
                input.Value = await cmd.ExecuteNonQueryAsync();
                input.Clear();
            });
        }
    }
}
