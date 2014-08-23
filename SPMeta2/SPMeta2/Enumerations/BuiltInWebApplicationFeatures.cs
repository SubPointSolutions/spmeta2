using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Out of the box SharePoint web application features.
    /// </summary>
    public class BuiltInWebApplicationFeatures
    {
        /// <summary>
        /// Allow users to acquire apps that require internet facing endpoints from the SharePoint Store.
        /// </summary>
        public static FeatureDefinition AppsThatRequireAccessibleInternetFacingEndpoints = new FeatureDefinition
        {
            Title = "Apps that require accessible internet facing endpoints",
            Id = new Guid("{7877bbf6-30f5-4f58-99d9-a0cc787c1300}"),
            Scope = FeatureDefinitionScope.WebApplication,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Provides the infrastructure to synchronize metadata for Document Sets.
        /// </summary>
        public static FeatureDefinition DocumentSetsMetadataSynchronization = new FeatureDefinition
        {
            Title = "Document Sets metadata synchronization",
            Id = new Guid("{3a4ce811-6fe0-4e97-a6ae-675470282cf2}"),
            Scope = FeatureDefinitionScope.WebApplication,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Uses the Search Server Service for search over broad enterprise content. In addition to list and site scopes, provides search over people profiles, business data, remote and custom content sources. Uses multiple tabs to display results in the Search Center.
        /// </summary>
        public static FeatureDefinition SharePointServerEnterpriseSearch = new FeatureDefinition
        {
            Title = "SharePoint Server Enterprise Search",
            Id = new Guid("{4750c984-7721-4feb-be61-c660c6190d43}"),
            Scope = FeatureDefinitionScope.WebApplication,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Features such as Visio Services, Access Services, and Excel Services Application, included in the SharePoint Server Enterprise License.
        /// </summary>
        public static FeatureDefinition SharePointServerEnterpriseWebApplicationFeatures = new FeatureDefinition
        {
            Title = "SharePoint Server Enterprise Web application features",
            Id = new Guid("{0ea1c3b6-6ac0-44aa-9f3f-05e8dbe6d70b}"),
            Scope = FeatureDefinitionScope.WebApplication,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Uses the Search Server Service for site and list scoped searches.
        /// </summary>
        public static FeatureDefinition SharePointServerSiteSearch = new FeatureDefinition
        {
            Title = "SharePoint Server Site Search",
            Id = new Guid("{bc29e863-ae07-4674-bd83-2c6d0aa5623f}"),
            Scope = FeatureDefinitionScope.WebApplication,
            ForceActivate = false,
            Enable = false
        };

        /// <summary>
        /// Features such as user profiles and search, included in the SharePoint Server Standard License.
        /// </summary>
        public static FeatureDefinition SharePointServerStandardWebApplicationFeatures = new FeatureDefinition
        {
            Title = "SharePoint Server Standard Web application features",
            Id = new Guid("{4f56f9fa-51a0-420c-b707-63ecbb494db1}"),
            Scope = FeatureDefinitionScope.WebApplication,
            ForceActivate = false,
            Enable = false
        };



    }
}
