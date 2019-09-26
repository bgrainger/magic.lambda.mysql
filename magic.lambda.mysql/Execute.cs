/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using MySql.Data.MySqlClient;
using magic.node;
using magic.signals.contracts;
using magic.lambda.mysql.utilities;

namespace magic.lambda.mysql
{
    [Slot(Name = "mysql.execute")]
    public class Execute : ISlot
    {
        public void Signal(ISignaler signaler, Node input)
        {
            Executor.Execute(input, signaler.Peek<MySqlConnection>("mssql-connection"), signaler, (cmd) =>
            {
                input.Value = cmd.ExecuteNonQuery();
            });
        }
    }
}
