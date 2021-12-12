/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.mssql.helpers;
using magic.lambda.mssql.crud.builders;
using help = magic.data.common.helpers;
using build = magic.data.common.builders;

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
            /*
             * Figuring out if we should return ID of newly created
             * record to caller.
             */
            var returnId = input.Children
                .FirstOrDefault(x => x.Name == "return-id")?.GetEx<bool>() ?? true;

            // Parsing and creating SQL.
            var exe = returnId ?
                build.SqlBuilder.Parse<SqlCreateBuilder>(signaler, input) :
                build.SqlBuilder.Parse<SqlCreateBuilderNoId>(signaler, input);

            /*
             * If the parsing process doesn't return a node, we're not supposed
             * to actually execute the SQL, but only to generate it
             * and parametrize it.
             */
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            help.Executor.Execute(
                exe,
                signaler.Peek<SqlConnectionWrapper>("mssql.connect").Connection,
                signaler.Peek<help.Transaction>("mssql.transaction"),
                (cmd, _) =>
            {
                /*
                 * Notice, create SQL returns last inserted ID, but only
                 * if caller told us he wanted it to do such.
                 */
                if (returnId)
                {
                    input.Value = cmd.ExecuteScalar();
                }
                else
                {
                    cmd.ExecuteNonQuery();
                    input.Value = null;
                }
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
            /*
             * Figuring out if we should return ID of newly created
             * record to caller.
             */
            var returnId = input.Children
                .FirstOrDefault(x => x.Name == "return-id")?.GetEx<bool>() ?? true;

            // Parsing and creating SQL.
            var exe = returnId ?
                build.SqlBuilder.Parse<SqlCreateBuilder>(signaler, input) :
                build.SqlBuilder.Parse<SqlCreateBuilderNoId>(signaler, input);

            /*
             * If the parsing process doesn't return a node, we're not supposed
             * to actually execute the SQL, but only to generate it
             * and parametrize it.
             */
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            await help.Executor.ExecuteAsync(
                exe,
                signaler.Peek<SqlConnectionWrapper>("mssql.connect").Connection,
                signaler.Peek<help.Transaction>("mssql.transaction"),
                async (cmd, _) =>
            {
                /*
                 * Notice, create SQL returns last inserted ID, but only
                 * if caller told us he wanted it to do such.
                 */
                if (returnId)
                {
                    input.Value = await cmd.ExecuteScalarAsync();
                }
                else
                {
                    await cmd.ExecuteNonQueryAsync();
                    input.Value = null;
                }
                input.Clear();
            });
        }
    }
}
