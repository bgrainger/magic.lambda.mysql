/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using MySql.Data.MySqlClient;
using magic.node;
using magic.signals.contracts;
using magic.lambda.mysql.utilities;
using magic.lambda.mysql.crud.builders;

namespace magic.lambda.mysql.crud
{
    /// <summary>
    /// The [mysql.create] slot class
    /// </summary>
    [Slot(Name = "mysql.create")]
    public class Create : ISlot
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var builder = new SqlCreateBuilder(input, signaler);
            var sqlNode = builder.Build();

            // Checking if this is a "build only" invocation.
            if (builder.IsGenerateOnly)
            {
                input.Value = sqlNode.Value;
                input.Clear();
                input.AddRange(sqlNode.Children.ToList());
                return;
            }

            // Executing SQL, now parametrized.
            Executor.Execute(sqlNode, signaler.Peek<MySqlConnection>("mysql-connection"), signaler, (cmd) =>
            {
                // Notice, create SQL returns last inserted ID!
                input.Value = cmd.ExecuteScalar();
                input.Clear();
            });
        }
    }
}
