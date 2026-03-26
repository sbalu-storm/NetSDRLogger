using ExtremelySimpleLogger;

namespace NetSdrLoggerConsole.Models
{
    public class LoggerFactory 
    {
        private static Logger _logger;

        public static Logger GetLogger()
        {
            if (_logger == null)
            {
                _logger = CreateLogger();
            }
            return _logger;
        }

        private static Logger CreateLogger()
        {
            return new Logger()
            {
                Name = "My Logger",
                Sinks = {
                    new FileSink("Log.txt", append: true),
                    //new ConsoleSink()
                }
            };
        }
        
    }
}
