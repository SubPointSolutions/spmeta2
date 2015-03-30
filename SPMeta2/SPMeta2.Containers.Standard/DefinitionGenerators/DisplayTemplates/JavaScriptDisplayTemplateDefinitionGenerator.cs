using System;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.Standard.DefinitionGenerators.DisplayTemplates
{
    public class JavaScriptDisplayTemplateDefinitionGenerator : TypedDefinitionGeneratorServiceBase<JavaScriptDisplayTemplateDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.FileName = string.Format("{0}.js", Rnd.String());
                def.Title = Rnd.String();

                def.Content = Rnd.Content();

                
                def.TargetScope = Rnd.String();

                def.TargetControlType = BuiltInJSTargetControlType.View;
                def.Standalone = BuiltInJSTemplateType.Override;
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
