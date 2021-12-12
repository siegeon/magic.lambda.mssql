/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using magic.node;
using magic.signals.contracts;
using build = magic.data.common.builders;

namespace magic.lambda.mssql.crud.builders
{
    /// <summary>
    /// Builder to create an update type of MS SQL Server statement.
    /// </summary>
    public class SqlUpdateBuilder : build.SqlUpdateBuilder
    {
        /// <summary>
        /// Creates an instance of your type.
        /// </summary>
        /// <param name="node">Arguments to create your statement from.</param>
        public SqlUpdateBuilder(Node node)
            : base(node, "\"")
        { }
    }
}
