using Butterfly.HabboHotel.Rooms;

namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class OpenBotActionComposer : ServerPacket
    {
        public OpenBotActionComposer(RoomUser BotUser, int ActionId, string BotSpeech)
            : base(ServerPacketHeader.OpenBotActionMessageComposer)
        {
            base.WriteInteger(BotUser.BotData.Id);
            base.WriteInteger(ActionId);
            if (ActionId == 2)
                base.WriteString(BotSpeech);
            else if (ActionId == 5)
                base.WriteString(BotUser.BotData.Name);
        }
    }
}