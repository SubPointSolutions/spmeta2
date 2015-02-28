using System;
using System.Collections.Generic;
using System.Text;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class MasterPageDefinitionGenerator : TypedDefinitionGeneratorServiceBase<MasterPageDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.FileName = Rnd.String() + ".master";

                def.Description = Rnd.String();

                var pageIndex = Rnd.Int(2);
                var pages = new[]
                {
                    Encoding.UTF8.GetBytes(DefaultMasterPageTemplates.Oslo),
                    Encoding.UTF8.GetBytes(DefaultMasterPageTemplates.Seattle)
                };

                def.Content = pages[pageIndex];
                def.NeedOverride = true;

                if (Rnd.Bool())
                    def.DefaultCSSFile = string.Format("{0}.css", Rnd.String());
            });
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
