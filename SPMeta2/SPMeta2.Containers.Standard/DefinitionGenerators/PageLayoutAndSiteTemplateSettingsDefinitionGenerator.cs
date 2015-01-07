using System;
using System.Collections.Generic;
using SPMeta2.Containers.DefinitionGenerators;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class PageLayoutAndSiteTemplateSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<PageLayoutAndSiteTemplateSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                // webs
                def.InheritWebTemplates = Rnd.Bool();

                if (!def.InheritWebTemplates.Value)
                    def.UseAnyWebTemplate = Rnd.Bool();

                if (def.UseAnyWebTemplate.HasValue && !def.UseAnyWebTemplate.Value)
                {
                    def.UseDefinedWebTemplates = true;// Rnd.Bool();

                    def.DefinedWebTemplates.Add(BuiltInWebTemplates.Collaboration.TeamSite);
                    def.DefinedWebTemplates.Add(BuiltInWebTemplates.Collaboration.Blog);
                    def.DefinedWebTemplates.Add(BuiltInWebTemplates.Collaboration.BlankSite);
                }

                def.ResetAllSubsitesToInheritWebTemplates = Rnd.Bool();

                // page layouts
                def.InheritPageLayouts = Rnd.Bool();

                if (!def.InheritPageLayouts.Value)
                    def.UseAnyPageLayout = Rnd.Bool();

                if (def.UseAnyPageLayout.HasValue && !def.UseAnyPageLayout.Value)
                {
                    def.UseDefinedPageLayouts = true; Rnd.Bool();

                    def.DefinedPageLayouts.Add(BuiltInPublishingPageLayoutNames.ArticleLeft);
                    def.DefinedPageLayouts.Add(BuiltInPublishingPageLayoutNames.ArticleRight);
                    def.DefinedPageLayouts.Add(BuiltInPublishingPageLayoutNames.BlankWebPartPage);
                }

                def.ResetAllSubsitesToInheritPageLayouts = Rnd.Bool();

                // default page layout
                def.InheritDefaultPageLayout = Rnd.Bool();

                if (!def.InheritDefaultPageLayout.Value)
                    def.UseDefinedDefaultPageLayout = true; Rnd.Bool();

                if (def.UseDefinedDefaultPageLayout.HasValue && def.UseDefinedDefaultPageLayout.Value)
                    def.DefinedDefaultPageLayout = BuiltInPublishingPageLayoutNames.ArticleRight;

                def.ResetAllSubsitesToInheritPageLayouts = Rnd.Bool();

                // convert settings
                def.ConverBlankSpacesIntoHyphen = Rnd.Bool();
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

            return new[] { sitePublishing, webPublishing };
        }

        public override DefinitionBase GetCustomParenHost()
        {
            var def = new WebDefinitionGenerator().GenerateRandomDefinition() as WebDefinition;

            def.WebTemplate = BuiltInWebTemplates.Publishing.PublishingSite_CMS;

            return def;
        }
    }
}
