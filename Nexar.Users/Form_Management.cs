using Nexar.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nexar.Users
{
    public partial class Form_Management : Form
    {
        IMyWorkspace _workspace;
        IReadOnlyList<IGroups_DesTeam_Groups> _groups;
        IReadOnlyList<IUsers_DesTeam_Users> _users;
        bool _ignoreCheckboxEvents;

        public Form_Management()
        {
            InitializeComponent();
        }

        private void RefreshGroupList()
        {
            this.listViewGroup.Items.Clear();
            this.listViewGroup.BeginUpdate();
            try
            {
                foreach (var group in _groups)
                {
                    var item = new ListViewItem(group.Name, 0);
                    this.listViewGroup.Items.Add(item);
                }
            }
            finally
            {
                this.listViewGroup.EndUpdate();
            }
        }

        private void RefreshUserList(string groupName)
        {
            listViewUser.Items.Clear();
            this.listViewUser.BeginUpdate();
            _ignoreCheckboxEvents = true;
            try
            {
                listViewUser.CheckBoxes = !string.IsNullOrEmpty(groupName);

                foreach (var user in _users)
                {
                    if (user.UserName == null)
                        continue;

                    var item = new ListViewItem(new string[5]
                    {
                        user.UserName,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        string.Join(", ", user.Groups.Select(x => x.Name)),
                    }, 1)
                    {
                        Name = user.UserName,
                        Tag = user
                    };

                    if (!string.IsNullOrEmpty(groupName))
                        item.Checked = user.Groups.FirstOrDefault(x => x.Name == groupName) != null;

                    listViewUser.Items.Add(item);
                }
            }
            finally
            {
                this.listViewUser.EndUpdate();
                _ignoreCheckboxEvents = false;
            }
        }

        private void Form_Managment_Load(object sender, EventArgs e)
        {
            // show the endpoint in the title
            Text = $"Login... {Config.ApiEndpoint}";

            // load as a task after the window is shown
            Task.Run(async () =>
            {
                // login
                await App.LoginAsync();

                // load data
                Invoke((MethodInvoker)(() =>
                {
                    // begin
                    Text = $"Loading... {Config.ApiEndpoint}";

                    // start loading workspaces
                    Task.Run(async () =>
                    {
                        // get data
                        await App.LoadWorkspacesAsync();

                        // show data
                        Invoke((MethodInvoker)(() =>
                        {
                            // populate workspaces
                            this.comboWorkspaces.BeginUpdate();
                            try
                            {
                                foreach (var workspace in App.Workspaces)
                                    this.comboWorkspaces.Items.Add(workspace.Name);
                            }
                            finally
                            {
                                this.comboWorkspaces.Enabled = true;
                                this.comboWorkspaces.EndUpdate();
                            }

                            // end
                            Text = $"Nexar.Users Demo - {Config.ApiEndpoint}";
                        }));
                    });

                    // activate, after browser windows this window may be passive
                    Activate();
                    TopMost = true;
                    TopMost = false;
                    Focus();
                }));
            });
        }

        private async Task QueryUsersAsync()
        {
            var res = await App.Client.Users.ExecuteAsync(_workspace.Url);
            ClientHelper.EnsureNoErrors(res);
            _users = res.Data.DesTeam.Users.OrderBy(x => x.Email).ToList();
        }

        private void comboWorkspaces_SelectionChangeCommitted(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                _workspace = App.Workspaces[this.comboWorkspaces.SelectedIndex];

                Task.Run(async () =>
                {
                    // get groups
                    var res = await App.Client.Groups.ExecuteAsync(_workspace.Url);
                    ClientHelper.EnsureNoErrors(res);
                    _groups = res.Data.DesTeam.Groups.OrderBy(x => x.Name).ToList();

                    await QueryUsersAsync();
                }).Wait();

                RefreshGroupList();
                RefreshUserList("");
            }
        }

        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet implemented."); //TODO
        }

        private void deleteGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not yet implemented."); //TODO
        }

        private void listViewGroup_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            MessageBox.Show("Not yet implemented."); //TODO
            e.CancelEdit = true;
        }

        private void listViewGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            var groupName = GetSelectedGroupName();
            RefreshUserList(groupName);
        }

        //TODO add to the current group, if any
        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var user = new Form_User.UserInfo();
            var userForm = new Form_User(user, true);
            if (userForm.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (new WaitCursor())
                {
                    Task.Run(async () =>
                    {
                        // create user
                        var input = new DesCreateUserInput
                        {
                            WorkspaceUrl = _workspace.Url,
                            UserName = user.UserName,
                            Password = user.Password,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.UserName
                        };
                        var res = await App.Client.CreateUser.ExecuteAsync(input);
                        ClientHelper.EnsureNoErrors(res);

                        await QueryUsersAsync();
                    }).Wait();

                    var groupName = GetSelectedGroupName();
                    RefreshUserList(groupName);
                }
            }
            catch (Exception ex)
            {
                App.ShowException(ex);
            }
        }

        private void updateUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewUser.SelectedItems.Count != 1)
                return;

            var user1 = listViewUser.SelectedItems[0].Tag as IMyUser;

            var user2 = new Form_User.UserInfo
            {
                UserName = user1.UserName,
                FirstName = user1.FirstName,
                LastName = user1.LastName,
            };

            Form_User userForm = new Form_User(user2, false);
            if (userForm.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (new WaitCursor())
                {
                    Task.Run(async () =>
                    {
                        // update user
                        var input = new DesUpdateUserInput
                        {
                            WorkspaceUrl = _workspace.Url,
                            UserId = user1.UserId,
                            FirstName = user2.FirstName,
                            LastName = user2.LastName,
                        };
                        var res = await App.Client.UpdateUser.ExecuteAsync(input);
                        ClientHelper.EnsureNoErrors(res);

                        await QueryUsersAsync();
                    }).Wait();

                    var groupName = GetSelectedGroupName();
                    RefreshUserList(groupName);
                }
            }
            catch (Exception ex)
            {
                App.ShowException(ex);
            }
        }

        private void deleteUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewUser.SelectedItems.Count != 1)
                return;

            if (!App.Ask("Delete user?"))
                return;

            try
            {
                using (new WaitCursor())
                {
                    var item = listViewUser.SelectedItems[0];
                    var user = (IMyUser)item.Tag;

                    Task.Run(async () =>
                    {
                        // delete user
                        var input = new DesDeleteUserInput
                        {
                            WorkspaceUrl = _workspace.Url,
                            UserId = user.UserId,
                        };
                        var res = await App.Client.DeleteUser.ExecuteAsync(input);
                        ClientHelper.EnsureNoErrors(res);

                        await QueryUsersAsync();
                    }).Wait();

                    var groupName = GetSelectedGroupName();
                    RefreshUserList(groupName);
                }
            }
            catch (Exception ex)
            {
                App.ShowException(ex);
            }
        }

        private string GetSelectedGroupName()
        {
            if (listViewGroup.SelectedItems.Count != 1)
                return "";

            var item = listViewGroup.SelectedItems[0];
            return item.Text;
        }

        private void listViewUser_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_ignoreCheckboxEvents)
                return;

            if (e.NewValue == CheckState.Checked)
            {
                try
                {
                    MessageBox.Show("Not yet implemented."); //TODO add to group
                    e.NewValue = CheckState.Unchecked;
                }
                catch (Exception ex)
                {
                    App.ShowException(ex);
                    e.NewValue = CheckState.Unchecked;
                }
            }
            else
            {
                try
                {
                    MessageBox.Show("Not yet implemented."); //TODO remove from group
                    e.NewValue = CheckState.Checked;
                }
                catch (Exception ex)
                {
                    App.ShowException(ex);
                    e.NewValue = CheckState.Checked;
                }
            }
        }
    }
}
