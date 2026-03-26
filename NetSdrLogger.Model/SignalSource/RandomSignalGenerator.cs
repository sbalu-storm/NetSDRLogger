using NetSdrLogger.Model.Helpers;
using NetSdrLoggerConsole.Models;

namespace NetSdrLogger.Model.SignalSource
{
    public interface ISignalGenerator
    {
        public Signal GenerateSignal();
        public Signal GenerateLinkedSignal(Signal previous);
    }

    internal class RandomSignalGenerator : ISignalGenerator
    {
        //todo time, endian converters
        const uint MHZ = 1000000;
        const uint KHZ = 1000;

        static Signal MinValues = new Signal(0, 100 * KHZ, 5 * KHZ, 5);
        static Signal MaxValues = new Signal(0, 100 * MHZ, 119 * KHZ, 70);

        public Signal GenerateSignal()
        {
            return new Signal
            {
                Timestamp = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds(),
                Frequency = (ulong)(RandomService.NextDouble() * (MaxValues.Frequency - MinValues.Frequency) + MinValues.Frequency),
                Bandwidth = (uint)RandomService.Next((int)MinValues.Bandwidth, (int)MaxValues.Bandwidth),
                SNR = (float)(RandomService.NextDouble() * (MaxValues.SNR - MinValues.SNR) + MinValues.SNR),
            };
        }
        public Signal GenerateLinkedSignal(Signal previous)
        {
            const uint WALK_SNR = 5;
            const float WALK_FREQ_PERCENTAGE = 10; // percentage from Bandwidth

            uint walkFreq = (uint)(previous.Bandwidth / 100 * WALK_FREQ_PERCENTAGE);

            Signal newSignal = previous;
            newSignal.SNR = previous.SNR + (float)(RandomService.NextDouble() * 2 * WALK_SNR - WALK_SNR); // Random walk for SNR
            if (newSignal.SNR < MinValues.SNR)
                newSignal.SNR = MinValues.SNR;
            if (newSignal.SNR > MaxValues.SNR)
                newSignal.SNR = MaxValues.SNR;

            long newFrequency = (long)previous.Frequency + (long)(RandomService.NextDouble() * 2 * walkFreq - walkFreq); // Random walk for SNR
            if (newFrequency < (long)MinValues.Frequency)
                newFrequency = (long)MinValues.Frequency;
            if (newFrequency > (long)MaxValues.Frequency)
                newFrequency = (long)MaxValues.Frequency;
            newSignal.Frequency = (ulong)newFrequency;

            newSignal.Bandwidth = previous.Bandwidth;

            newSignal.Timestamp = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds();

            return newSignal;
        }
    }
}
