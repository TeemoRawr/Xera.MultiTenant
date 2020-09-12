using System;

namespace Xera.MultiTenant.AspNetCore.Contracts
{
    public class Tenant
    {
        internal Tenant(
            Guid tenantId,
            string name,
            string database,
            string server,
            int serverPort,
            string serverUsername,
            string serverPassword)
        {
            TenantId = tenantId;
            Name = name;
            Database = database;
            Server = server;
            ServerPort = serverPort;
            ServerUsername = serverUsername;
            ServerPassword = serverPassword;
        }

        public Guid TenantId { get; }
        public string Name { get; }
        public string Database { get; }
        public string Server { get; }
        public int ServerPort { get; }
        public string ServerUsername { get; }
        public string ServerPassword { get; }

        public static Tenant Create(
            Guid tenantId,
            string name,
            string database,
            string server,
            int serverPort,
            string serverUsername,
            string serverPassword)
        {
            var tenant = new Tenant(tenantId, name, database, server, serverPort, serverUsername, serverPassword);

            return tenant;
        }
    }
}