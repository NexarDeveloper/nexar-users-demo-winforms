using System;
using System.Windows.Forms;

namespace Nexar.Users
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
                Config.ApiEndpoint = args[0];

            Application.EnableVisualStyles();
            Application.Run(new Form_Management());
        }
    }
}
