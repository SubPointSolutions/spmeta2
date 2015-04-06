using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;
using System.Runtime.Serialization;

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
            return new ToStringResult<UserCodeWebPartDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.SolutionId)
                          .AddPropertyValue(p => p.AssemblyFullName)
                          .AddPropertyValue(p => p.TypeFullName)
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

        #endregion
    }
}
