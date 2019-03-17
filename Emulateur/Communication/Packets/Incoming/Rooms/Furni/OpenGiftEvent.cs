using Butterfly.Communication.Packets.Outgoing.Structure;
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            int PresentId = Packet.PopInt();
            Item Present = Room.GetRoomItemHandler().GetItem(PresentId);
            if (Present == null)
                return;

            if (!Room.CheckRights(Session, true))

            if (Present.GetBaseItem().InteractionType == InteractionType.GIFT)
            {
                DataRow Data = null;
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `base_id`,`extra_data` FROM `user_presents` WHERE `item_id` = @presentId LIMIT 1");
                    dbClient.AddParameter("presentId", Present.Id);
                    Data = dbClient.GetRow();
                }

                if (Data == null)
                {
                    Room.GetRoomItemHandler().RemoveFurniture(null, Present.Id);

                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("DELETE items, items_limited FROM items LEFT JOIN items_limited ON (items_limited.item_id = items.id) WHERE items.id = '" + Present.Id + "'");
                        dbClient.RunQuery("DELETE FROM `user_presents` WHERE `item_id` = '" + Present.Id + "' LIMIT 1");
                    }

                    Session.GetHabbo().GetInventoryComponent().RemoveItem(Present.Id);
                    return;
                }
                
                ItemData BaseItem = null;
                if (!ButterflyEnvironment.GetGame().GetItemManager().GetItem(Convert.ToInt32(Data["base_id"]), out BaseItem))
                {
                    Room.GetRoomItemHandler().RemoveFurniture(null, Present.Id);

                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("DELETE items, items_limited FROM items LEFT JOIN items_limited ON (items_limited.item_id = items.id) WHERE items.id = '" + Present.Id + "'");
                        dbClient.RunQuery("DELETE FROM `user_presents` WHERE `item_id` = '" + Present.Id + "' LIMIT 1");
                    }

                    Session.GetHabbo().GetInventoryComponent().RemoveItem(Present.Id);
                    return;
                }
                
                FinishOpenGift(Session, BaseItem, Present, Room, Data);
            }
            else if (Present.GetBaseItem().InteractionType == InteractionType.EXTRABOX)
            {
                ItemExtrabox.OpenExtrabox(Session, Present, Room);
            }
            {
                ItemExtrabox.OpenLegendBox(Session, Present, Room);
            }
            else if(Present.GetBaseItem().InteractionType == InteractionType.BADGEBOX)
            {
                ItemExtrabox.OpenBadgeBox(Session, Present, Room);
            }
        {
            bool ItemIsInRoom = true;

            Room.GetRoomItemHandler().RemoveFurniture(Session, Present.Id);

            using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `items` SET `base_item` = @BaseItem, `extra_data` = @edata WHERE `id` = @itemId LIMIT 1");
                dbClient.AddParameter("itemId", Present.Id);
                dbClient.AddParameter("BaseItem", Row["base_id"]);
                dbClient.AddParameter("edata", Row["extra_data"]);
                dbClient.RunQuery();

                dbClient.RunQuery("DELETE FROM `user_presents` WHERE `item_id` = " + Present.Id + " LIMIT 1");
            }

            Present.BaseItem = Convert.ToInt32(Row["base_id"]);
            Present.ResetBaseItem();
            Present.ExtraData = (!string.IsNullOrEmpty(Convert.ToString(Row["extra_data"])) ? Convert.ToString(Row["extra_data"]) : "");


            if (Present.Data.Type == 's')
            {
                if (!Room.GetRoomItemHandler().SetFloorItem(Session, Present, Present.GetX, Present.GetY, Present.Rotation, true, false, true))
                {
                    using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("UPDATE `items` SET `room_id` = '0' WHERE `id` = @itemId LIMIT 1");
                        dbClient.AddParameter("itemId", Present.Id);
                        dbClient.RunQuery();
                    }

                    Session.GetHabbo().GetInventoryComponent().TryAddItem(Present);

                    ItemIsInRoom = false;
                }
            }
            else
            {
                using (IQueryAdapter dbClient = ButterflyEnvironment.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("UPDATE `items` SET `room_id` = '0' WHERE `id` = @itemId LIMIT 1");
                    dbClient.AddParameter("itemId", Present.Id);
                    dbClient.RunQuery();
                }

                Session.GetHabbo().GetInventoryComponent().TryAddItem(Present);

                ItemIsInRoom = false;
            }

            Session.SendPacket(new OpenGiftComposer(Present.Data, Present.ExtraData, Present, ItemIsInRoom));
        }