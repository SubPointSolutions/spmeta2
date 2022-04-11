using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Exceptions;

namespace SPMeta2.Regression.SSOM.Extensions
{
    internal static class SPGroupExtensions
    {
        public static string GetOwnerLogin(this SPGroup group)
        {
            if (group.Owner is SPGroup)
                return (group.Owner as SPGroup).LoginName;

            if (group.Owner is SPUser)
                return (group.Owner as SPUser).LoginName;

            throw new SPMeta2Exception(string.Format("Cannot get LoginName for object:[{0}] of type:[{1}]",
                group, group.Owner != null ? group.Owner.GetType().ToString() : "NULL"));
        }

        public static string GetDefaultUserLoginName(this SPGroup group)
        {
            return group.Users[group.Users.Count - 1].LoginName;
        }

        public static SPGroup GetAssociatedVisitorGroup(this SPGroup group)
        {
            return group.ParentWeb.AssociatedVisitorGroup;
        }

        public static SPGroup GetAssociatedOwnerGroup(this SPGroup group)
        {
            return group.ParentWeb.AssociatedOwnerGroup;
        }

        public static SPGroup GetAssociatedMemberGroup(this SPGroup group)
        {
            return group.ParentWeb.AssociatedMemberGroup;
        }
    }
}
