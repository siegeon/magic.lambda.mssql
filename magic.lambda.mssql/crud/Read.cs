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
    /// [mssql.read] slot for selecting rows from some table.
    /// </summary>
    [Slot(Name = "mssql.read")]
    public class Read : ISlot, ISlotAsync
    {
        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Parsing and creating SQL.
            var exe = help.SqlBuilder.Parse<SqlReadBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            help.Executor.Execute(
                exe,
                signaler.Peek<SqlConnectionWrapper>("mssql.connect").Connection,
                signaler.Peek<help.Transaction>("mssql.transaction"),
                (cmd, _) =>
            {
                using (var reader = cmd.ExecuteReader())
                {
                    input.Clear();
                    while (reader.Read())
                    {
                        var rowNode = new Node(".");
                        for (var idxCol = 0; idxCol < reader.FieldCount; idxCol++)
                        {
                            var colNode = new Node(reader.GetName(idxCol), help.Converter.GetValue(reader[idxCol]));
                            rowNode.Add(colNode);
                        }
                        input.Add(rowNode);
                    }
                }
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
            var exe = help.SqlBuilder.Parse<SqlReadBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            await help.Executor.ExecuteAsync(
                exe,
                signaler.Peek<SqlConnectionWrapper>("mssql.connect").Connection,
                signaler.Peek<help.Transaction>("mssql.transaction"),
                async (cmd, _) =>
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    input.Clear();
                    while (await reader.ReadAsync())
                    {
                        var rowNode = new Node(".");
                        for (var idxCol = 0; idxCol < reader.FieldCount; idxCol++)
                        {
                            var colNode = new Node(reader.GetName(idxCol), help.Converter.GetValue(reader[idxCol]));
                            rowNode.Add(colNode);
                        }
                        input.Add(rowNode);
                    }
                }
            });
        }
    }
}
