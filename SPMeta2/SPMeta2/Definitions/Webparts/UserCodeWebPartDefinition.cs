using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Fields;
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
    public class UserCodeWebPartDefinition : WebPartDefinition
    {
        #region constructors

        public UserCodeWebPartDefinition()
        {
            UserCodeProperties = new List<UserCodeProperty>();
        }

        #endregion

        #region properties

        public Guid SolutionId { get; set; }
        public string AssemblyFullName { get; set; }
        public string TypeFullName { get; set; }

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

    public class UserCodeProperty
    {
        #region properties

        public string Name { get; set; }
        public string Value { get; set; }

        #endregion
    }
}
