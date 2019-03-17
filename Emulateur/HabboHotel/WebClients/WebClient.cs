﻿using Butterfly.Communication.Packets.Incoming;
using Butterfly.Communication.Packets.Outgoing;
using Butterfly.Core;
using Butterfly.Database.Interfaces;
using Butterfly.Communication.WebSocket;
using Butterfly.Net;
using ConnectionManager;
using SharedPacketLib;
using System;
using System.Data;
using Butterfly.Communication.Packets.Outgoing.WebSocket;

namespace Butterfly.HabboHotel.WebClients
{
    public class WebClient
    {
        private readonly ConnectionInformation _connection;
        private readonly WebPacketParser _packetParser;

        private bool _isStaff;
        public Language Langue { get; private set; }

        public int UserId { get; private set; }

        public int ConnectionID { get; }

        public WebClient(int id, ConnectionInformation connection)
        {
            this.ConnectionID = id;
            this._isStaff = false;
            this._connection = connection;
            this._packetParser = new WebPacketParser(this);
        }

        public void TryAuthenticate(string AuthTicket)
        {
            if (string.IsNullOrEmpty(AuthTicket))
                return;

            string ip = this.GetConnection().getIp();


            DataRow dUserInfo;

            using (IQueryAdapter queryreactor = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                queryreactor.SetQuery("SELECT id FROM bans WHERE expire > @nowtime AND (bantype = 'ip' AND value = @IP1) LIMIT 1");
                queryreactor.AddParameter("nowtime", ButterflyEnvironment.GetUnixTimestamp());
                queryreactor.AddParameter("IP1", ip);

                DataRow IsBanned = queryreactor.GetRow();
                if (IsBanned != null)
                    return;

                queryreactor.SetQuery("SELECT user_id, is_staff, langue FROM user_websocket WHERE auth_ticket = @sso");
                queryreactor.AddParameter("sso", AuthTicket);
                

                dUserInfo = queryreactor.GetRow();
                if (dUserInfo == null)
                    return;

                this.UserId = Convert.ToInt32(dUserInfo["user_id"]);
                this._isStaff = ButterflyEnvironment.EnumToBool((string)dUserInfo["is_staff"]);
                this.Langue = LanguageManager.ParseLanguage(Convert.ToString(dUserInfo["langue"]));
                queryreactor.RunQuery("UPDATE user_websocket SET auth_ticket = '' WHERE user_id = '" + UserId + "'");
            }

            ButterflyEnvironment.GetGame().GetClientWebManager().LogClonesOut(UserId);
            ButterflyEnvironment.GetGame().GetClientWebManager().RegisterClient(this, UserId);

            this.SendPacket(new AuthOkComposer());
            this.SendPacket(new UserIsStaffComposer(this._isStaff));
            //this.SendPacket(new NotifTopInitComposer(ButterflyEnvironment.GetGame().GetNotifTopManager().GetAllMessages()));

        }

        private void SwitchParserRequest()
        {
            this._packetParser.onNewPacket += new WebPacketParser.HandlePacket(this.parser_onNewPacket);

            byte[] packet = (this._connection.parser as InitialPacketParser).currentData;
            this._connection.parser.Dispose();
            this._connection.parser = (IDataParser)this._packetParser;
            this._connection.parser.handlePacketData(packet);
        }

        private void parser_onNewPacket(ClientPacket Message)
        {
            try
            {
                ButterflyEnvironment.GetGame().GetPacketManager().TryExecuteWebPacket(this, Message);
            }
            catch (Exception ex)
            {
                Logging.LogPacketException(Message.ToString(), (ex).ToString());
            }
        }

        public ConnectionInformation GetConnection()
        {
            return this._connection;
        }

        public void StartConnection()
        {
            if (this._connection == null)
                return;

            (this._connection.parser as InitialPacketParser).SwitchParserRequest += new InitialPacketParser.NoParamDelegate(this.SwitchParserRequest);

            this._connection.startPacketProcessing();
        }

        public void Dispose()
        {
            if (this._connection != null)
                this._connection.Dispose();
        }

        public void SendPacket(IServerPacket Message)
        {
            if (Message == null || this.GetConnection() == null)
                return;
            this.GetConnection().SendData(EncodeDecode.EncodeMessage(Message.GetBytes()));
        }
    }
}
