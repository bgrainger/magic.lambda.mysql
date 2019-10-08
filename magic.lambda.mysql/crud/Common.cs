/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using magic.node;
using magic.signals.contracts;
using com = magic.data.common;

namespace magic.lambda.mssql.crud
{
    /*
     * Helper class for CRUD operations.
     */
    internal static class Common
    {
        /*
         * Builds our SQL according to the specified node, and returns true
         * if SQL should not be executed, but this is a "generate only" operation.
         */
        public static bool ParseNode<T>(ISignaler signaler, Node input, out Node sqlNode) where T : com.SqlBuilder
        {
            /*
             * Unfortunately this is our only method to create an instance of type,
             * since it requires arguments in its CTOR.
             */
            var builder = Activator.CreateInstance(typeof(T), new object[] { input, signaler }) as T;
            sqlNode = builder.Build();

            // Checking if this is a "build only" invocation.
            if (builder.IsGenerateOnly)
            {
                input.Value = sqlNode.Value;
                input.Clear();
                input.AddRange(sqlNode.Children.ToList());
                return true;
            }
            return false;
        }
    }
}
