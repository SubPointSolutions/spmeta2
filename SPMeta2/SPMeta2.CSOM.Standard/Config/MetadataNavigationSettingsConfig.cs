using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.Config
{
    public class MetadataNavigationSettingsConfig
    {
        #region constructors

        public MetadataNavigationSettingsConfig()
        {
            SettingDocument = new XDocument(new XElement("MetadataNavigationSettings"));

            SettingDocument.Root.SetAttribute("SchemaVersion", "1");
            SettingDocument.Root.SetAttribute("IsEnabled", "True");
            SettingDocument.Root.SetAttribute("AutoIndex", "True");
        }

        #endregion

        #region properties

        protected XDocument SettingDocument { get; set; }


        #endregion

        #region static

        public static MetadataNavigationSettingsConfig GetMetadataNavigationSettings(List list)
        {
            var result = new MetadataNavigationSettingsConfig();

            var context = list.Context;

            context.Load(list, l => l.RootFolder);
            context.ExecuteQueryWithTrace();

#if NET35
            throw new SPMeta2NotImplementedException("Not implemented for SP2010 and NET35");
#endif

#if !NET35
            var props = list.RootFolder.Properties;

            if (props.FieldValues.ContainsKey("client_MOSS_MetadataNavigationSettings"))
            {
                var value = ConvertUtils.ToString(props["client_MOSS_MetadataNavigationSettings"]);

                if (!string.IsNullOrEmpty(value))
                {
                    result.SettingDocument = XDocument.Parse(value);
                }
            }

#endif

            return result;
        }

        #endregion

        #region methods

        public void EnsureDefaultFolderHierarchyNode()
        {
            var root = SettingDocument.Root;

            var parentNode = root.Descendants("NavigationHierarchies").FirstOrDefault();

            if (parentNode == null)
            {
                parentNode = new XElement("NavigationHierarchies");
                root.Add(parentNode);

                var folderHierarchyNode = new XElement("FolderHierarchy");

                folderHierarchyNode.SetAttribute("HideFoldersNode", "False");
                parentNode.Add(folderHierarchyNode);
            }
        }

        public void AddConfiguredHierarchy(MetadataNavigationHierarchyConfig item)
        {
            var currentKey = FindConfiguredKeyFilter(item.FieldId);

            if (currentKey == null)
            {
                EnsureDefaultFolderHierarchyNode();

                // parentNode should be ensured iearly in EnsureDefaultFolderHierarchyNode()
                var root = SettingDocument.Root;
                var parentNode = root.Descendants("NavigationHierarchies").FirstOrDefault();

                var newNode = new XElement("MetadataField");

                newNode.SetAttributeValue("FieldID", item.FieldId.ToString("D"));
                newNode.SetAttributeValue("FieldType", item.FieldType);

                newNode.SetAttributeValue("CachedName", item.CachedName);
                newNode.SetAttributeValue("CachedDisplayName", item.CachedDisplayName);

                parentNode.Add(newNode);
            }
        }

        public void AddConfiguredKeyFilter(MetadataNavigationKeyFilterConfig item)
        {
            var currentKey = FindConfiguredKeyFilter(item.FieldId);

            if (currentKey == null)
            {
                var root = SettingDocument.Root;

                var parentNode = root.Descendants("KeyFilters").FirstOrDefault();

                if (parentNode == null)
                {
                    parentNode = new XElement("KeyFilters");
                    root.Add(parentNode);
                }

                var newNode = new XElement("MetadataField");

                newNode.SetAttributeValue("FieldID", item.FieldId.ToString("D"));
                newNode.SetAttributeValue("FieldType", item.FieldType);

                newNode.SetAttributeValue("CachedName", item.CachedName);
                newNode.SetAttributeValue("CachedDisplayName", item.CachedDisplayName);

                parentNode.Add(newNode);
            }
        }

        public static void SetMetadataNavigationSettings(List list, MetadataNavigationSettingsConfig settings)
        {
            var xmlValue = settings.SettingDocument.Root.ToString();

            var context = list.Context;

            context.Load(list, l => l.RootFolder);
            context.ExecuteQueryWithTrace();

#if NET35
            throw new SPMeta2NotImplementedException("Not implemented for SP2010 and NET35");
#endif

#if !NET35

            list.RootFolder.Properties["client_MOSS_MetadataNavigationSettings"] = xmlValue;
            list.RootFolder.Update();
            list.Update();

            context.ExecuteQueryWithTrace();
#endif
        }

        public MetadataNavigationHierarchyConfig FindConfiguredHierarchy(Guid guid)
        {
            var root = SettingDocument.Root;
            var parentNode = root.Descendants("NavigationHierarchies").FirstOrDefault();

            if (parentNode == null)
                return null;

            var nodes = parentNode.Descendants("MetadataField");
            var targetNode = nodes.FirstOrDefault(n => n.GetAttributeValue("FieldID") == guid.ToString("D"));

            if (targetNode != null)
            {
                var result = new MetadataNavigationHierarchyConfig();

                result.FieldId = ConvertUtils.ToGuid(targetNode.GetAttributeValue("FieldID")).Value;

                result.FieldType = ConvertUtils.ToString(targetNode.GetAttributeValue("FieldType"));
                result.CachedDisplayName = ConvertUtils.ToString(targetNode.GetAttributeValue("CachedDisplayName"));
                result.CachedName = ConvertUtils.ToString(targetNode.GetAttributeValue("CachedName"));

                return result;
            }

            return null;
        }

        public MetadataNavigationKeyFilterConfig FindConfiguredKeyFilter(Guid guid)
        {
            var root = SettingDocument.Root;
            var parentNode = root.Descendants("KeyFilters").FirstOrDefault();

            if (parentNode == null)
                return null;

            var nodes = parentNode.Descendants("MetadataField");
            var targetNode = nodes.FirstOrDefault(n => n.GetAttributeValue("FieldID") == guid.ToString("D"));

            if (targetNode != null)
            {
                var result = new MetadataNavigationKeyFilterConfig();

                result.FieldId = ConvertUtils.ToGuid(targetNode.GetAttributeValue("FieldID")).Value;

                result.FieldType = ConvertUtils.ToString(targetNode.GetAttributeValue("FieldType"));
                result.CachedDisplayName = ConvertUtils.ToString(targetNode.GetAttributeValue("CachedDisplayName"));
                result.CachedName = ConvertUtils.ToString(targetNode.GetAttributeValue("CachedName"));

                return result;
            }

            return null;
        }

        #endregion
    }
}
