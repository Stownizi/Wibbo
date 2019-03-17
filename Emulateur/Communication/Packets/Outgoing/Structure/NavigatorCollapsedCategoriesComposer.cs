namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class NavigatorCollapsedCategoriesComposer : ServerPacket
    {
        public NavigatorCollapsedCategoriesComposer()
            : base(ServerPacketHeader.NavigatorCollapsedCategoriesMessageComposer)
        {
            base.WriteInteger(0);
        }
    }
}
