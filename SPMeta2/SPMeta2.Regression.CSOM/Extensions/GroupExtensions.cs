using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;

namespace SPMeta2.Regression.CSOM.Extensions
{
    internal static class GroupExtensions
    {
        public static string GetOwnerLogin(this Group group)
        {
            if (!group.IsPropertyAvailable("Owner"))
            {
                var owner = group.Owner;

                group.Context.Load(owner, g => g.LoginName);
                group.Context.ExecuteQueryWithTrace();

                return owner.LoginName;
            }

            return group.Owner.LoginName;
        }

        public static string GetDefaultUserLoginName(this Group group)
        {
            if (!group.IsPropertyAvailable("Users"))
            {
                var users = group.Users;

                group.Context.Load(users, g => g.Include(gg => gg.LoginName));
                group.Context.ExecuteQueryWithTrace();

                return users[0].LoginName;
            }

            return group.Users[0].LoginName;
        }
    }
}
