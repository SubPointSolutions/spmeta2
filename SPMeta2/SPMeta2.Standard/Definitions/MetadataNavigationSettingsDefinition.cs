using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Utils;

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
    [SelfHostCapabilityAttribute]
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
        [DataMember]
        public List<MetadataNavigationHierarchy> Hierarchies { get; set; }

        [ExpectValidation]
        [DataMember]
        public List<MetadataNavigationKeyFilter> KeyFilters { get; set; }

        #endregion

        #region override

        public override string ToString()
        {
            var result = string.Empty;

            if (Hierarchies.Count > 0)
            {
                result += "Hierarchies: ";

                result += string.Join(", ",
                                      Hierarchies.Where(h => h.FieldId.HasValue)
                                                              .Select(h => h.FieldId.Value.ToString())
                                                              .ToArray());
            }

            if (KeyFilters.Count > 0)
            {
                if (!string.IsNullOrEmpty(result))
                    result += " ";

                result += "KeyFilters: ";

                result += string.Join(", ",
                                      KeyFilters.Where(h => h.FieldId.HasValue)
                                                             .Select(h => h.FieldId.Value.ToString())
                                                             .ToArray());
            }

            if (!string.IsNullOrEmpty(result))
                return result;

            return new ToStringResultRaw()
                        .ToString();
        }

        #endregion
    }
}
