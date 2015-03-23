using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy web parts from sandbox solutions.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
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
    }

    public class UserCodeProperty
    {
        #region properties

        public string Name { get; set; }
        public string Value { get; set; }

        #endregion
    }
}
