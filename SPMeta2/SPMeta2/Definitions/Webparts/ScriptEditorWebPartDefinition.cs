using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Exceptions;

namespace SPMeta2.Definitions.Webparts
{
    /// <summary>
    /// Allows to define and deploy 'Script Editor' web part.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "System.Web.UI.WebControls.WebParts.WebPart", "System.Web")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WebParts.WebPart", "Microsoft.SharePoint.Client")]

    [DefaultRootHost(typeof(WebDefinition))]
    [DefaultParentHost(typeof(WebPartPageDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]

    public class ScriptEditorWebPartDefinition : WebPartDefinition
    {
        #region properties

        private string id;

        [DataMember]
        public override string Id
        {
            get { return id; }
            set
            {
                // https://github.com/SubPointSolutions/spmeta2/issues/450
                if (string.IsNullOrEmpty(value) || value.Length < 32)
                {
                    throw new SPMeta2InvalidDefinitionPropertyException(
                        "Id property for ScriptEditorWebPartDefinition must be more than 32 symbols - https://github.com/SubPointSolutions/spmeta2/issues/450");
                }

                id = value;
            }
        }

        [DataMember]
        public string Content { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<ScriptEditorWebPartDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.Content)

                          .ToString();
        }

        #endregion
    }
}
