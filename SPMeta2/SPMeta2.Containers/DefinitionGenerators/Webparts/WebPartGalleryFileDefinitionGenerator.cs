using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Containers.DefinitionGenerators.Webparts
{
    public class WebPartGalleryFileDefinitionGenerator : TypedDefinitionGeneratorServiceBase<WebPartGalleryFileDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Title = Rnd.String();
                def.FileName = string.Format("{0}.dwp", Rnd.String());

                def.Description = Rnd.String();

                def.Content = Encoding.UTF8.GetBytes(DefaultDwpWebpartTemplates.MSContentEditor);
            });
        }

        public override ModelNode GetCustomParenHost()
        {
            var definition = BuiltInListDefinitions.Catalogs.Wp.Inherit<ListDefinition>(def =>
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
