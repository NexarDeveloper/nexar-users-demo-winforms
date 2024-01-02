using Nexar.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nexar.Users
{
    public partial class Form_Management : Form
    {
        IMyWorkspace _workspace;
        bool _ignoreCheckboxEvents;

        public Form_Management()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Fetches groups and populates the list.
        /// </summary>
        private void RefreshGroupList()
        {
            using (new WaitCursor())
            {
                listViewGroup.BeginUpdate();
                listViewGroup.Items.Clear();
                try
                {
                    var groups = Task.Run(async () =>
                    {
                        var res = await App.Client.Groups.ExecuteAsync(_workspace.Url);
                        res.AssertNoErrors();
                        return res.Data.DesTeam.Groups.OrderBy(x => x.Name);
                    }).Result;

                    foreach (var group in groups)
                    {
                        var item = new ListViewItem(group.Name, 0)
                        {
                            Name = group.Name,
                            Tag = group.Id
                        };
                        listViewGroup.Items.Add(item);
                    }
                }
                finally
                {
                    listViewGroup.EndUpdate();
                }
            }
        }

        /// <summary>
        /// Fetches users and populates the list.
        /// </summary>
        private void RefreshUserList(string groupId)
        {
            using (new WaitCursor())
            {
                listViewUser.BeginUpdate();
                listViewUser.Items.Clear();
                _ignoreCheckboxEvents = true;
                try
                {
                    var users = Task.Run(async () =>
                    {
                        var res = await App.Client.Users.ExecuteAsync(_workspace.Url);
                        res.AssertNoErrors();
                        return res.Data.DesTeam.Users.OrderBy(x => x.Email);
                    }).Result;

                    listViewUser.CheckBoxes = groupId != null;

                    foreach (var user in users)
                    {
                        if (user.UserName == null)
                            continue;

                        var tag = new UserInfo(user);
                        var item = NewUserListItem(tag);
                        if (groupId != null)
                            item.Checked = tag.Groups.Exists(x => x.Id == groupId);

                        listViewUser.Items.Add(item);
                    }
                }
                finally
                {
                    listViewUser.EndUpdate();
                    _ignoreCheckboxEvents = false;
                }
            }
        }

        /// <summary>
        /// Creates a new user list item.
        /// </summary>
        private static ListViewItem NewUserListItem(UserInfo user)
        {
            var roles = user.Groups
                .OrderBy(x => x.Name)
                .Select(x => x.Name);

            var items = new string[5]
            {
                user.UserName,
                user.FirstName,
                user.LastName,
                user.Email,
                string.Join(", ", roles),
            };

            return new ListViewItem(items, 1)
            {
                Name = user.UserName,
                Tag = user
            };
        }

        /// <summary>
        /// Login, fetch workspaces, populate the workspace list.
        /// </summary>
        private void Form_Management_Load(object sender, EventArgs e)
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
                    var defaultWorkspaceIndex = -1;

                    // start loading workspaces
                    Task.Run(async () =>
                    {
                        // get data
                        await App.LoadWorkspacesAsync();

                        // show data
                        Invoke((MethodInvoker)(() =>
                        {
                            // populate workspaces
                            comboWorkspaces.BeginUpdate();
                            try
                            {
                                var index = -1;
                                foreach (var workspace in App.Workspaces)
                                {
                                    ++index;
                                    comboWorkspaces.Items.Add(workspace.Name);
                                    if (workspace.IsDefault)
                                        defaultWorkspaceIndex = index;
                                }
                            }
                            finally
                            {
                                comboWorkspaces.Enabled = true;
                                comboWorkspaces.EndUpdate();
                            }

                            // select the default workspace
                            if (defaultWorkspaceIndex >= 0)
                            {
                                comboWorkspaces.SelectedIndex = defaultWorkspaceIndex;
                                comboWorkspaces_SelectionChangeCommitted(null, null);
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

        /// <summary>
        /// Sets the selected workspace current and refreshes lists.
        /// </summary>
        private void comboWorkspaces_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _workspace = App.Workspaces[comboWorkspaces.SelectedIndex];
            RefreshGroupList();
            RefreshUserList(null);
        }

        /// <summary>
        /// Adds "New Group".
        /// </summary>
        private void addGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int newGroupNumber = 1;
            var newGroupName = "New Group";
            while (listViewGroup.Items.ContainsKey(newGroupName))
            {
                ++newGroupNumber;
                newGroupName = $"New Group {newGroupNumber}";
            }

            try
            {
                using (new WaitCursor())
                {
                    var newGroup = Task.Run(async () =>
                    {
                        var res = await App.Client.CreateGroup.ExecuteAsync(_workspace.Url, newGroupName);
                        res.AssertNoErrors();

                        return new Groups_DesTeam_Groups_DesUserGroup(res.Data.DesCreateUserGroup.Id, newGroupName);
                    }).Result;

                    var item = new ListViewItem(newGroupName, 0)
                    {
                        Name = newGroup.Name,
                        Tag = newGroup.Id
                    };

                    listViewGroup.Items.Add(item);
                    item.Selected = true;
                }
            }
            catch (Exception ex)
            {
                App.ShowException(ex);
            }
        }

        /// <summary>
        /// Gets the selected group ID or null.
        /// </summary>
        private string GetSelectedGroupId()
        {
            return listViewGroup.SelectedItems.Count == 1 ? (string)listViewGroup.SelectedItems[0].Tag : null;
        }

        /// <summary>
        /// Deletes the selected group.
        /// </summary>
        private void deleteGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var groupId = GetSelectedGroupId();
            if (groupId is null)
                return;

            if (!App.Ask("Delete group?"))
                return;

            try
            {
                using (new WaitCursor())
                {
                    Task.Run(async () =>
                    {
                        var res = await App.Client.DeleteGroup.ExecuteAsync(groupId);
                        res.AssertNoErrors();
                    }).Wait();

                    RefreshGroupList();
                    RefreshUserList(null);
                }
            }
            catch (Exception ex)
            {
                App.ShowException(ex);
            }
        }

        /// <summary>
        /// Renames the group with its edited label.
        /// </summary>
        private void listViewGroup_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            var groupId = GetSelectedGroupId();
            if (groupId is null)
                return;

            try
            {
                using (new WaitCursor())
                {
                    Task.Run(async () =>
                    {
                        var res = await App.Client.RenameGroup.ExecuteAsync(groupId, e.Label);
                        res.AssertNoErrors();
                    }).Wait();

                    listViewGroup.SelectedItems[0].Name = e.Label;
                }

                RefreshUserList(groupId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                e.CancelEdit = true;
            }
        }

        /// <summary>
        /// Updates the user list checkboxes for the selected group.
        /// </summary>
        private void listViewGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            var groupId = GetSelectedGroupId();

            listViewUser.BeginUpdate();
            _ignoreCheckboxEvents = true;

            if (groupId is null)
            {
                listViewUser.CheckBoxes = false;
            }
            else
            {
                listViewUser.CheckBoxes = true;
                foreach (ListViewItem item in listViewUser.Items)
                {
                    var user = (UserInfo)item.Tag;
                    item.Checked = user.Groups.Exists(x => x.Id == groupId);
                }
            }

            listViewUser.EndUpdate();
            _ignoreCheckboxEvents = false;
        }

        /// <summary>
        /// Adds a new user.
        /// </summary>
        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var user = new UserInfo();
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
                        res.AssertNoErrors();
                    }).Wait();

                    var groupId = GetSelectedGroupId();
                    RefreshUserList(groupId);
                }
            }
            catch (Exception ex)
            {
                App.ShowException(ex);
            }
        }

        /// <summary>
        /// Updates the selected user details.
        /// </summary>
        private void updateUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewUser.SelectedItems.Count != 1)
                return;

            var user1 = (UserInfo)listViewUser.SelectedItems[0].Tag;

            var user2 = new UserInfo
            {
                UserName = user1.UserName,
                FirstName = user1.FirstName,
                LastName = user1.LastName,
            };

            var userForm = new Form_User(user2, false);
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
                        res.AssertNoErrors();
                    }).Wait();

                    var groupId = GetSelectedGroupId();
                    RefreshUserList(groupId);
                }
            }
            catch (Exception ex)
            {
                App.ShowException(ex);
            }
        }

        /// <summary>
        /// Deletes the selected user.
        /// </summary>
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
                    var user = (UserInfo)item.Tag;

                    Task.Run(async () =>
                    {
                        // delete user
                        var input = new DesDeleteUserInput
                        {
                            WorkspaceUrl = _workspace.Url,
                            UserId = user.UserId,
                        };
                        var res = await App.Client.DeleteUser.ExecuteAsync(input);
                        res.AssertNoErrors();
                    }).Wait();

                    var groupId = GetSelectedGroupId();
                    RefreshUserList(groupId);
                }
            }
            catch (Exception ex)
            {
                App.ShowException(ex);
            }
        }

        /// <summary>
        /// Adds or removes the user to the selected group.
        /// </summary>
        private void listViewUser_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_ignoreCheckboxEvents)
                return;

            var groupId = GetSelectedGroupId();
            var user = (UserInfo)listViewUser.Items[e.Index].Tag;

            using (new WaitCursor())
            {
                if (e.NewValue == CheckState.Checked)
                {
                    try
                    {
                        Task.Run(async () =>
                        {
                            // add user to group
                            var res = await App.Client.AddUserToGroup.ExecuteAsync(groupId, user.UserId);
                            res.AssertNoErrors();
                        }).Wait();

                        _ignoreCheckboxEvents = true;
                        var groupName = listViewGroup.SelectedItems[0].Name;
                        user.Groups.Add(new GroupInfo { Id = groupId, Name = groupName });
                        var item = NewUserListItem(user);
                        item.Checked = true;
                        listViewUser.Items[e.Index] = item;
                        _ignoreCheckboxEvents = false;
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
                        Task.Run(async () =>
                        {
                            // remove user from group
                            var res = await App.Client.RemoveUserFromGroup.ExecuteAsync(groupId, user.UserId);
                            res.AssertNoErrors();
                        }).Wait();

                        user.Groups.RemoveAll(x => x.Id == groupId);
                        var item = NewUserListItem(user);
                        listViewUser.Items[e.Index] = item;
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
}
