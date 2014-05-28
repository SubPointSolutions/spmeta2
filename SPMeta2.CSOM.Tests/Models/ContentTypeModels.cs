using System;
using SPMeta2.Definitions;
using SPMeta2.Extensions;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default;

namespace SPMeta2.CSOM.Tests.Models
{
    public class ContentTypeModels
    {
        #region properties

        public static ContentTypeDefinition CustomItem = new ContentTypeDefinition
        {
            ParentContentTypeId = "0x01",
            Group = ModelConsts.DefaultGroup,
            Id = new Guid("{00AF00A1-9FBA-41D6-8E7F-DB37BCB1847B}"),
            Name = "__Custom Item Content Type",
        };

        public static ContentTypeDefinition CustomDocument = new ContentTypeDefinition
        {
            //
            ParentContentTypeId = "0x0101",
            Group = ModelConsts.DefaultGroup,
            Id = new Guid("{2EF91F5B-205E-4E5C-AA71-EFFBAF6717BD}"),
            Name = "__Test Document Content Type",
        };

        public static ContentTypeDefinition CustomChildDocument = new ContentTypeDefinition
        {
            ParentContentTypeId = CustomDocument.GetContentTypeId(),
            Group = ModelConsts.DefaultGroup,
            Id = new Guid("{BF91B4C9-BCC2-4AF8-8CFB-79C8F84F1D44}"),
            Name = "__Test Child Document Content Type",
        };

        #endregion
    }
}
