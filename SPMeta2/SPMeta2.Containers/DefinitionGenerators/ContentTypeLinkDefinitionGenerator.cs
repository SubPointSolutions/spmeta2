using System;
using SPMeta2.Containers.Services;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Standard.Enumerations;
using SPMeta2.Syntax.Default;
using System.Collections.Generic;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class ContentTypeLinkDefinitionGenerator : TypedDefinitionGeneratorServiceBase<ContentTypeLinkDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            var contentTypes = new Dictionary<string, string>
            {
                {BuiltInContentTypeNames.Task, BuiltInContentTypeId.Task},
                {BuiltInContentTypeNames.Event, BuiltInContentTypeId.Event},
                {BuiltInContentTypeNames.Contact, BuiltInContentTypeId.Contact}
            };


            return WithEmptyDefinition(def =>
            {
                var ctName = Rnd.RandomFromArray(contentTypes.Keys);
                var ctId = contentTypes[ctName];

                def.ContentTypeId = ctId;
                def.ContentTypeName = ctName;
            });
        }

        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            var webPublishing = BuiltInWebFeatures.TeamCollaborationLists
                              .Inherit(f =>
                              {
                                  f.Enable = true;
                              });

            return new[] { webPublishing };
        }

        public override ModelNode GetCustomParenHost()
        {
            var definition = new ListDefinitionGenerator().GenerateRandomDefinition(def =>
            {
                var listDef = def as ListDefinition;

                listDef.ContentTypesEnabled = true;
                listDef.TemplateType = BuiltInListTemplateTypeId.GenericList;
            });

            var node = new ListModelNode
            {
                Value = definition,
                Options = { RequireSelfProcessing = true }
            };

            return node;
        }
    }
}
