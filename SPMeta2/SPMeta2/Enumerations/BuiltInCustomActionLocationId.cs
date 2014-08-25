using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Out of the box SharePoint user custom action location ids.
    /// 
    /// Based on http://msdn.microsoft.com/en-us/library/office/bb802730(v=office.15).aspx
    /// Server Ribbon Custom Action Locations and Group IDs are not included yet.
    /// </summary>
    public static class BuiltInCustomActionLocationId
    {
        #region properties

        public static class DisplayFormToolbar
        {
            public static string Location = "DisplayFormToolbar";
        }

        public static class EditControlBlock
        {
            public static string Location = "EditControlBlock";
        }

        public static class EditFormToolbar
        {
            public static string Location = "EditFormToolbar";
        }

        public static class NewFormToolbar
        {
            public static string Location = "NewFormToolbar";
        }

        public static class ViewToolbar
        {
            public static string Location = "ViewToolbar";
        }

        public static class Microsoft
        {
            public static class SharePoint
            {
                public static class StandardMenu
                {
                    public static string Location = "Microsoft.SharePoint.StandardMenu";

                    public static class Groups
                    {
                        public static string ActionsMenu = "ActionsMenu";
                        public static string ActionsMenuForSurvey = "ActionsMenuForSurvey";
                        public static string SettingsMenuForSurvey = "SettingsMenuForSurvey";
                        public static string SiteActions = "SiteActions";
                    }
                }

                public static class ContentTypeSettings
                {
                    public static string Location = "Microsoft.SharePoint.ContentTypeSettings";

                    public static class Groups
                    {
                        public static string Fields = "Fields";
                        public static string General = "General";
                    }
                }

                public static class ContentTypeTemplateSettings
                {
                    public static string Location = "Microsoft.SharePoint.ContentTypeTemplateSettings";

                    public static class Groups
                    {
                        public static string Fields = "Fields";
                        public static string General = "General";
                    }
                }

                public static class Create
                {
                    public static string Location = "Microsoft.SharePoint.Create";

                    public static class Groups
                    {
                        public static string WebPages = "WebPages";
                    }
                }

                public static class GroupsPage
                {
                    public static string Location = "Microsoft.SharePoint.GroupsPage";

                    public static class Groups
                    {
                        public static string NewMenu = "NewMenu";
                        public static string SettingsMenu = "SettingsMenu";
                    }
                }

                public static class ListEdit
                {
                    public static string Location = "Microsoft.SharePoint.ListEdit";

                    public static class Groups
                    {
                        public static string Communications = "Communications";
                        public static string GeneralSettings = "GeneralSettings";
                        public static string Permissions = "Permissions";
                    }

                    public static class DocumentLibrary
                    {
                        public static string Location = "Microsoft.SharePoint.ListEdit.DocumentLibrary";
                    }

                }


                public static class PeoplePage
                {
                    public static string Location = "Microsoft.SharePoint.PeoplePage";

                    public static class Groups
                    {
                        public static string ActionsMenu = "ActionsMenu";
                        public static string NewMenu = "NewMenu";
                        public static string SettingsMenu = "SettingsMenu";
                    }
                }


                public static class SiteSettings
                {
                    public static string Location = "Microsoft.SharePoint.SiteSettings";

                    public static class Groups
                    {
                        public static string Customization = "Customization";
                        public static string Galleries = "Galleries";
                        public static string SiteAdministration = "SiteAdministration";
                        public static string SiteCollectionAdmin = "SiteCollectionAdmin";
                        public static string UsersAndPermissions = "UsersAndPermissions";
                    }
                }

                public static class Administration
                {

                    public static class Applications
                    {
                        public static string Location = "Microsoft.SharePoint.Administration.Applications";

                        public static class Groups
                        {
                            public static string Databases = "Databases";
                            public static string ServiceApplications = "ServiceApplications";
                            public static string SiteCollections = "SiteCollections";
                            public static string WebApplications = "WebApplications";
                        }
                    }

                    public static class Backups
                    {
                        public static string Location = "Microsoft.SharePoint.Administration.Backups";

                        public static class Groups
                        {
                            public static string FarmBackup = "FarmBackup";
                            public static string GranularBackup = "GranularBackup";
                        }
                    }

                    public static class ConfigurationWizards
                    {
                        public static string Location = "Microsoft.SharePoint.Administration.ConfigurationWizards";

                        public static class Groups
                        {
                            public static string FarmConfiguration = "FarmConfiguration";
                        }
                    }

                    public static class Default
                    {
                        public static string Location = "Microsoft.SharePoint.Administration.Default";

                        public static class Groups
                        {
                            public static string CA_Applications = "CA_Applications";
                            public static string CA_Backups = "CA_Backups";
                            public static string CA_ConfigurationWizards = "CA_ConfigurationWizards";
                            public static string CA_GeneralApplicationSettings = "CA_GeneralApplicationSettings";
                            public static string CA_Monitoring = "CA_Monitoring";
                            public static string CA_Security = "CA_Security";
                            public static string CA_SystemSettings = "CA_SystemSettings";
                            public static string CA_UpgradeAndMigration = "CA_UpgradeAndMigration";
                        }
                    }

                    public static class GeneralApplicationSettings
                    {
                        public static string Location = "Microsoft.SharePoint.Administration.GeneralApplicationSettings";

                        public static class Groups
                        {
                            public static string ExternalServiceConnections = "ExternalServiceConnections";
                            public static string SiteDirectory = "SiteDirectory";
                            public static string SPD = "SPD";
                        }
                    }


                    public static class Monitoring
                    {
                        public static string Location = "Microsoft.SharePoint.Administration.Monitoring";

                        public static class Groups
                        {
                            public static string HealthStatus = "HealthStatus";
                            public static string Reporting = "Reporting";
                            public static string TimerJobs = "TimerJobs";
                        }
                    }

                    public static class Security
                    {
                        public static string Location = "Microsoft.SharePoint.Administration.Security";

                        public static class Groups
                        {
                            public static string GeneralSecurity = "GeneralSecurity";
                            public static string InformationPolicy = "InformationPolicy";
                            public static string Users = "Users";
                        }
                    }

                    public static class SystemSettings
                    {
                        public static string Location = "Microsoft.SharePoint.Administration.SystemSettings";

                        public static class Groups
                        {
                            public static string Email = "Email";
                            public static string FarmManagement = "Farm Management";
                            public static string Servers = "Servers";
                        }
                    }

                    public static class UpgradeAndMigration
                    {
                        public static string Location = "Microsoft.SharePoint.Administration.UpgradeAndMigration";

                        public static class Groups
                        {
                            public static string Patch = "Patch";
                        }
                    }
                }
            }
        }

        #endregion
    }
}