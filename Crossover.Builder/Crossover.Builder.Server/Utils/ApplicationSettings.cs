using System.Configuration;
using System.Globalization;

namespace Crossover.Builder.Server.Utils
{
    public static class ApplicationSettings
    {
        public const string HttpPortKey = "http-port";
        public const string HttpsPortKey = "https-port";

        static ApplicationSettings()
        {
            int httpPort;
            if (!int.TryParse(ConfigurationManager.AppSettings[HttpPortKey],
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out httpPort)) httpPort = 9080;
            HttpPort = httpPort;

            int httpsPort;
            if (!int.TryParse(ConfigurationManager.AppSettings[HttpsPortKey],
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out httpsPort)) httpsPort = 9443;
            HttpsPort = httpsPort;
        }

        public static int HttpPort { get; private set; }
        public static int HttpsPort { get; private set; }
    }
}