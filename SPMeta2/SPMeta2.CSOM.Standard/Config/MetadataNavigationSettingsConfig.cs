using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.Standard.Config
{
    public class MetadataNavigationSettingsConfig
    {
        public static MetadataNavigationSettingsConfig GetMetadataNavigationSettings(List list)
        {
            throw new SPMeta2NotImplementedException("MetadataNavigationSettings provision for CSOM isnot yet implemented  by M2 - https://github.com/SubPointSolutions/spmeta2/issues/738");
        }

        public void AddConfiguredHierarchy(MetadataNavigationHierarchyConfig metadataNavigationHierarchy)
        {
            throw new SPMeta2NotImplementedException("MetadataNavigationSettings provision for CSOM isnot yet implemented  by M2 - https://github.com/SubPointSolutions/spmeta2/issues/738");
        }

        public void AddConfiguredKeyFilter(MetadataNavigationKeyFilterConfig metadataNavigationKeyFilterConfig)
        {
            throw new SPMeta2NotImplementedException("MetadataNavigationSettings provision for CSOM isnot yet implemented  by M2 - https://github.com/SubPointSolutions/spmeta2/issues/738");
        }

        public static void SetMetadataNavigationSettings(List list, MetadataNavigationSettingsConfig settings)
        {
            throw new SPMeta2NotImplementedException("MetadataNavigationSettings provision for CSOM isnot yet implemented  by M2 - https://github.com/SubPointSolutions/spmeta2/issues/738");
        }

        public MetadataNavigationHierarchyConfig FindConfiguredHierarchy(Guid guid)
        {
            throw new SPMeta2NotImplementedException("MetadataNavigationSettings provision for CSOM isnot yet implemented  by M2 - https://github.com/SubPointSolutions/spmeta2/issues/738");
        }

        public MetadataNavigationKeyFilterConfig FindConfiguredKeyFilter(Guid guid)
        {
            throw new SPMeta2NotImplementedException("MetadataNavigationSettings provision for CSOM isnot yet implemented  by M2 - https://github.com/SubPointSolutions/spmeta2/issues/738");
        }
    }
}
