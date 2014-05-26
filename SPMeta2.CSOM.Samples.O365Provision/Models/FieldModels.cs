using System;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Samples.O365Provision.Models
{
    public static class FieldModels
    {
        #region properties

        public static FieldDefinition Contact = new FieldDefinition
        {
            Id = new Guid("{85C67EEA-5E3D-4DF3-AE8C-DC4D0A1D8BAC}"),
            Title = "Contact",
            InternalName = "spmetaCRM_Contact",
            FieldType = "Text",
            Group = ModelConsts.DefaultGroup
        };

        public static FieldDefinition Details = new FieldDefinition
        {
            Id = new Guid("{80F921C6-80C1-450B-A816-2D0C2FF1A173}"),
            Title = "Details",
            InternalName = "spmetaCRM_Details",
            FieldType = "Text",
            Group = ModelConsts.DefaultGroup
        };

        public static FieldDefinition ClientFeedback = new FieldDefinition
        {
            Id = new Guid("{3DE71186-580C-4576-9E63-50EAE1A14ABB}"),
            Title = "Client feedback",
            InternalName = "spmetaCRM_ClientFeedback",
            FieldType = "Text",
            Group = ModelConsts.DefaultGroup
        };

        public static FieldDefinition ClientRating = new FieldDefinition
        {
            Id = new Guid("{6F49F09A-44E5-41A2-A4C6-964049A1CB5D}"),
            Title = "Client Rating",
            InternalName = "spmetaCRM_ClientRating",
            FieldType = "Number",
            Group = ModelConsts.DefaultGroup
        };

        #endregion
    }
}
