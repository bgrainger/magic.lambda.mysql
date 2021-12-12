/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using magic.node;
using magic.signals.contracts;
using builder = magic.data.common.builders;

namespace magic.lambda.mysql.crud.builders
{
    /// <summary>
    /// Specialised select SQL builder, to create a select MySQL SQL statement
    /// by semantically traversing an input node.
    /// </summary>
    public class SqlReadBuilder : builder.SqlReadBuilder
    {
        /// <summary>
        /// Creates a select SQL statement
        /// </summary>
        /// <param name="node">Root node to generate your SQL from.</param>
        /// <param name="signaler">Signaler to invoke slots.</param>
        public SqlReadBuilder(Node node, ISignaler signaler)
            : base(node, "`")
        { }
    }
}
