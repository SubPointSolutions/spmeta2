using System.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHandlers.Base;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Base
{
    public abstract class TemplateModelHandlerBase : ContentFileModelHandlerBase
    {
        #region methods

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {
            var typedDefinition = definition.WithAssertAndCast<TemplateDefinitionBase>("model", value => value.RequireNotNull());

            if (typedDefinition.TargetControlTypes.Count > 0)
            {
                item["TargetControlType"] = typedDefinition.TargetControlTypes.ToArray();
            }

            item["TemplateHidden"] = typedDefinition.HiddenTemplate;

            if (!string.IsNullOrEmpty(typedDefinition.Description))
                item["MasterPageDescription"] = typedDefinition.Description;

            if (!string.IsNullOrEmpty(typedDefinition.PreviewURL))
            {
                var htmlPreviewValue = new FieldUrlValue { Url = typedDefinition.PreviewURL };

                if (!string.IsNullOrEmpty(typedDefinition.PreviewDescription))
                    htmlPreviewValue.Description = typedDefinition.PreviewDescription;

                item["HtmlDesignPreviewUrl"] = htmlPreviewValue;
            }
        }

        #endregion
    }
}
