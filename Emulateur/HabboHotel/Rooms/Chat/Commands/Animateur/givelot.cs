using Butterfly.Communication.Packets.Outgoing.Structure;
using Butterfly.Database.Interfaces;
using Butterfly.HabboHotel.GameClients;
using System.Collections.Generic;

namespace Butterfly.HabboHotel.Rooms.Chat.Commands.Cmd
            if (!ButterflyEnvironment.GetGame().GetItemManager().GetItem(12018410, out ItemData))
                return;
            
            int NbBadge = ButterflyEnvironment.GetRandomNumber(1, 2);

            ItemData ItemDataBadge = null;
            if (!ButterflyEnvironment.GetGame().GetItemManager().GetItem(91947063, out ItemDataBadge))
                return;

            List<Item> Items = ItemFactory.CreateMultipleItems(ItemData, roomUserByHabbo.GetClient().GetHabbo(), "", NbLot);
            {
                if (roomUserByHabbo.GetClient().GetHabbo().GetInventoryComponent().TryAddItem(PurchasedItem))
                {
                    roomUserByHabbo.GetClient().SendPacket(new FurniListNotificationComposer(PurchasedItem.Id, 1));
                }
            }