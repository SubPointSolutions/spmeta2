using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    public class TargetApplicationFieldValue
    {
        public bool IsMasked { get; set; }
        public string Name { get; set; }
        public string CredentialType { get; set; }
    }

    /// <summary>
    /// Allows to define and deploy secure store target applicationL.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.Office.SecureStoreService.Server.TargetApplication", "Microsoft.Office.SecureStoreService")]

    [DefaultRootHost(typeof(FarmDefinition))]
    [DefaultParentHost(typeof(SecureStoreApplicationDefinition))]

    [Serializable]

    [ExpectWithExtensionMethod]
    public class TargetApplicationDefinition : DefinitionBase
    {
        #region constructors

        public TargetApplicationDefinition()
        {
            TargetApplicationClams = new Collection<string>();
            Fields = new Collection<TargetApplicationFieldValue>();
        }

        #endregion

        #region properties

        public string ApplicationId { get; set; }
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string ContactEmail { get; set; }
        public int TicketTimeout { get; set; }
        public string Type { get; set; }

        public Collection<string> TargetApplicationClams { get; set; }
        public Collection<TargetApplicationFieldValue> Fields { get; set; }

        public string CredentialManagementUrl { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<TargetApplicationDefinition>(this)
                          .AddPropertyValue(p => p.ApplicationId)
                          .AddPropertyValue(p => p.FriendlyName)
                          .AddPropertyValue(p => p.Type)

                          .ToString();
        }

        #endregion
    }
}
