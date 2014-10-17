using System;
using System.Collections.Generic;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class WebNavigationSettingsDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WebNavigationSettingsDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.CurrentNavigationMaximumNumberOfDynamicItems = Rnd.Int(15) + 1;
                def.CurrentNavigationShowPages = Rnd.Bool();
                def.CurrentNavigationShowSubsites = Rnd.Bool();

                def.GlobalNavigationMaximumNumberOfDynamicItems = Rnd.Int(15) + 1;
                def.GlobalNavigationShowPages = Rnd.Bool();
                def.GlobalNavigationShowSubsites = Rnd.Bool();

                // TODO, source
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
    }
}
