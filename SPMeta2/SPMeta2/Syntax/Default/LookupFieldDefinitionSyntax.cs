using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{
    public static class LookupFieldDefinitionSyntax
    {
        #region methods

        public static SiteModelNode AddLookupField(this SiteModelNode model, LookupFieldDefinition definition)
        {
            return AddLookupField(model, definition, null);
        }

        public static SiteModelNode AddLookupField(this SiteModelNode model, LookupFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }
        public static WebModelNode AddLookupField(this WebModelNode model, LookupFieldDefinition definition)
        {
            return AddLookupField(model, definition, null);
        }

        public static WebModelNode AddLookupField(this WebModelNode model, LookupFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ListModelNode AddLookupField(this ListModelNode model, LookupFieldDefinition definition)
        {
            return AddLookupField(model, definition, null);
        }

        public static ListModelNode AddLookupField(this ListModelNode model, LookupFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddLookupFields(this ModelNode model, IEnumerable<LookupFieldDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
