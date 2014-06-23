using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Model.Consts;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegSiteFields
    {
        #region const

        private static string GetInternalName(string name)
        {
            return string.Format("spmt2rg_st_{0}", name);
        }

        #endregion

        #region properties

        public static FieldDefinition BooleanField = new FieldDefinition
        {
            Id = new Guid("{91dc6f97-7bc8-4b67-8030-5c32d1ca66dd}"),
            Title = "Boolean Field",
            InternalName = GetInternalName("BooleanField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Boolean
        };

        public static FieldDefinition ChoiceField = new FieldDefinition
        {
            Id = new Guid("{416b594e-7dc0-4d4b-99e8-71b2d8912f62}"),
            Title = "Choice Field",
            InternalName = GetInternalName("ChoiceField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Choice
        };

        public static FieldDefinition CurrencyField = new FieldDefinition
        {
            Id = new Guid("{e778e90a-fc7a-46f9-ad9d-1461531bfc44}"),
            Title = "Currency Field",
            InternalName = GetInternalName("CurrencyField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Currency
        };

        public static FieldDefinition DateTimeField = new FieldDefinition
        {
            Id = new Guid("{031906b0-2ab1-4b1e-a899-01949deaee26}"),
            Title = "DateTime Field",
            InternalName = GetInternalName("DateTimeField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.DateTime
        };

        public static FieldDefinition GuidField = new FieldDefinition
        {
            Id = new Guid("{e547af73-6a2a-4349-9297-1d4ec40a0680}"),
            Title = "Guid Field",
            InternalName = GetInternalName("GuidField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Guid
        };

        public static FieldDefinition LookupField = new FieldDefinition
        {
            Id = new Guid("{3fa9c397-712a-4599-b965-97594f400baa}"),
            Title = "Lookup Field",
            InternalName = GetInternalName("LookupField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Lookup
        };

        public static FieldDefinition MultiChoiceField = new FieldDefinition
        {
            Id = new Guid("{3f900842-671a-4ab2-a4f1-5d2eb9402af7}"),
            Title = "MultiChoice Field",
            InternalName = GetInternalName("MultiChoiceField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.MultiChoice
        };

        public static FieldDefinition NoteField = new FieldDefinition
        {
            Id = new Guid("{a42ca944-b862-4586-abe0-8caf7706e847}"),
            Title = "Note Field",
            InternalName = GetInternalName("NoteField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Note
        };

        public static FieldDefinition NumberField = new FieldDefinition
        {
            Id = new Guid("{ad793045-c0dc-44cc-b35b-be307fc9570f}"),
            Title = "Number Field",
            InternalName = GetInternalName("NumberField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Number
        };

        public static FieldDefinition TextField = new FieldDefinition
        {
            Id = new Guid("{fd661e3a-4fcf-4f34-9a3e-2a303b46cc30}"),
            Title = "Text Field",
            InternalName = GetInternalName("TextField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Text
        };

        public static FieldDefinition UrlField = new FieldDefinition
        {
            Id = new Guid("{c8cd864f-8484-422c-8112-23766f3c234a}"),
            Title = "Url Field",
            InternalName = GetInternalName("UrlField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.URL
        };

        public static FieldDefinition UserField = new FieldDefinition
        {
            Id = new Guid("{be43d460-d733-457d-a97b-e1804f0b4559}"),
            Title = "User Field",
            InternalName = GetInternalName("UserField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.User
        };

        #endregion
    }
}
