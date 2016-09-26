using System;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SubPointSolutions.Docs.Code.Consts;

namespace SubPointSolutions.Docs.Code.Definitions
{
    public static class DocFields
    {
        #region properties

        public static class Clients
        {
            public static FieldDefinition ClientDescription = new FieldDefinition
            {
                Title = "Client Description",
                InternalName = "dcs_ClientDescription",
                Group = DocConsts.DefaulFieldsGroup,
                Id = new Guid("06975b67-01f5-47d7-9e2e-2702dfb8c217"),
                FieldType = BuiltInFieldTypes.Note,
            };

            public static FieldDefinition ClientNumber = new FieldDefinition
            {
                Title = "Client Number",
                InternalName = "dcs_ClientNumber",
                Group = DocConsts.DefaulFieldsGroup,
                Id = new Guid("22264486-7561-45ec-a6bc-591ba243693b"),
                FieldType = BuiltInFieldTypes.Number,
            };

            public static FieldDefinition ClientWebSite = new FieldDefinition
            {
                Title = "Client Web Site",
                InternalName = "dcs_ClientWebSite",
                Group = DocConsts.DefaulFieldsGroup,
                Id = new Guid("5e014aa8-7ed7-4986-8bd3-40e992e72779"),
                FieldType = BuiltInFieldTypes.URL,
            };

            public static FieldDefinition ClientDebit = new FieldDefinition
            {
                Title = "Client Debit",
                InternalName = "dcs_ClientDebit",
                Group = DocConsts.DefaulFieldsGroup,
                Id = new Guid("ca1f7aca-6207-4849-8508-a02dfebaa0f8"),
                FieldType = BuiltInFieldTypes.Number,
            };

            public static FieldDefinition ClientCredit = new FieldDefinition
            {
                Title = "Client Credit",
                InternalName = "dcs_ClientCredit",
                Group = DocConsts.DefaulFieldsGroup,
                Id = new Guid("be3aa713-e6e5-43b9-8d9e-f27768f1deee"),
                FieldType = BuiltInFieldTypes.Number,
            };
        }

        #endregion
    }
}
