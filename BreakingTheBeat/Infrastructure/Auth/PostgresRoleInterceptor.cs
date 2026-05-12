using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Gateway.Infrastructure.Auth
{ 

    public class PostgresRoleInterceptor : DbConnectionInterceptor
    {
        private readonly PostgresRoleService _roleService;

        public PostgresRoleInterceptor(PostgresRoleService roleService)
        {
            _roleService = roleService;
        }

        public override async Task ConnectionOpenedAsync(DbConnection connection,ConnectionEndEventData eventData,CancellationToken cancellationToken = default)
        {
            var role = await _roleService.GetDatabaseRole();

            Console.WriteLine($"SET ROLE: {role}");

            await using var command = connection.CreateCommand();

            command.CommandText = $"SET ROLE {role};";

            await command.ExecuteNonQueryAsync(cancellationToken);

            await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        }
    }
}
