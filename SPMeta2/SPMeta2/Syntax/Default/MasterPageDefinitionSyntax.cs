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
    public class MasterPageModelNode : ListItemModelNode
    {

    }

    public static class MasterPageDefinitionSyntax
    {
        #region methods

        public static ListModelNode AddMasterPage(this ListModelNode model, MasterPageDefinition definition)
        {
            return AddMasterPage(model, definition, null);
        }

        public static ListModelNode AddMasterPage(this ListModelNode model, MasterPageDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static ModelNode AddMasterPages(this ModelNode model, IEnumerable<MasterPageDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
