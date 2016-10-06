using System;
using System.Collections.Generic;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Models;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers.DefinitionGenerators;

namespace SPMeta2.Containers.Standard.DefinitionGenerators
{
    public class DocumentSetDefinitionGenerator : TypedDefinitionGeneratorServiceBase<DocumentSetDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.Name = Rnd.String();
                def.ContentTypeId = BuiltInContentTypeId.DocumentSet_Correct;
            });
        }

        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            var docSetsFeature = BuiltInSiteFeatures.DocumentSets.Inherit(def =>
            {
                def.Enable();
            });

            return new[] { docSetsFeature };
        }

        public override ModelNode GetCustomParenHost()
        {
            var listDefinitionGenerator = new ListDefinitionGenerator();
            var listDefinition = listDefinitionGenerator.GenerateRandomDefinition() as ListDefinition;

            listDefinition.TemplateType = BuiltInListTemplateTypeId.DocumentLibrary;
            listDefinition.ContentTypesEnabled = true;

            var node = new ListModelNode
            {
                Value = listDefinition,
            };

            node.AddContentTypeLink(BuiltInContentTypeId.DocumentSet_Correct);

            return node;
        }
    }
}
