using Nexar.Client;
using Nexar.Client.Login;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nexar.Users
{
    static class App
    {
        public static NexarClient Client { get; private set; }
        public static string AccessToken { get; private set; }
        public static IReadOnlyList<IMyWorkspace> Workspaces { get; private set; }

        /// <summary>
        /// Starts the browser sign in page and gets the login data.
        /// </summary>
        public static async Task LoginAsync()
        {
            try
            {
                if (Config.Authority.Contains(":"))
                {
                    var clientId = Environment.GetEnvironmentVariable("NEXAR_CLIENT_ID") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_ID'");
                    var clientSecret = Environment.GetEnvironmentVariable("NEXAR_CLIENT_SECRET") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_SECRET'");
                    var login = await LoginHelper.LoginAsync(
                        clientId,
                        clientSecret,
                        new string[] { "user.access", "design.domain" },
                        Config.Authority);

                    AccessToken = login.AccessToken;
                }
                else
                {
                    AccessToken = Config.Authority;
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Gets and caches user workspaces.
        /// </summary>
        /// <returns></returns>
        public static async Task LoadWorkspacesAsync()
        {
            try
            {
                Client = NexarClientFactory.CreateClient(Config.ApiEndpoint, AccessToken);
                var res = await Client.Workspaces.ExecuteAsync();
                res.AssertNoErrors();
                Workspaces = res.Data.DesWorkspaces;
            }
            catch (Exception ex)
            {
                ShowException(ex);
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Shows the exception message box.
        /// </summary>
        public static void ShowException(Exception ex)
        {
            if (ex is AggregateException aggr && aggr.InnerExceptions.Count == 1)
                ex = aggr.InnerExceptions[0];

            var message = $"{ex.Message}\n\n{ex}";
            MessageBox.Show(message, Config.MyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }

        /// <summary>
        /// Shows the Yes/No dialog.
        /// </summary>
        public static bool Ask(string message)
        {
            var res = MessageBox.Show(message, Config.MyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            return res == DialogResult.Yes;
        }
    }
}
