namespace NetSdrLogger.Model.NetSDR
{
    internal struct HardwareHeader
    {
        public const int LENGTH = 2;
        public HardwareHeader() { }
        public static HardwareHeader CreateFromQueue(Queue<byte> data)
        {
            if (data.Count < LENGTH)
            {
                throw new FormatException();
            }

            HardwareHeader hardwareHeader = new();

            hardwareHeader.lsb = data.Dequeue();
            byte hiByte = data.Dequeue();

            // Example: 100 01010
            // 3-bit: 100 (4)
            // 5-bit: 01010 (10)

            // Extract upper 3 bits
            hardwareHeader.messagetype = (byte)((hiByte >> 5) & 0x07);

            // Extract lower 5 bits
            hardwareHeader.msb = (byte)(hiByte & 0x1F);

            return hardwareHeader;
        }

        public int GetLengthValue()
        {
            return lsb + ((int)msb) << 8;
        }

        public TargetMessageType GetMessageFromDeviceToPC()
        {
            return (TargetMessageType)messagetype;
        }

        public byte lsb;
        public byte messagetype;
        public byte msb;
    }
}
