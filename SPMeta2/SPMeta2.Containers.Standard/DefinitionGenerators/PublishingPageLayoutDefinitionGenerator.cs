using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class PublishingPageLayoutDefinitionGenerator : TypedDefinitionGeneratorServiceBase<PublishingPageLayoutDefinition>
    {
        public PublishingPageLayoutDefinitionGenerator()
        {
            PublishingPageContentType = new ContentTypeDefinitionGenerator().GenerateRandomDefinition() as ContentTypeDefinition;

            PublishingPageContentType.ParentContentTypeId = BuiltInPublishingContentTypeId.Page;
            PublishingPageContentType.Group = "Page Layout Content Types";
        }

        public ContentTypeDefinition PublishingPageContentType;

        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.FileName = Rnd.String() + ".aspx";

                def.Description = Rnd.String();
                def.Content = Rnd.Bool()
                        ? PublishingPageLayoutTemplates.ArticleLeft :
                        PublishingPageLayoutTemplates.ArticleRight;

                def.AssociatedContentTypeId = PublishingPageContentType.GetContentTypeId();

                def.NeedOverride = true;
            });
        }

        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            var sitePublishing = BuiltInSiteFeatures.SharePointServerPublishingInfrastructure
                               .Inherit(f =>
                               {
                                   f.Enable = true;
                               });

            var webPublishing = BuiltInWebFeatures.SharePointServerPublishing
                              .Inherit(f =>
                              {
                                  f.Enable = true;
                              });

            return new DefinitionBase[] { sitePublishing, webPublishing, PublishingPageContentType };
        }

        public override DefinitionBase GetCustomParenHost()
        {
            return BuiltInListDefinitions.Calalogs.MasterPage.Inherit<ListDefinition>(def =>
            {
                def.RequireSelfProcessing = false;
            });
        }
    }
}
