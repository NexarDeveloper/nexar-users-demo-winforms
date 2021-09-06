using Nexar.Client;
using System.Collections.Generic;
using System.Linq;

namespace Nexar.Users
{
    /// <summary>
    /// User group info.
    /// </summary>
    /// <remarks>
    /// It is used for Strawberry Shake result copies and for our own instances on adding users to groups.
    /// </remarks>
    class GroupInfo
    {
        public string Id;
        public string Name;
    }

    /// <summary>
    /// User info.
    /// </summary>
    /// <remarks>
    /// It is used for Strawberry Shake result copies and by the user details dialog.
    /// Also, we update the group list on adding or removing users to groups.
    /// </remarks>
    class UserInfo
    {
        public string UserId;
        public string UserName;
        public string Email;
        public string FirstName;
        public string LastName;
        public List<GroupInfo> Groups;

        public string Password;

        public UserInfo()
        {
        }

        public UserInfo(IMyUser user)
        {
            UserId = user.UserId;
            UserName = user.UserName;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Groups = user.Groups.Select(x => new GroupInfo { Id = x.Id, Name = x.Name }).ToList();
        }
    }
}
