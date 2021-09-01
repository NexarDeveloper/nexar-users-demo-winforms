using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Nexar.Users
{
    public partial class Form_User : Form
    {
        public class UserInfo
        {
            public string UserName;
            public string Password;
            public string FirstName;
            public string LastName;
        }

        public UserInfo User { get; }

        private readonly bool _addUser;

        public Form_User(UserInfo user, bool addUser)
        {
            InitializeComponent();

            User = user;
            _addUser = addUser;
        }

        private void Form_User_Load(object sender, EventArgs e)
        {
            if (_addUser)
            {
                buttonAddUpdate.Text = "Add";
                textBoxUserName.ReadOnly = false;
                labelPassword.Visible = true;
                textBoxPassword.Visible = true;
            }
            else
            {
                textBoxUserName.Text = User.UserName;
                textBoxFirstName.Text = User.FirstName;
                textBoxLastName.Text = User.LastName;
            }
        }

        private string CalculateMD5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private void buttonAddUpdate_Click(object sender, EventArgs e)
        {
            if (_addUser)
            {
                User.UserName = textBoxUserName.Text;
                User.Password = CalculateMD5Hash(textBoxPassword.Text);
            }

            User.FirstName = textBoxFirstName.Text;
            User.LastName = textBoxLastName.Text;
        }
    }
}
