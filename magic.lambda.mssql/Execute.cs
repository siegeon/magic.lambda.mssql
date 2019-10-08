/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Data.SqlClient;
using System.Threading.Tasks;
using magic.node;
using magic.data.common;
using magic.signals.contracts;

namespace magic.lambda.mssql
{
    /// <summary>
    /// [mssql.execute] slot for executing a non query type of SQL.
    /// </summary>
    [Slot(Name = "mssql.execute")]
    public class Execute : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            Executor.Execute(
                input,
                signaler.Peek<SqlConnection>("mssql.connect"),
                signaler.Peek<Transaction>("mssql.transaction"),
                (cmd) =>
            {
                input.Value = cmd.ExecuteNonQuery();
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
            await Executor.ExecuteAsync(
                input, 
                signaler.Peek<SqlConnection>("mssql.connect"),
                signaler.Peek<Transaction>("mssql.transaction"),
                async (cmd) =>
            {
                input.Value = await cmd.ExecuteNonQueryAsync();
            });
        }
    }
}
