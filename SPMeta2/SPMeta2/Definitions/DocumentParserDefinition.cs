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
    /// Allows to define and deploy custom SPDocumentParser.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPDocumentParser", "Microsoft.SharePoint")]

    [DefaultRootHostAttribute(typeof(FarmDefinition))]
    [DefaultParentHostAttribute(typeof(FarmDefinition))]

    [Serializable]
    [ExpectWithExtensionMethod]
    public class DocumentParserDefinition : DefinitionBase
    {
        #region properties

        public string FileExtension { get; set; }
        public string ProgId { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<DocumentParserDefinition>(this)
                          .AddPropertyValue(p => p.FileExtension)
                          .AddPropertyValue(p => p.ProgId)
                          .ToString();
        }

        #endregion
    }
}
