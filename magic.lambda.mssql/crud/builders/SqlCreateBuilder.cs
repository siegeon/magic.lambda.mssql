/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Text;
using magic.node;
using com = magic.data.common;
using magic.signals.contracts;

namespace magic.lambda.mssql.crud.builders
{
    /// <summary>
    /// Create SQL type of builder for MS SQL Server types of statements.
    /// </summary>
    public class SqlCreateBuilder : com.SqlCreateBuilder
    {
        /// <summary>
        /// Creates a new instance of your class.
        /// </summary>
        /// <param name="node">Arguments used to semantically build your SQL.</param>
        /// <param name="signaler">Signaler used to invoke the original slot.</param>
        public SqlCreateBuilder(Node node, ISignaler signaler)
            : base(node, "\"")
        { }

        /// <summary>
        /// Appends the "in between" parts of your SQL.
        /// </summary>
        /// <param name="builder">Builder where to put the content.</param>
        protected override void GetInBetween(StringBuilder builder)
        {
            builder.Append(" output inserted.id");
        }
    }
}
