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
        protected override void MapProperties(object modelHost, SPListItem item, ContentPageDefinitionBase definition)
        {
            var typedTemplateModel = definition.WithAssertAndCast<TemplateDefinitionBase>("model", value => value.RequireNotNull());

            if (typedTemplateModel.TargetControlTypes.Count > 0)
            {
                var multiChoiceValue = new SPFieldMultiChoiceValue();

                foreach (var value in typedTemplateModel.TargetControlTypes)
                    multiChoiceValue.Add(value);

                item["TargetControlType"] = multiChoiceValue;
            }

            item["TemplateHidden"] = typedTemplateModel.HiddenTemplate;

            if (!string.IsNullOrEmpty(typedTemplateModel.Description))
                item["MasterPageDescription"] = typedTemplateModel.Description;

            if (!string.IsNullOrEmpty(typedTemplateModel.PreviewURL))
            {
                var htmlPreviewValue = new SPFieldUrlValue { Url = typedTemplateModel.PreviewURL };

                if (!string.IsNullOrEmpty(typedTemplateModel.PreviewDescription))
                    htmlPreviewValue.Description = typedTemplateModel.PreviewDescription;

                item["HtmlDesignPreviewUrl"] = htmlPreviewValue;
            }
        }

    }
}
