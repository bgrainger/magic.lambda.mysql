/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using magic.node;
using magic.data.common;
using magic.signals.contracts;
using magic.lambda.mysql.helpers;

namespace magic.lambda.mysql
{
    /// <summary>
    /// [mysql.connect] slot for connecting to a MySQL server instance.
    /// </summary>
    [Slot(Name = "mysql.connect")]
    public class Connect : ISlot, ISlotAsync
    {
        readonly IConfiguration _configuration;

        /// <summary>
        /// Creates a new instance of your class.
        /// </summary>
        /// <param name="configuration">Configuration for your application.</param>
        public Connect(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            using (var connection = new MySqlConnectionWrapper(
                Executor.GetConnectionString(
                    input,
                    "mysql",
                    "information_schema",
                    _configuration)))
            {
                signaler.Scope(
                    "mysql.connect",
                    connection,
                    () => signaler.Signal("eval", input));
                input.Value = null;
            }
        }

        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            using (var connection = new MySqlConnectionWrapper(
                Executor.GetConnectionString(
                    input,
                    "mysql",
                    "information_schema",
                    _configuration)))
            {
                await signaler.ScopeAsync(
                    "mysql.connect",
                    connection,
                    async () => await signaler.SignalAsync("eval", input));
                input.Value = null;
            }
        }
    }
}
