using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    public class BuiltInAuditMaskType
    {
        public static string None = "None";
        public static string CheckOut = "CheckOut";
        public static string CheckIn = "CheckIn";
        public static string View = "View";
        public static string Delete = "Delete";
        public static string Update = "Update";
        public static string ProfileChange = "ProfileChange";
        public static string ChildDelete = "ChildDelete";
        public static string SchemaChange = "SchemaChange";
        public static string SecurityChange = "SecurityChange";
        public static string Undelete = "Undelete";
        public static string Workflow = "Workflow";
        public static string Copy = "Copy";
        public static string Move = "Move";
        public static string Search = "Search";
        public static string All = "All";
    }
}
