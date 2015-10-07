using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default
{

    [Serializable]
    [DataContract]
    public class ScriptEditorWebPartModelNode : WebPartModelNode
    {

    }

    public static class ScriptEditorWebPartDefinitionSyntax
    {
        #region methods

        public static TModelNode AddScriptEditorWebPart<TModelNode>(this TModelNode model, ScriptEditorWebPartDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddScriptEditorWebPart(model, definition, null);
        }

        public static TModelNode AddScriptEditorWebPart<TModelNode>(this TModelNode model, ScriptEditorWebPartDefinition definition,
            Action<ScriptEditorWebPartModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddScriptEditorWebParts<TModelNode>(this TModelNode model, IEnumerable<ScriptEditorWebPartDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
