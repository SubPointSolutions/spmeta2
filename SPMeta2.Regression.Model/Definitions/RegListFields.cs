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
    public static class RegListFields
    {
        #region const

        private static string GetInternalName(string name)
        {
            return string.Format("spmt2rg_lst_{0}", name);
        }

        #endregion

        #region properties

        public static FieldDefinition BooleanField = new FieldDefinition
        {
            Id = new Guid("{08f47106-2769-4343-ac80-14bc896a072c}"),
            Title = "Boolean Field",
            InternalName = GetInternalName("BooleanField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Boolean
        };

        public static FieldDefinition ChoiceField = new FieldDefinition
        {
            Id = new Guid("{5d05b336-3f2a-4839-a453-601b1c62b2b4}"),
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
            Id = new Guid("{e035735f-1e62-4563-bf89-ff8ee9cb49b5}"),
            Title = "DateTime Field",
            InternalName = GetInternalName("DateTimeField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.DateTime
        };

        public static FieldDefinition GuidField = new FieldDefinition
        {
            Id = new Guid("{248b97fa-5590-4f30-bbfb-7514b96d55d7}"),
            Title = "Guid Field",
            InternalName = GetInternalName("GuidField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Guid
        };

        public static FieldDefinition LookupField = new FieldDefinition
        {
            Id = new Guid("{86a0930c-3a73-4bc4-977a-d4feccc9d668}"),
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
            Id = new Guid("{1a4a601a-2bbb-4a84-b99e-feff2bcc0d5a}"),
            Title = "Note Field",
            InternalName = GetInternalName("NoteField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Note
        };

        public static FieldDefinition NumberField = new FieldDefinition
        {
            Id = new Guid("{50acb746-8f06-41c6-a7b0-f89cb40a9629}"),
            Title = "Number Field",
            InternalName = GetInternalName("NumberField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Number
        };

        public static FieldDefinition TextField = new FieldDefinition
        {
            Id = new Guid("{6324a796-6030-46a8-a4ab-0542eb5349ef}"),
            Title = "Text Field",
            InternalName = GetInternalName("TextField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.Text
        };

        public static FieldDefinition UrlField = new FieldDefinition
        {
            Id = new Guid("{f3b42132-a2d7-4cd0-b8c2-d5be24062bd3}"),
            Title = "Url Field",
            InternalName = GetInternalName("UrlField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.URL
        };

        public static FieldDefinition UserField = new FieldDefinition
        {
            Id = new Guid("{f3d0a3ec-e97c-4443-9c95-9b1480b15c91}"),
            Title = "User Field",
            InternalName = GetInternalName("UserField"),
            Description = String.Empty,
            Group = ModelConsts.DefaultGroup,
            FieldType = BuiltInFieldTypes.User
        };

        #endregion
    }
}
