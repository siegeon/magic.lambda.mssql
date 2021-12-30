/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using magic.node;
using build = magic.data.common.builders;

namespace magic.lambda.mssql.crud.builders
{
    /// <summary>
    /// Builder for creating a delete type of MS SQL Server statement.
    /// </summary>
    public class SqlDeleteBuilder : build.SqlDeleteBuilder
    {
        /// <summary>
        /// Creates an instance of your type.
        /// </summary>
        /// <param name="node">Arguments to build your SQL from.</param>
        public SqlDeleteBuilder(Node node)
            : base(node, "\"")
        { }
    }
}
