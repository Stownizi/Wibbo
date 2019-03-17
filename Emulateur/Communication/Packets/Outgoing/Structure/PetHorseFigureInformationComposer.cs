using Butterfly.HabboHotel.Rooms;

namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class PetHorseFigureInformationComposer : ServerPacket
    {
        public PetHorseFigureInformationComposer(RoomUser PetUser)
            : base(ServerPacketHeader.PetHorseFigureInformationMessageComposer)
        {
            base.WriteInteger(PetUser.PetData.VirtualId);
            base.WriteInteger(PetUser.PetData.PetId);
            base.WriteInteger(PetUser.PetData.Type);
            base.WriteInteger(int.Parse(PetUser.PetData.Race));
            base.WriteString(PetUser.PetData.Color.ToLower());
            base.WriteInteger(1);
            if (PetUser.PetData.Saddle > 0)
            {
                base.WriteInteger(3); //Count

                base.WriteInteger(2);
                base.WriteInteger(PetUser.PetData.PetHair);
                base.WriteInteger(PetUser.PetData.HairDye);

                base.WriteInteger(3);
                base.WriteInteger(PetUser.PetData.PetHair);
                base.WriteInteger(PetUser.PetData.HairDye);

                base.WriteInteger(4);
                base.WriteInteger(PetUser.PetData.Saddle);
                base.WriteInteger(0);
            }
            else
            {

                base.WriteInteger(2); //Count

                base.WriteInteger(2);
                base.WriteInteger(PetUser.PetData.PetHair);
                base.WriteInteger(PetUser.PetData.HairDye);

                base.WriteInteger(3);
                base.WriteInteger(PetUser.PetData.PetHair);
                base.WriteInteger(PetUser.PetData.HairDye);
            }
            base.WriteBoolean(PetUser.PetData.Saddle > 0);
            base.WriteBoolean(PetUser.RidingHorse);
        }
    }
}
