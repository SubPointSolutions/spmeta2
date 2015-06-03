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
    public class MasterPagePreviewDefinitionGenerator : TypedDefinitionGeneratorServiceBase<MasterPagePreviewDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.FileName = Rnd.String() + ".preview";


                var pageIndex = Rnd.Int(2);
                var pages = new[]
                {
                    Encoding.UTF8.GetBytes(DefaultMasterPagePreviewTemplates.Oslo),
                    Encoding.UTF8.GetBytes(DefaultMasterPagePreviewTemplates.Seattle)
                };

                def.Content = pages[pageIndex];
                def.NeedOverride = true;
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
