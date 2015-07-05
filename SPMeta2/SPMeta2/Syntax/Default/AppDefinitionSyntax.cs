using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class AppModelNode : TypedModelNode
    {

    }

    public static class AppDefinitionSyntax
    {
        #region methods

        public static TModelNode AddApp<TModelNode>(this TModelNode model, AppDefinition definition)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return AddApp(model, definition, null);
        }

        public static TModelNode AddApp<TModelNode>(this TModelNode model, AppDefinition definition,
            Action<AppModelNode> action)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddApps<TModelNode>(this TModelNode model, IEnumerable<AppDefinition> definitions)
           where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
