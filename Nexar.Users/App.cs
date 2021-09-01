using Microsoft.Extensions.DependencyInjection;
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
        public static IReadOnlyList<IMyWorkspace> Workspaces { get; private set; }
        public static string Username => Login.Username;

        public static LoginInfo Login { get; private set; }
        /// <summary>
        /// Run this as a task after the window is shown.
        /// </summary>
        /// <remarks>
        /// Why not before as it used to be. If a user does not login and maybe closes the login page,
        /// then the application is running waiting for the login. The user may not know about it.
        /// So, with the window shown the user is aware of it and may close.
        /// </remarks>
        public static async Task LoginAsync()
        {
            try
            {
                var clientId = Environment.GetEnvironmentVariable("NEXAR_CLIENT_ID") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_ID'");
                var clientSecret = Environment.GetEnvironmentVariable("NEXAR_CLIENT_SECRET") ?? throw new InvalidOperationException("Please set environment 'NEXAR_CLIENT_SECRET'");
                Login = await LoginHelper.LoginAsync(
                    clientId,
                    clientSecret,
                    new string[] { "user.access", "design.domain" },
                    Config.Authority);
            }
            catch (Exception ex)
            {
                ShowException(ex);
                Environment.Exit(1);
            }
        }

        public static async Task LoadWorkspacesAsync()
        {
            try
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection
                    .AddNexarClient()
                    .ConfigureHttpClient(c =>
                    {
                        c.BaseAddress = new Uri(Config.ApiEndpoint);
                        c.DefaultRequestHeaders.Add("token", Login.AccessToken);
                    })
                ;
                var services = serviceCollection.BuildServiceProvider();

                Client = services.GetRequiredService<NexarClient>();
                var res = await Client.Workspaces.ExecuteAsync();
                ClientHelper.EnsureNoErrors(res);
                Workspaces = res.Data.DesWorkspaces;
            }
            catch (Exception ex)
            {
                ShowException(ex);
                Environment.Exit(1);
            }
        }

        public static void ShowException(Exception ex)
        {
            if (ex is AggregateException aggr && aggr.InnerExceptions.Count == 1)
                ex = aggr.InnerExceptions[0];

            var message = $"{ex.Message}\n\n{ex}";
            MessageBox.Show(message, Config.MyTitle, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }

        public static bool Ask(string message)
        {
            var res = MessageBox.Show(message, Config.MyTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            return res == DialogResult.Yes;
        }
    }
}
