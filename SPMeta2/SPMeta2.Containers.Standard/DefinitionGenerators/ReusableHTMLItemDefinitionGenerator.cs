using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class ReusableHTMLItemDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ReusableHTMLItemDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.Comments = Rnd.String();
                def.ReusableHTML = Rnd.String();

                def.ShowInDropDownMenu = Rnd.Bool();
                def.AutomaticUpdate = Rnd.Bool();
            });
        }

        public override DefinitionBase GetCustomParenHost()
        {
            return BuiltInListDefinitions.ReusableContent.Inherit<ListDefinition>(def =>
            {
                def.RequireSelfProcessing = false;
            });
        }
    }
}
