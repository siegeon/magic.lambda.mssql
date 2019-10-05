/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using magic.node;
using com = magic.data.common;
using magic.signals.contracts;

namespace magic.lambda.mssql.crud.builders
{
    /// <summary>
    /// Builder to create an update type of MS SQL Server statement.
    /// </summary>
    public class SqlUpdateBuilder : com.SqlUpdateBuilder
    {
        /// <summary>
        /// Creates an instance of your type.
        /// </summary>
        /// <param name="node">Arguments to create your statement from.</param>
        /// <param name="signaler">Signaler used to invoke the original slot.</param>
        public SqlUpdateBuilder(Node node, ISignaler signaler)
            : base(node, "\"")
        { }
    }
}
