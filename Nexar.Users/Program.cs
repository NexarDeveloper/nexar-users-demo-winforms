using System;
using System.Windows.Forms;

namespace Nexar.Users
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new Form_Management());
        }
    }
}
