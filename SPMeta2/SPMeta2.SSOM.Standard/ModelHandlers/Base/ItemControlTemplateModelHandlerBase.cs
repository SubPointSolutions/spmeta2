using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Base
{
    public abstract class ItemControlTemplateModelHandlerBase : TemplateModelHandlerBase
    {
        protected override void MapProperties(object modelHost, Hashtable fileProperties, ContentPageDefinitionBase definition)
        {
            base.MapProperties(modelHost, fileProperties, definition);

            var typedTemplateModel = definition.WithAssertAndCast<ItemControlTemplateDefinitionBase>("model", value => value.RequireNotNull());

            if (typedTemplateModel.TargetControlTypes.Count > 0)
            {
                var multiChoiceValue = new SPFieldMultiChoiceValue();

                foreach (var value in typedTemplateModel.TargetControlTypes)
                    multiChoiceValue.Add(value);

                fileProperties["TargetControlType"] = multiChoiceValue.ToString();
            }

            if (!string.IsNullOrEmpty(typedTemplateModel.PreviewURL))
            {
                var htmlPreviewValue = new SPFieldUrlValue { Url = typedTemplateModel.PreviewURL };

                if (!string.IsNullOrEmpty(typedTemplateModel.PreviewDescription))
                    htmlPreviewValue.Description = typedTemplateModel.PreviewDescription;

                fileProperties["HtmlDesignPreviewUrl"] = htmlPreviewValue.ToString();
            }
        }

    }
}
