using System;
using Microsoft.SharePoint;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Samples.FoundationAppTests.Models
{
    public static class AppFieldModels
    {
        #region properties

        public static FieldDefinition ShowOnTheMainPage = new FieldDefinition
        {
            Id = new Guid("{2EBA8FEB-13A5-48D8-991B-D4B45B3B7AE3}"),
            Title = "Show on main page",
            InternalName = "spdevIntrShowOnTheMainPage",
            Description = "Flag to show announcement on the main page",
            Group = ModelConsts.MetadataGroup,
            FieldType = SPFieldType.Boolean.ToString()
        };

        public static FieldDefinition DepartmentRef = new FieldDefinition
        {
            Id = new Guid("{8AFF81C0-0BF4-4A99-9890-933B0A27060D}"),
            Title = "Department",
            InternalName = "spdevIntrDepartment",
            Description = "Company department",
            Group = ModelConsts.MetadataGroup,
            FieldType = SPFieldType.Lookup.ToString()
        };

        public static FieldDefinition DepartmentCode = new FieldDefinition
        {
            Id = new Guid("{9857074A-E76D-49EB-B916-3A18A81AD739}"),
            Title = "Department code",
            InternalName = "spdevIntrDepartmentCode",
            Description = "Company department code",
            Group = ModelConsts.MetadataGroup,
            FieldType = SPFieldType.Text.ToString()
        };

        #endregion
    }
}
