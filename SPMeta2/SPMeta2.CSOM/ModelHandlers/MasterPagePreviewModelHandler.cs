using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.ModelHandlers.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class MasterPagePreviewModelHandler : ContentFileModelHandlerBase
    {
        public override string FileExtension
        {
            get { return "preview"; }
            set
            {

            }
        }

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {
            var typedDefinition = definition.WithAssertAndCast<MasterPagePreviewDefinition>("model", value => value.RequireNotNull());

            item[BuiltInInternalFieldNames.ContentTypeId] = BuiltInContentTypeId.MasterPagePreview;

            if (typedDefinition.UIVersion.Count > 0)
            {
                item["UIVersion"] = string.Join(";#", typedDefinition.UIVersion.ToArray());
            }
        }

        public override Type TargetType
        {
            get { return typeof(MasterPagePreviewDefinition); }
        }
    }
}
