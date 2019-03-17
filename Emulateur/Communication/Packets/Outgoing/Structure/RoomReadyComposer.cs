namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class RoomReadyComposer : ServerPacket
    {
        public RoomReadyComposer(int RoomId, string Model)
            : base(ServerPacketHeader.RoomReadyMessageComposer)
        {
            base.WriteString(Model);
            base.WriteInteger(RoomId);
        }
    }
}
