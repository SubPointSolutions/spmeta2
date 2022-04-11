using SPMeta2.Definitions;

namespace SPMeta2.BuiltInDefinitions
{
    /// <summary>
    /// Out of the box SharePoint list and libraries.
    /// </summary>
    public static class BuiltInListViewDefinitions
    {
        public static class Libraries
        {
            public static ListViewDefinition MySubmissions = new ListViewDefinition
            {
                Title = "My submissions",
                Url = "my-sub.aspx"
            };

            public static ListViewDefinition ApproveRejectItems = new ListViewDefinition
            {
                Title = "Approve/reject Items",
                Url = "mod-view.aspx"
            };

            public static ListViewDefinition AllItems = new ListViewDefinition
            {
                Title = "All Items",
                Url = "AllItems.aspx"
            };

            public static ListViewDefinition AllDocuments = new ListViewDefinition
            {
                Title = "All Documents",
                Url = "AllItems.aspx"
            };

            public static ListViewDefinition Combine = new ListViewDefinition
            {
                Title = "Combine",
                Url = "Combine.aspx"
            };

            public static ListViewDefinition DispForm = new ListViewDefinition
            {
                Title = "DispForm",
                Url = "DispForm.aspx"
            };


            public static ListViewDefinition EditForm = new ListViewDefinition
            {
                Title = "EditForm",
                Url = "EditForm.aspx"
            };

            public static ListViewDefinition Repair = new ListViewDefinition
            {
                Title = "repair",
                Url = "repair.aspx"
            };

            public static ListViewDefinition Thumbnails = new ListViewDefinition
            {
                Title = "Thumbnails",
                Url = "Thumbnails.aspx"
            };

            public static ListViewDefinition Upload = new ListViewDefinition
            {
                Title = "Upload",
                Url = "Upload.aspx"
            };
        }

        public static class Lists
        {
            public static ListViewDefinition AllItems = new ListViewDefinition
            {
                Title = "All Items",
                Url = "AllItems.aspx"
            };

            public static ListViewDefinition DispForm = new ListViewDefinition
            {
                Title = "DispForm",
                Url = "DispForm.aspx"
            };


            public static ListViewDefinition EditForm = new ListViewDefinition
            {
                Title = "EditForm",
                Url = "EditForm.aspx"
            };

            public static ListViewDefinition NewForm = new ListViewDefinition
            {
                Title = "NewForm",
                Url = "NewForm.aspx"
            };
        }

        public static class Events
        {
            public static ListViewDefinition AllEvents = new ListViewDefinition
            {
                Title = "All Events",
                Url = "AllItems.aspx"
            };

            public static ListViewDefinition Calendar = new ListViewDefinition
            {
                Title = "Calendar",
                Url = "calendar.aspx"
            };

            public static ListViewDefinition CurrentEvents = new ListViewDefinition
            {
                Title = "Current Events",
                Url = "MyItems.aspx"
            };
        }

        public static class Assets
        {
            public static ListViewDefinition AllAssets = new ListViewDefinition
            {
                Title = "All Assets",
                Url = "AllItems.aspx"
            };

            public static ListViewDefinition ApproveRejectItems = new ListViewDefinition
            {
                Title = "Approve/reject Items",
                Url = "ApproveReject.aspx"
            };

            public static ListViewDefinition MySubmissions = new ListViewDefinition
            {
                Title = "My submissions",
                Url = "MySubmissions.aspx"
            };

            public static ListViewDefinition Thumbnails = new ListViewDefinition
            {
                Title = "Thumbnails",
                Url = "Thumbnails.aspx"
            };
        }

        public static class Pages
        {
            public static ListViewDefinition AllPages = new ListViewDefinition
            {
                Title = "All Pages",
                Url = "AllItems.aspx"
            };

            public static ListViewDefinition MergeDocuments = new ListViewDefinition
            {
                Title = "Merge Documents",
                Url = "Combine.aspx"
            };

            public static ListViewDefinition RelinkDocuments = new ListViewDefinition
            {
                Title = "Relink Documents",
                Url = "repair.aspx"
            };

        }

        public static class Tasks
        {
            public static ListViewDefinition ActiveTasks = new ListViewDefinition
            {
                Title = "Active Tasks",
                Url = "active.aspx"
            };

            public static ListViewDefinition AllTasks = new ListViewDefinition
            {
                Title = "All Tasks",
                Url = "AllItems.aspx"
            };

            public static ListViewDefinition ByAssignedTo = new ListViewDefinition
            {
                Title = "By Assigned To",
                Url = "byowner.aspx"
            };

            public static ListViewDefinition ByMyGroups = new ListViewDefinition
            {
                Title = "By My Groups",
                Url = "MyGrTsks.aspx"
            };

            public static ListViewDefinition DueToday = new ListViewDefinition
            {
                Title = "Due Today",
                Url = "duetoday.aspx"
            };

            public static ListViewDefinition MyTasks = new ListViewDefinition
            {
                Title = "My Tasks",
                Url = "MyItems.aspx"
            };
        }
    }
}
