namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class RoomInviteMessageComposer : ServerPacket
    {
        public RoomInviteMessageComposer()
            : base(ServerPacketHeader.RoomInviteMessageComposer)
        {
			
        }
    }
}
