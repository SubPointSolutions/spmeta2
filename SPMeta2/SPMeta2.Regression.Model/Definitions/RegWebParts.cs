using SPMeta2.Definitions;
using SPMeta2.Regression.Model.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Model.Definitions
{
    public static class RegWebParts
    {
        public static WebPartDefinition ContentEditorWebPart = new WebPartDefinition
        {
            Title = "About Us",
            Id = "appAboutUsWebPart",
            WebpartXmlTemplate = WebPartTemplates.CEWP,
            ZoneId = "Header",
            ZoneIndex = 10
        };

    }
}
