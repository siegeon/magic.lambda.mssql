/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Data.SqlClient;
using magic.signals.contracts;

namespace magic.lambda.mssql.utilities
{
    /*
     * Helper class wrapping a transaction, creating a guarantee of that the
     * transaction is rolled back, unless it's explicitly committed.
     */
    internal class Transaction : IDisposable
    {
        readonly SqlTransaction _transaction;
        bool _signaled;

        /*
         * Creates a new instance of your type.
         */
        public Transaction(ISignaler signaler)
        {
            _transaction = signaler.Peek<SqlConnection>("mssql.connect").BeginTransaction();
        }

        /*
         * Rolls back the transaction.
         */
        public void Rollback()
        {
            _signaled = true;
            _transaction.Rollback();
        }

        /*
         * Committing your transaction.
         */
        public void Commit()
        {
            _signaled = true;
            _transaction.Commit();
        }

        public void Dispose()
        {
            if (!_signaled)
                _transaction.Rollback();
        }
    }
}
