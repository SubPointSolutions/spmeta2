﻿using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SPMeta2.Syntax.Default
{
    [Serializable]
    [DataContract]
    public class ComposedLookItemLinkModelNode : TypedModelNode
    {

    }

    public static class ComposedLookItemLinkDefinitionSyntax
    {
        #region methods

        public static TModelNode AddComposedLookItemLink<TModelNode>(this TModelNode model, ComposedLookItemLinkDefinition definition)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return AddComposedLookItemLink(model, definition, null);
        }

        public static TModelNode AddComposedLookItemLink<TModelNode>(this TModelNode model, ComposedLookItemLinkDefinition definition,
            Action<ComposedLookItemLinkModelNode> action)
            where TModelNode : ModelNode, IWebHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion
    }
}
