using Butterfly.Communication.Packets.Outgoing.Structure;
using Butterfly.HabboHotel.GameClients;
                    int.TryParse(Params[2], out raceid);
            RoomClient.SendPacket(new UsersComposer(roomUserByHabbo));