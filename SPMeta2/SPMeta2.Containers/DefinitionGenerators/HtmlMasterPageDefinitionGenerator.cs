using System;
using System.Collections.Generic;
using System.Text;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class HtmlMasterPageDefinitionGenerator : TypedDefinitionGeneratorServiceBase<HtmlMasterPageDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.FileName = Rnd.String() + ".html";

                def.Description = Rnd.String();

                var pageIndex = Rnd.Int(2);
                var pages = new[]
                {
                    Encoding.UTF8.GetBytes(DefaultHtmlMasterPageTemplates.DesignManagerStarter)
                };

                def.Content = pages[0];
                def.NeedOverride = true;

                if (Rnd.Bool())
                    def.DefaultCSSFile = string.Format("{0}.css", Rnd.String());
            });
        }

        public override ModelNode GetCustomParenHost()
        {
            var definition = BuiltInListDefinitions.Catalogs.MasterPage.Inherit<ListDefinition>(def =>
            {

            });

            var node = new ListModelNode
            {
                Value = definition,
                Options = { RequireSelfProcessing = false }
            };

            return node;
        }
    }
}
