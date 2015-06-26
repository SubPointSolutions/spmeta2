using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class ListFieldLinkDefinitionSyntax
    {
        #region methods

        public static ListModelNode AddListFieldLink(this ListModelNode model, FieldDefinition definition)
        {
            return AddListFieldLink(model, definition, null);
        }

        public static ListModelNode AddListFieldLink(this ListModelNode model, FieldDefinition definition, Action<ModelNode> action)
        {
            if (definition.Id != default(Guid))
            {
                return model.AddDefinitionNode(new ListFieldLinkDefinition
                {
                    FieldId = definition.Id
                }, action) as ListModelNode;
            }

            return model.AddDefinitionNode(new ListFieldLinkDefinition
            {
                FieldInternalName = definition.InternalName
            }, action) as ListModelNode;
        }

        public static ListModelNode AddListFieldLink(this ListModelNode model, ListFieldLinkDefinition definition)
        {
            return AddListFieldLink(model, definition, null);
        }

        public static ListModelNode AddListFieldLink(this ListModelNode model, ListFieldLinkDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddListFieldLinks(this ModelNode model, IEnumerable<ListFieldLinkDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        public static ListModelNode AddListFieldLinks(this ListModelNode model, IEnumerable<FieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddListFieldLink(definition);

            return model;
        }

        #endregion
    }
}
