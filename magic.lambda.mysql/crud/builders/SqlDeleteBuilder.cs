/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using magic.node;
using magic.signals.contracts;
using builder = magic.data.common.builders;

namespace magic.lambda.mysql.crud.builders
{
    /// <summary>
    /// Specialised delete SQL builder, to create a delete MySQL SQL statement
    /// by semantically traversing an input node.
    /// </summary>
    public class SqlDeleteBuilder : builder.SqlDeleteBuilder
    {
        /// <summary>
        /// Creates a delete SQL statement
        /// </summary>
        /// <param name="node">Root node to generate your SQL from.</param>
        /// <param name="signaler">Signaler to invoke slots.</param>
        public SqlDeleteBuilder(Node node, ISignaler signaler)
            : base(node, "`")
        { }
    }
}
