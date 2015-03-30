using System.Collections;
using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHandlers.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Base
{
   

    public abstract class TemplateModelHandlerBase : ContentFileModelHandlerBase
    {
        protected override void MapProperties(object modelHost, Hashtable fileProperties, ContentPageDefinitionBase definition)
        {
            var typedTemplateModel = definition.WithAssertAndCast<TemplateDefinitionBase>("model", value => value.RequireNotNull());

            //fileProperties["TemplateHidden"] = typedTemplateModel.HiddenTemplate.ToString();
            fileProperties["TemplateHidden"] = typedTemplateModel.HiddenTemplate.ToString();

            if (!string.IsNullOrEmpty(typedTemplateModel.Description))
                fileProperties["MasterPageDescription"] = typedTemplateModel.Description;

        }
    }
}
