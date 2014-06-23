using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegWebPartPages
    {
        public static WebPartPageDefinition Page1 = new WebPartPageDefinition
        {
            Title = "WebPartPage1",
            FileName = "WebPartPage1.aspx",
            PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1,
            NeedOverride = true
        };

        public static WebPartPageDefinition Page2 = new WebPartPageDefinition
        {
            Title = "WebPartPage2",
            FileName = "WebPartPage2.aspx",
            PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd2,
            NeedOverride = true
        };

        public static WebPartPageDefinition WebPartContainerPage = new WebPartPageDefinition
        {
            Title = "WebPartContainerPage",
            FileName = "WebPartContainerPage.aspx",
            PageLayoutTemplate = BuiltInWebPartPageTemplates.spstd1,
            NeedOverride = true
        };
    }
}
