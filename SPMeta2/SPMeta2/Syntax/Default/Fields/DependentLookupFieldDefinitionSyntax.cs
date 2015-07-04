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
    public class DependentLookupFieldModelNode : FieldModelNode
    {

    }

    public static class DependentLookupFieldDefinitionSyntax
    {
        #region methods

        public static SiteModelNode AddDependentLookupField(this SiteModelNode model, DependentLookupFieldDefinition definition)
        {
            return AddDependentLookupField(model, definition, null);
        }

        public static SiteModelNode AddDependentLookupField(this SiteModelNode model, DependentLookupFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }
        public static WebModelNode AddDependentLookupField(this WebModelNode model, DependentLookupFieldDefinition definition)
        {
            return AddDependentLookupField(model, definition, null);
        }

        public static WebModelNode AddDependentLookupField(this WebModelNode model, DependentLookupFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        public static ListModelNode AddDependentLookupField(this ListModelNode model, DependentLookupFieldDefinition definition)
        {
            return AddDependentLookupField(model, definition, null);
        }

        public static ListModelNode AddDependentLookupField(this ListModelNode model, LookupFieldDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        //public static ModelNode AddDependentLookupFields(this ModelNode model, IEnumerable<DependentLookupFieldDefinition> definitions)
        //{
        //    foreach (var definition in definitions)
        //        model.AddDefinitionNode(definition);

        //    return model;
        //}

        #endregion
    }
}
