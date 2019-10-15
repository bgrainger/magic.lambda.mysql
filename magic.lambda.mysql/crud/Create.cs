/*
 * Magic, Copyright(c) Thomas Hansen 2019, thomas@gaiasoul.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using magic.node;
using com = magic.data.common;
using magic.signals.contracts;
using magic.lambda.mysql.crud.builders;

namespace magic.lambda.mysql.crud
{
    /// <summary>
    /// The [mysql.create] slot class
    /// </summary>
    [Slot(Name = "mysql.create")]
    public class Create : ISlot, ISlotAsync
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            // Parsing and creating SQL.
            var exe = com.SqlBuilder.Parse<SqlCreateBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            com.Executor.Execute(
                exe,
                signaler.Peek<MySqlConnection>("mysql.connect"),
                signaler.Peek<com.Transaction>("mysql.transaction"),
                (cmd) =>
            {
                // Notice, create SQL returns last inserted ID.
                input.Value = cmd.ExecuteScalar();
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
            var exe = com.SqlBuilder.Parse<SqlCreateBuilder>(signaler, input);
            if (exe == null)
                return;

            // Executing SQL, now parametrized.
            await com.Executor.ExecuteAsync(
                exe,
                signaler.Peek<MySqlConnection>("mysql.connect"),
                signaler.Peek<com.Transaction>("mysql.transaction"),
                async (cmd) =>
            {
                // Notice, create SQL returns last inserted ID.
                input.Value = await cmd.ExecuteScalarAsync();
                input.Clear();
            });
        }
    }
}
