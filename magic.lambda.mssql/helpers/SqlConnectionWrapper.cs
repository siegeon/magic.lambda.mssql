/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Data.SqlClient;

namespace magic.lambda.mssql.helpers
{
    internal class SqlConnectionWrapper : IDisposable
    {
        readonly Lazy<SqlConnection> _connection;

        public SqlConnectionWrapper(string connectionString)
        {
            _connection = new Lazy<SqlConnection>(() => new SqlConnection(connectionString));
        }

        /*
         * Property to retrieve underlying MySQL connection.
         */
        public SqlConnection Connection
        {
            get
            {
                if (!_connection.IsValueCreated)
                    _connection.Value.Open();

                return _connection.Value;
            }
        }

        public void Dispose()
        {
            if (_connection.IsValueCreated)
                _connection.Value.Dispose();
        }
    }
}
