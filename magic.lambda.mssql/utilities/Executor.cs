/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Data.SqlClient;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.mssql.utilities
{
    /*
     * Helper class for creating an SQL command.
     */
	public static class Executor
    {
        /*
         * Creates and parametrizes a SQL command, with the specified parameters,
         * for then to invoke the specified callback with the command.
         */
        public static void Execute(
            Node input,
            SqlConnection connection,
            Action<SqlCommand> functor)
        {
            using (var cmd = new SqlCommand(input.GetEx<string>(), connection))
            {
                foreach (var idxPar in input.Children)
                {
                    cmd.Parameters.AddWithValue(idxPar.Name, idxPar.Get<object>());
                }

                // Making sure we clean nodes before invoking lambda callback.
                input.Value = null;
                input.Clear();

                // Invoking lambda callback supplied by caller.
                functor(cmd);
            }
        }
    }
}
