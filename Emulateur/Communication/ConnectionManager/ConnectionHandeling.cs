﻿using Butterfly;
using Butterfly.Core;
using Butterfly.Net;
using System;

namespace ConnectionManager
{
    public class ConnectionHandeling
    {
        public GameSocketManager manager;

        public ConnectionHandeling(int port, int maxConnections, int connectionsPerIP)
        {
            this.manager = new GameSocketManager();
            this.manager.init(port, maxConnections, connectionsPerIP, new InitialPacketParser());

            this.manager.connectionEvent += new GameSocketManager.ConnectionEvent(this.manager_connectionEvent);
        }

        private void manager_connectionEvent(ConnectionInformation connection)
        {
            connection.connectionClose += new ConnectionInformation.ConnectionChange(this.connectionChanged);
            ButterflyEnvironment.GetGame().GetClientManager().CreateAndStartClient(connection.getConnectionID(), connection);
        }

        private void connectionChanged(ConnectionInformation information)
        {
            this.CloseConnection(information);
            information.connectionClose -= new ConnectionInformation.ConnectionChange(this.connectionChanged);
        }

        public void CloseConnection(ConnectionInformation Connection)
        {
            try
            {
                ButterflyEnvironment.GetGame().GetClientManager().DisposeConnection(Connection.getConnectionID());
                Connection.Dispose();
            }
            catch (Exception ex)
            {
                Logging.LogException((ex).ToString());
            }
        }


        public void Destroy()
        {
            this.manager.Destroy();
        }
    }
}
