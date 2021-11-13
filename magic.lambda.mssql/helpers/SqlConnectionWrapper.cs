/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using System.Data.SqlClient;

namespace magic.lambda.mssql.helpers
{
    internal sealed class SqlConnectionWrapper : IDisposable
    {
        readonly Lazy<SqlConnection> _connection;

        public SqlConnectionWrapper(string connectionString)
        {
            _connection = new Lazy<SqlConnection>(() =>
            {
                var connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            });
        }

        /*
         * Property to retrieve underlying MySQL connection.
         */
        public SqlConnection Connection => _connection.Value;

        public void Dispose()
        {
            if (_connection.IsValueCreated)
                _connection.Value.Dispose();
        }
    }
}
