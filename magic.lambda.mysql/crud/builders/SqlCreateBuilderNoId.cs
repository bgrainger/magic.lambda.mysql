/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using magic.node;
using magic.signals.contracts;
using builder = magic.data.common.builders;

namespace magic.lambda.mysql.crud.builders
{
    /// <summary>
    /// Specialised insert SQL builder, to create an insert MySQL SQL statement
    /// by semantically traversing an input node, that does not return the ID
    /// of the newly created record.
    /// </summary>
    public class SqlCreateBuilderNoId : builder.SqlCreateBuilder
    {
        /// <summary>
        /// Creates an insert SQL statement
        /// </summary>
        /// <param name="node">Root node to generate your SQL from.</param>
        /// <param name="signaler">Signaler to invoke slots.</param>
        public SqlCreateBuilderNoId(Node node, ISignaler signaler)
            : base(node, "`")
        { }
    }
}
