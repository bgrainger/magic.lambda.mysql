/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Text;
using magic.node;
using magic.signals.contracts;
using com = magic.data.common;

namespace magic.lambda.mysql.crud.builders
{
    /// <summary>
    /// Specialised insert SQL builder, to create an insert MySQL SQL statement
    /// by semantically traversing an input node.
    /// </summary>
    public class SqlCreateBuilder : com.SqlCreateBuilder
    {
        /// <summary>
        /// Creates an insert SQL statement
        /// </summary>
        /// <param name="node">Root node to generate your SQL from.</param>
        /// <param name="signaler">Signaler to invoke slots.</param>
        public SqlCreateBuilder(Node node, ISignaler signaler)
            : base(node, "`")
        { }

        /// <summary>
        /// Overridden to generate the tail parts of your SQL.
        /// </summary>
        /// <param name="builder">Where to put your tail.</param>
        protected override void AppendTail(StringBuilder builder)
        {
            builder.Append("; select last_insert_id();");
        }
    }
}
