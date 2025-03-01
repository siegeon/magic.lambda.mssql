﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Threading.Tasks;
using magic.node;
using magic.node.contracts;
using magic.signals.contracts;
using magic.data.common.helpers;
using magic.lambda.mssql.helpers;

namespace magic.lambda.mssql
{
    /// <summary>
    /// [mssql.connect] slot, for connecting to a MS SQL Server database instance.
    /// </summary>
    [Slot(Name = "mssql.connect")]
    public class Connect : ISlot, ISlotAsync
    {
        readonly IMagicConfiguration _configuration;

        /// <summary>
        /// Creates a new instance of your type.
        /// </summary>
        /// <param name="configuration">Configuration for your application.</param>
        public Connect(IMagicConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            using (var connection = new SqlConnectionWrapper(
                Executor.GetConnectionString(
                    input,
                    "mssql",
                    "master",
                    _configuration)))
            {
                signaler.Scope(
                    "mssql.connect",
                    connection, () => signaler.Signal("eval", input));
                input.Value = null;
            }
        }

        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            using (var connection = new SqlConnectionWrapper(
                Executor.GetConnectionString(
                    input,
                    "mssql",
                    "master",
                    _configuration)))
            {
                await signaler.ScopeAsync(
                    "mssql.connect",
                    connection,
                    async () => await signaler.SignalAsync("eval", input));
                input.Value = null;
            }
        }
    }
}
