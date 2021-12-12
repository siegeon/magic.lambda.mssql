﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using magic.node;
using magic.signals.contracts;
using System.Threading.Tasks;
using magic.data.common.helpers;
using magic.lambda.mssql.helpers;

namespace magic.lambda.mysql
{
    /// <summary>
    /// [mssql.transaction.create] slot for creating a new MS SQL database transaction.
    /// </summary>
    [Slot(Name = "mssql.transaction.create")]
    public class CreateTransaction : ISlot, ISlotAsync
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            signaler.Scope(
                "mssql.transaction",
                new Transaction(signaler, signaler.Peek<SqlConnectionWrapper>("mssql.connect").Connection),
                () => signaler.Signal("eval", input));
        }

        /// <summary>
        /// [mssql.transaction.create] slot for creating a new MS SQL database transaction.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            await signaler.ScopeAsync(
                "mssql.transaction",
                new Transaction(signaler, signaler.Peek<SqlConnectionWrapper>("mssql.connect").Connection),
                async () => await signaler.SignalAsync("eval", input));
        }
    }
}
