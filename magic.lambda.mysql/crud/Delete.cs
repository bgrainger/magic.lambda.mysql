/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Threading.Tasks;
using magic.node;
using magic.signals.contracts;
using magic.lambda.mysql.helpers;
using help = magic.data.common.helpers;
using magic.lambda.mysql.crud.builders;

namespace magic.lambda.mysql.crud
{
    /// <summary>
    /// The [mysql.delete] slot class
    /// </summary>
    [Slot(Name = "mysql.delete")]
    public class Delete : ISlot, ISlotAsync
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Parsing and creating SQL.
            var exe = help.SqlBuilder.Parse<SqlDeleteBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            help.Executor.Execute(
                exe,
                signaler.Peek<MySqlConnectionWrapper>("mysql.connect").Connection,
                signaler.Peek<help.Transaction>("mysql.transaction"),
                (cmd, _) =>
            {
                input.Value = cmd.ExecuteNonQuery();
                input.Clear();
            });
        }

        /// <summary>
        /// Implementation of your slot.
        /// </summary>
        /// <param name="signaler">Signaler used to raise the signal.</param>
        /// <param name="input">Arguments to your slot.</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            // Parsing and creating SQL.
            var exe = help.SqlBuilder.Parse<SqlDeleteBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            await help.Executor.ExecuteAsync(
                exe,
                signaler.Peek<MySqlConnectionWrapper>("mysql.connect").Connection,
                signaler.Peek<help.Transaction>("mysql.transaction"),
                async (cmd, _) =>
            {
                input.Value = await cmd.ExecuteNonQueryAsync();
                input.Clear();
            });
        }
    }
}
