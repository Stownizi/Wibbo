namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class TradingCompleteMessageComposer : ServerPacket
    {
        public TradingCompleteMessageComposer()
            : base(ServerPacketHeader.TradingCompleteMessageComposer)
        {
			
        }
    }
}
