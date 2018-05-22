using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Exceptions;
using SPMeta2.Utils;

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

    [ExpectManyInstances]

    [ExpectWebpartType(WebPartType = "Microsoft.SharePoint.WebPartPages.ScriptEditorWebPart, Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c")]


    public class ScriptEditorWebPartDefinition : WebPartDefinition
    {
        #region properties

        private string _id;

        [DataMember]
        public override string Id
        {
            get { return _id; }
            set
            {
                // https://github.com/SubPointSolutions/spmeta2/issues/450
                if (string.IsNullOrEmpty(value) || value.Length < 32)
                {
                    throw new SPMeta2InvalidDefinitionPropertyException(
                        "Id property for ScriptEditorWebPartDefinition must be more than 32 symbols - https://github.com/SubPointSolutions/spmeta2/issues/450");
                }

                _id = value;
            }
        }

        [DataMember]
        public string Content { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())
                          .AddRawPropertyValue("Content", Content)

                          .ToString();
        }

        #endregion
    }
}
