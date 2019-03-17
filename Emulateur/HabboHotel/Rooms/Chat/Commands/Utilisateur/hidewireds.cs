using Butterfly.Database.Interfaces;
using Butterfly.HabboHotel.GameClients;

namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd{    class hidewireds : IChatCommand    {        public void Execute(GameClient Session, Room Room, RoomUser UserRoom, string[] Params)        {            Room currentRoom = Session.GetHabbo().CurrentRoom;            if (currentRoom == null)                return;            currentRoom.RoomData.HideWireds = !currentRoom.RoomData.HideWireds;            using (IQueryAdapter queryreactor = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())            {                queryreactor.RunQuery("UPDATE rooms SET allow_hidewireds = '" + TextHandling.BooleanToInt(currentRoom.RoomData.HideWireds) + "' WHERE id = " + currentRoom.Id);            }            if (currentRoom.RoomData.HideWireds)            {                UserRoom.SendWhisperChat(ButterflyEnvironment.GetLanguageManager().TryGetValue("cmd.hidewireds.true", Session.Langue));            }            else            {                UserRoom.SendWhisperChat(ButterflyEnvironment.GetLanguageManager().TryGetValue("cmd.hidewireds.false", Session.Langue));            }        }    }}