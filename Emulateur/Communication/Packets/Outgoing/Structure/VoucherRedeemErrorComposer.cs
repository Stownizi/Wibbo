﻿namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    class VoucherRedeemErrorComposer : ServerPacket
    {
        public VoucherRedeemErrorComposer(int Type)
            : base(ServerPacketHeader.VoucherRedeemErrorMessageComposer)
        {
            base.WriteString(Type.ToString());
        }
    }
}
