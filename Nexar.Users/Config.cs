using System;

namespace Nexar.Users
{
    /// <summary>
    /// App configuration.
    /// </summary>
    static class Config
    {
        public const string MyTitle = "Nexar.Users";

        public static NexarMode NexarMode { get; }
        public static string Authority { get; }
        public static string ApiEndpoint { get; set; }

        static Config()
        {
            var mode = Environment.GetEnvironmentVariable("NEXAR_MODE") ?? "Prod";
            NexarMode = (NexarMode)Enum.Parse(typeof(NexarMode), mode, true);

            switch (NexarMode)
            {
                case NexarMode.Prod:
                    Authority = "https://identity.nexar.com/";
                    ApiEndpoint = "https://api.nexar.com/graphql/";
                    break;
                default:
                    throw new Exception();
            }
        }
    }

    public enum NexarMode
    {
        Prod,
    }
}
