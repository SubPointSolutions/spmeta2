using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.Standard.Config
{
    public class MetadataNavigationSettingsConfig
    {
        public static MetadataNavigationSettingsConfig GetMetadataNavigationSettings(List list)
        {
            throw new NotImplementedException();
        }

        public void AddConfiguredHierarchy(MetadataNavigationHierarchyConfig metadataNavigationHierarchy)
        {
            throw new NotImplementedException();
        }

        public void AddConfiguredKeyFilter(MetadataNavigationKeyFilterConfig metadataNavigationKeyFilterConfig)
        {
            throw new NotImplementedException();
        }

        public static void SetMetadataNavigationSettings(List list, MetadataNavigationSettingsConfig settings)
        {
            throw new NotImplementedException();
        }

        public MetadataNavigationHierarchyConfig FindConfiguredHierarchy(Guid guid)
        {
            throw new NotImplementedException();
        }

        public MetadataNavigationKeyFilterConfig FindConfiguredKeyFilter(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
