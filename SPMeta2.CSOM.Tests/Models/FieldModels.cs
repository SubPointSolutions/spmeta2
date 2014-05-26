using System;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.Tests.Models
{
    public static class FieldModels
    {
        #region properties

        public static FieldDefinition Contact = new FieldDefinition
        {
            Id = new Guid("{85C67EEA-5E3D-4DF3-AE8C-DC4D0A1D8BAC}"),
            Title = "test_Contact",
            InternalName = "test_Contact",
            FieldType = "Text",
            Group = ModelConsts.DefaultGroup
        };

        public static FieldDefinition Details = new FieldDefinition
        {
            Id = new Guid("{80F921C6-80C1-450B-A816-2D0C2FF1A173}"),
            Title = "test_Details",
            InternalName = "test_Details",
            FieldType = "Text",
            Group = ModelConsts.DefaultGroup
        };

        public static FieldDefinition Details1 = new FieldDefinition
        {
            Id = new Guid("{3DE71186-580C-4576-9E63-50EAE1A14ABB}"),
            Title = "test_Details1",
            InternalName = "test_Details1",
            FieldType = "Text",
            Group = ModelConsts.DefaultGroup
        };

        public static FieldDefinition Details2 = new FieldDefinition
        {
            Id = new Guid("{6F49F09A-44E5-41A2-A4C6-964049A1CB5D}"),
            Title = "test_Details2",
            InternalName = "test_Details2",
            FieldType = "Text",
            Group = ModelConsts.DefaultGroup
        };

        public static FieldDefinition TaxCategory = new FieldDefinition
        {
            Id = new Guid("{C0A95827-4C8B-4888-9ACE-2ACF28C4612C}"),
            Title = "test_TaxCategory",
            InternalName = "test_TaxCategory",
            FieldType = BuiltInFieldTypes.TaxonomyFieldType,
            Group = ModelConsts.DefaultGroup
        };

        public static FieldDefinition TaxCategoryMulti = new FieldDefinition
        {
            Id = new Guid("{2EEC306A-06F2-469D-8813-65A2B6E033A2}"),
            Title = "test_TaxCategoryMulti",
            InternalName = "test_TaxCategoryMulti",
            FieldType = BuiltInFieldTypes.TaxonomyFieldTypeMulti,
            Group = ModelConsts.DefaultGroup
        };

        #endregion
    }
}
