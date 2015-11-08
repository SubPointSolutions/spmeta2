using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;

namespace SPMeta2.Standard.Definitions
{
    [DataContract]
    [Serializable]
    public class MetadataNavigationHierarchy
    {
        [DataMember]
        public Guid? FieldId { get; set; }
    }

    [DataContract]
    [Serializable]
    public class MetadataNavigationKeyFilter
    {
        [DataMember]
        public Guid? FieldId { get; set; }
    }

    /// <summary>
    /// Allows to define and deploy managed metadata navigation settting to the target list.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPList", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.List", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(ListDefinition))]


    [Serializable]
    [DataContract]
    [SingletonIdentity]

    [ParentHostCapability(typeof(WebDefinition))]
    public class MetadataNavigationSettingsDefinition : DefinitionBase
    {
        #region constructors
        public MetadataNavigationSettingsDefinition()
        {
            Hierarchies = new List<MetadataNavigationHierarchy>();
            KeyFilters = new List<MetadataNavigationKeyFilter>();
        }

        #endregion


        #region properties

        [ExpectValidation]
        public List<MetadataNavigationHierarchy> Hierarchies { get; set; }

        [ExpectValidation]
        public List<MetadataNavigationKeyFilter> KeyFilters { get; set; }

        #endregion
    }
}
