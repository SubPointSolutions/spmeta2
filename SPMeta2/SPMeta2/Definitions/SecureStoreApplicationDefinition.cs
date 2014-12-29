using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows too define and deploy Secure Store Application.
    /// </summary>

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.Office.SecureStoreService.Server.ISecureStore", "Microsoft.Office.SecureStoreService")]
    //[SPObjectTypeAttribute(SPObjectModelType.CSOM, "", "")]


    [DefaultRootHostAttribute(typeof(FarmDefinition))]
    [DefaultParentHostAttribute(typeof(FarmDefinition))]

    [ExpectAddHostExtensionMethod]
    [Serializable]
    public class SecureStoreApplicationDefinition : DefinitionBase
    {
        #region properties

        public string Name { get; set; }
        public Guid? Id { get; set; }
        public bool UseDefault { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SecureStoreApplicationDefinition>(this)
                          .AddPropertyValue(p => p.Name)
                          .AddPropertyValue(p => p.Id)
                          .AddPropertyValue(p => p.UseDefault)

                          .ToString();
        }

        #endregion
    }
}
