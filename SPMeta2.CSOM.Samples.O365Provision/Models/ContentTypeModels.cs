using System;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Samples.O365Provision.Models
{
    public class ContentTypeModels
    {
        #region properties

        public static ContentTypeDefinition Client = new ContentTypeDefinition
        {
            ParentContentTypeId = "0x01",
            Group = ModelConsts.DefaultGroup,
            Id = new Guid("{00AF00A1-9FBA-41D6-8E7F-DB37BCB1847B}"),
            Name = "CRM Client",
        };

        public static ContentTypeDefinition ClientDocument = new ContentTypeDefinition
        {
            ParentContentTypeId = "0x0101",
            Group = ModelConsts.DefaultGroup,
            Id = new Guid("{2EF91F5B-205E-4E5C-AA71-EFFBAF6717BD}"),
            Name = "CRM Document",
        };

        #endregion
    }
}
