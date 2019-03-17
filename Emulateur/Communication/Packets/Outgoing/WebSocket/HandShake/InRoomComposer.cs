﻿namespace Butterfly.Communication.Packets.Outgoing.WebSocket
{
    class InRoomComposer: ServerPacket
    {
        public InRoomComposer(bool InRoom)
            : base(5)
        {
            base.WriteBoolean(InRoom);
        }
    }
}