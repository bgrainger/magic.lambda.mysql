﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using MySql.Data.MySqlClient;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.mysql.utilities
{
	internal static class Executor
    {
        public static void Execute(
            Node input,
            MySqlConnection connection,
            ISignaler signaler,
            Action<MySqlCommand> functor)
        {
            using (var cmd = new MySqlCommand(input.GetEx<string>(), connection))
            {
                foreach (var idxPar in input.Children)
                {
                    cmd.Parameters.AddWithValue(idxPar.Name, idxPar.Get<object>());
                }

                // Making sure we clean nodes before invoking lambda callback.
                input.Value = null;
                input.Clear();

                // Invoking lambda callback supplied by caller.
                functor(cmd);
            }
        }
    }
}
