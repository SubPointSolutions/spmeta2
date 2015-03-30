using System;
using Microsoft.SharePoint.Client;
using SPMeta2.CSOM.Standard.ModelHandlers.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.DisplayTemplates
{
    public class ItemDisplayTemplateModelHandler : ItemControlTemplateModelHandlerBase
    {
        public override string FileExtension
        {
            get { return "html"; }
            set
            {

            }
        }

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {
            base.MapProperties(modelHost, item, definition);

            var typedDefinition = definition.WithAssertAndCast<ItemDisplayTemplateDefinition>("model", value => value.RequireNotNull());

            item[BuiltInInternalFieldNames.ContentTypeId] = "0x0101002039C03B61C64EC4A04F5361F38510660300500DA5E";
            //item["DisplayTemplateLevel"] = "Item";


            if (!string.IsNullOrEmpty(typedDefinition.ManagedPropertyMappings))
                item["ManagedPropertyMapping"] = typedDefinition.ManagedPropertyMappings;
        }

        public override Type TargetType
        {
            get { return typeof(ItemDisplayTemplateDefinition); }
        }

        //public override string FileExtension
        //{
        //    get { throw new NotImplementedException(); }
        //    set { throw new NotImplementedException(); }
        //}
    }
}
