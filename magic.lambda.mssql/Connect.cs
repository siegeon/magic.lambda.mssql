/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.mssql
{
    /// <summary>
    /// [mssql.connect] slot, for connecting to a MS SQL Server database instance.
    /// </summary>
	[Slot(Name = "mssql.connect")]
	public class Connect : ISlot
	{
        readonly IConfiguration _configuration;

        /// <summary>
        /// Creates a new instance of your type.
        /// </summary>
        /// <param name="configuration">Configuration for your application.</param>
        public Connect(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        public void Signal(ISignaler signaler, Node input)
		{
            var connectionString = input.GetEx<string>();

            // Checking if this is a "generic connection string".
            if (connectionString.StartsWith("[", StringComparison.InvariantCulture) &&
                connectionString.EndsWith("]", StringComparison.InvariantCulture))
            {
                var generic = _configuration["databases:mssql:generic"];
                connectionString = generic.Replace("{database}", connectionString.Substring(1, connectionString.Length - 2));
            }
            else if (!connectionString.Contains(";"))
            {
                var generic = _configuration["databases:mssql:generic"];
                connectionString = generic.Replace("{database}", connectionString);
            }

			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();
                signaler.Scope("mssql.connect", connection, () =>
                {
                    signaler.Signal("eval", input);
                });
				input.Value = null;
			}
		}
	}
}
