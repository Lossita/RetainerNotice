using System.Collections.Generic;

namespace RetainerNotice.Network
{
    internal enum Opcode
    {
        RetainerInformation,
        ContentFinderNotifyPop
    }

    internal static class OpcodeStorage
    {
        public static Dictionary<ushort, Opcode> Global = new Dictionary<ushort, Opcode>
        {
            {0x0380, Opcode.RetainerInformation},
            {0x00C9, Opcode.ContentFinderNotifyPop},

        };
        public static Dictionary<ushort, Opcode> China = new Dictionary<ushort, Opcode>
        {
            {0x028E,Opcode.RetainerInformation},
            {0x03D1, Opcode.ContentFinderNotifyPop},
        };
    }
}