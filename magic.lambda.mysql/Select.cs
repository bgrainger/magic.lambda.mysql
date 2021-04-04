/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using magic.node;
using magic.data.common;
using magic.node.extensions;
using magic.signals.contracts;
using magic.lambda.mysql.helpers;

namespace magic.lambda.mysql
{
    /// <summary>
    /// [mysql.select] slot for executing a select type of SQL command, that returns
    /// a row set.
    /// </summary>
    [Slot(Name = "mysql.select")]
    public class Select : ISlot, ISlotAsync
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            var mrsNode = input.Children.FirstOrDefault(x => x.Name == "multiple-result-sets");
            var multipleResultSets = mrsNode?.GetEx<bool>() ?? false;
            mrsNode?.UnTie();
            Executor.Execute(
                input,
                signaler.Peek<MySqlConnectionWrapper>("mysql.connect").Connection,
                signaler.Peek<Transaction>("mysql.transaction"),
                (cmd, max) =>
            {
                using (var reader = cmd.ExecuteReader())
                {
                    do
                    {
                        Node parentNode;
                        if (multipleResultSets)
                        {
                            parentNode = new Node();
                            input.Add(parentNode);
                        }
                        else
                        {
                            parentNode = input;
                        }
                        while (reader.Read())
                        {
                            if (max != -1 && max-- == 0)
                                break; // Reached maximum limit
                            var rowNode = new Node();
                            for (var idxCol = 0; idxCol < reader.FieldCount; idxCol++)
                            {
                                var colNode = new Node(reader.GetName(idxCol), magic.data.common.Converter.GetValue(reader[idxCol]));
                                rowNode.Add(colNode);
                            }
                            parentNode.Add(rowNode);
                        }
                    } while (multipleResultSets && reader.NextResult());
                }
            });
        }

        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            var mrsNode = input.Children.FirstOrDefault(x => x.Name == "multiple-result-sets");
            var multipleResultSets = mrsNode?.GetEx<bool>() ?? false;
            mrsNode?.UnTie();
            await Executor.ExecuteAsync(
                input,
                signaler.Peek<MySqlConnectionWrapper>("mysql.connect").Connection,
                signaler.Peek<Transaction>("mysql.transaction"),
                async (cmd, max) =>
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    do
                    {
                        Node parentNode;
                        if (multipleResultSets)
                        {
                            parentNode = new Node();
                            input.Add(parentNode);
                        }
                        else
                        {
                            parentNode = input;
                        }
                        while (await reader.ReadAsync())
                        {
                            if (max != -1 && max-- == 0)
                                break; // Reached maximum limit
                            var rowNode = new Node();
                            for (var idxCol = 0; idxCol < reader.FieldCount; idxCol++)
                            {
                                var colNode = new Node(reader.GetName(idxCol), magic.data.common.Converter.GetValue(reader[idxCol]));
                                rowNode.Add(colNode);
                            }
                            parentNode.Add(rowNode);
                        }
                    } while (multipleResultSets && await reader.NextResultAsync());
                }
            });
        }
    }
}
