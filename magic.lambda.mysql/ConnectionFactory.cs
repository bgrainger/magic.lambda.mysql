﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using MySql.Data.MySqlClient;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.mysql
{
    /// <summary>
    /// [.db-factory.connection.mysql] slot for creating a MySQL connection and returning to caller.
    /// </summary>
    [Slot(Name = ".db-factory.connection.mysql")]
    public class ConnectionFactory : ISlot
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            input.Value = new MySqlConnection();
        }
    }
}
