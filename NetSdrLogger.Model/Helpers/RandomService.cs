namespace NetSdrLogger.Model.Helpers
{
    internal sealed class RandomService
    {
        private static readonly Random _random = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
        public static double NextDouble()
        {
            lock (_random)
            {
                return _random.NextDouble();
            }
        }
        public static int Next(int minValue, int maxValue)
        {
            lock (_random)
            {
                return _random.Next(minValue, maxValue);
            }
        }
    }
}
