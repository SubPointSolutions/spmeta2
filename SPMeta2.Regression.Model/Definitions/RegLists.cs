using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegLists
    {
        #region properties

        public static ListDefinition DocumentLibrary = new ListDefinition
        {
            Title = "Custom Document Library",
            Url = "spmeta2_custom_document_library",
            ContentTypesEnabled = true,
            Description = "Custom doc lib",
            TemplateType = BuiltInListTemplateTypeId.DocumentLibrary
        };

        public static ListDefinition GenericList = new ListDefinition
        {
            Title = "Custom List",
            Url = "spmeta2_custom_list",
            ContentTypesEnabled = true,
            Description = "Custom list",
            TemplateType = BuiltInListTemplateTypeId.GenericList
        };


        #endregion
    }
}
