namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class OnGuideSessionStarted : ServerPacket
    {
        public OnGuideSessionStarted()
            : base(ServerPacketHeader.OnGuideSessionStarted)
        {
			
        }
    }
}
