using System;
using SPMeta2.Definitions;

namespace SPMeta2.Regression.Models.DynamicModels
{
    public static class DynamicFieldModels
    {
        #region properties

        public static FieldDefinition BooleanField = GetFieldTestTemplate("Boolean");
        public static FieldDefinition CurrencyField = GetFieldTestTemplate("Currency");
        public static FieldDefinition DateTimeField = GetFieldTestTemplate("DateTime");
        public static FieldDefinition GuidField = GetFieldTestTemplate("Guid");
        public static FieldDefinition LookupField = GetFieldTestTemplate("Lookup");
        public static FieldDefinition MultiChoiceField = GetFieldTestTemplate("MultiChoice");
        public static FieldDefinition NoteField = GetFieldTestTemplate("Note");
        public static FieldDefinition NumberField = GetFieldTestTemplate("Number");
        public static FieldDefinition TextField = GetFieldTestTemplate("Text");
        public static FieldDefinition URLField = GetFieldTestTemplate("URL");
        public static FieldDefinition UserField = GetFieldTestTemplate("User");

        #endregion

        #region methods

        public static FieldDefinition GetFieldTestTemplate(string fieldType)
        {
            return GetFieldTestTemplate(fieldType, null);
        }

        public static FieldDefinition GetFieldTestTemplate(string fieldType, Action<FieldDefinition> action)
        {
            var result = new FieldDefinition
            {
                Id = Guid.NewGuid(),
                Title = GetTitleFieldName(fieldType),
                InternalName = GetInternalFieldName(fieldType),
                FieldType = fieldType,
                Description = Guid.NewGuid().ToString("N"),
                Group = ModelConsts.DefaultFieldGroup
            };

            if (action != null) action(result);

            return result;
        }

        public static string GetTitleFieldName(string fieldType)
        {
            return string.Format("spmt_tl_{0}", fieldType);
        }

        public static string GetInternalFieldName(string fieldType)
        {
            return string.Format("spmt_it_{0}", fieldType);
        }

        #endregion
    }
}

