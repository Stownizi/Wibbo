namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class SetUniqueIdComposer : ServerPacket
    {
        public SetUniqueIdComposer(string Id)
            : base(ServerPacketHeader.SetUniqueIdMessageComposer)
        {
            base.WriteString(Id);
        }
    }
}
