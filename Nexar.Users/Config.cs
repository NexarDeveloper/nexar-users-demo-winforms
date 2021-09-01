using System;

namespace Nexar.Users
{
    /// <summary>
    /// App configuration. Different modes and endpoints are used for internal development.
    /// Clients usually use nexar.com endpoints.
    /// </summary>
    static class Config
    {
        public const string MyTitle = "Nexar.Users";

        public static string Authority { get; }
        public static string ApiEndpoint { get; set; }

        static Config()
        {
            var modeStr = Environment.GetEnvironmentVariable("NEXAR_MODE") ?? "Prod";
            var mode = (Mode)Enum.Parse(typeof(Mode), modeStr, true);

            switch (mode)
            {
                case Mode.Dev:
                    Authority = "https://identity.nexar.com/";
                    ApiEndpoint = "https://api.nexar.com/graphql/";
                    break;
                case Mode.Prod:
                    Authority = "https://identity.nexar.com/";
                    ApiEndpoint = "https://api.nexar.com/graphql/";
                    break;
                default:
                    throw new Exception();
            }
        }

        public enum Mode
        {
            Prod,
            Dev
        }
    }
}
