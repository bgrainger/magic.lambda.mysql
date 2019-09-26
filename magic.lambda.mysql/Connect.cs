/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.mysql
{
	[Slot(Name = "mysql.connect")]
	public class Connect : ISlot
	{
        readonly IConfiguration _configuration;

        public Connect(IConfiguration configuration)
		{
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

		public void Signal(ISignaler signaler, Node input)
		{
            var connectionString = input.GetEx<string>();

            // Checking if this is a "generic connection string".
            if (connectionString.StartsWith("[", StringComparison.InvariantCulture) &&
                connectionString.EndsWith("]", StringComparison.InvariantCulture))
            {
                var generic = _configuration["databases:mysql:generic"];
                connectionString = generic.Replace("{database}", connectionString.Substring(1, connectionString.Length - 2));
            }
            else if (!connectionString.Contains(";"))
            {
                var generic = _configuration["databases:mysql:generic"];
                connectionString = generic.Replace("{database}", connectionString);
            }

            using (var connection = new MySqlConnection(connectionString))
			{
                connection.Open();
                signaler.Scope("mysql-connection", connection, () =>
                {
                    signaler.Signal("eval", input);
                });
                input.Value = null;
            }
        }
	}
}
