namespace NetSdrLogger.Model.NetSDR
{
    internal class BitUtils
    {
        public static UInt64 GetLittleEndianUInt64FromQueue(Queue<byte> data)
        {
            byte[] byteVal = new byte[sizeof(UInt64)];
            for (int i = 0; i < byteVal.Length; ++i)
            {
                byteVal[i] = data.Dequeue();
            }
            return BitConverter.ToUInt64(byteVal, 0);
        }

        public static UInt32 GetLittleEndianUInt32FromQueue(Queue<byte> data)
        {
            byte[] byteVal = new byte[sizeof(UInt64)];
            for (int i = 0; i < byteVal.Length; ++i)
            {
                byteVal[i] = data.Dequeue();
            }
            return BitConverter.ToUInt32(byteVal, 0);
        }

        public static float GetLittleEndianFloatFromQueue(Queue<byte> data)
        {
            byte[] byteVal = new byte[sizeof(UInt64)];
            for (int i = 0; i < byteVal.Length; ++i)
            {
                byteVal[i] = data.Dequeue();
            }
            return BitConverter.ToSingle(byteVal, 0);
        }
    }
}
