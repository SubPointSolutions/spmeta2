using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Enumerations;
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
            public static ListViewDefinition AllItems = new ListViewDefinition
            {
                Title = "All Items",
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
    }
}
