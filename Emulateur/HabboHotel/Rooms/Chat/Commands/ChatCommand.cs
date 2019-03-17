﻿using Butterfly.HabboHotel.GameClients;

namespace Butterfly.HabboHotel.Rooms.Chat
{
    public struct ChatCommand
    {
        public readonly int commandID;
        public readonly string input;
        public readonly int minrank;
        public readonly string descriptionFr;
        public readonly string descriptionEn;
        public readonly string descriptionBr;

        public ChatCommand(int pCommandID, string pInput, int pRank, string pDescriptionFr, string pDescriptionEn, string pdescriptionBr)
        {
            this.commandID = pCommandID;
            this.input = pInput;
            this.minrank = pRank;
            this.descriptionFr = pDescriptionFr;
            this.descriptionEn = pDescriptionEn;
            this.descriptionBr = pdescriptionBr;
        }

        public bool UserGotAuthorization(GameClient session)
        {
            if (this.minrank == 0)
                return true;
            if (this.minrank > 0)
            {
                if ((long)this.minrank <= (long)session.GetHabbo().Rank)
                    return true;
            }
            else if (this.minrank < 0)
            {
                if (this.minrank == -1)
                {
                    if (session.GetHabbo().CurrentRoom.CheckRights(session))
                        return true;
                }
                else if (this.minrank == -2 && session.GetHabbo().CurrentRoom.CheckRights(session, true))
                    return true;
            }
            return false;
        }

        public bool UserGotAuthorizationStaffLog(GameClient session)
        {
            if (this.minrank > 4)
                return true;

            return false;
        }

        public int UserGotAuthorization2(GameClient session)
        {
            if (this.minrank == 0)
                return 0;
            if (this.minrank > 0)
            {
                if ((long)this.minrank <= (long)session.GetHabbo().Rank)
                    return 0;
                else if (this.minrank == 2)
                    return 2;
            }
            else if (this.minrank < 0)
            {
                if (this.minrank == -1)
                {
                    if (session.GetHabbo().CurrentRoom.CheckRights(session))
                        return 0;
                    else
                        return 3;
                }
                else if (this.minrank == -2 && session.GetHabbo().CurrentRoom.CheckRights(session, true))
                    return 0;
                else
                    return 4;
            }
            return 1;
        }
    }
}