namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class CanCreateRoomComposer : ServerPacket
    {
        public CanCreateRoomComposer(bool Error, int MaxRoomsPerUser)
            : base(ServerPacketHeader.CanCreateRoomMessageComposer)
        {
            base.WriteInteger(Error ? 1 : 0);
            base.WriteInteger(MaxRoomsPerUser);
        }
    }
}
