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
    /// The [mysql.read] slot class
    /// </summary>
    [Slot(Name = "mysql.read")]
    public class Read : ISlot
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var builder = new SqlReadBuilder(input, signaler);
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
                using (var reader = cmd.ExecuteReader())
                {
                    input.Clear();
                    while (reader.Read())
                    {
                        var rowNode = new Node(".");
                        for (var idxCol = 0; idxCol < reader.FieldCount; idxCol++)
                        {
                            var colNode = new Node(reader.GetName(idxCol), reader[idxCol]);
                            rowNode.Add(colNode);
                        }
                        input.Add(rowNode);
                    }
                }
            });
        }
    }
}
