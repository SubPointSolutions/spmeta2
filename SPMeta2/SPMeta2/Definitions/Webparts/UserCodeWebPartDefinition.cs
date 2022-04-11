using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy web parts from sandbox solutions.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(SiteDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]

    [ExpectManyInstances]

    [ExpectWebpartType(WebPartType = "Microsoft.SharePoint.WebPartPages.SPUserCodeWebPart , Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]

    public class UserCodeWebPartDefinition : WebPartDefinition
    {
        #region constructors

        public UserCodeWebPartDefinition()
        {
            UserCodeProperties = new List<UserCodeProperty>();
        }

        #endregion

        #region properties

        [ExpectRequired]
        [DataMember]
        public Guid SolutionId { get; set; }

        [ExpectRequired]
        [DataMember]
        public string AssemblyFullName { get; set; }

        [ExpectRequired]
        [DataMember]
        public string TypeFullName { get; set; }

        [DataMember]
        public List<UserCodeProperty> UserCodeProperties { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("SolutionId", SolutionId)
                          .AddRawPropertyValue("AssemblyFullName", AssemblyFullName)
                          .AddRawPropertyValue("TypeFullName", TypeFullName)
                          .ToString();
        }

        #endregion
    }

    [DataContract]
    public class UserCodeProperty
    {
        #region properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public bool? IsTokenisable { get; set; }

        #endregion
    }
}
