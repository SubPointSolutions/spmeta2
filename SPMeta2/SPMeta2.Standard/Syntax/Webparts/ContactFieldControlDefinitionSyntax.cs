using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Syntax.Default;
using SPMeta2.Syntax.Default.Extensions;

namespace SPMeta2.Standard.Syntax
{
    [Serializable]
    [DataContract]
    public class ContactFieldControlModelNode : WebPartModelNode
    {

    }

    public static class ContactFieldControlDefinitionSyntax
    {
        #region methods

        public static TModelNode AddContactFieldControl<TModelNode>(this TModelNode model, ContactFieldControlDefinition definition)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return AddContactFieldControl(model, definition, null);
        }

        public static TModelNode AddContactFieldControl<TModelNode>(this TModelNode model, ContactFieldControlDefinition definition,
            Action<ContactFieldControlModelNode> action)
            where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        #endregion

        #region array overload

        public static TModelNode AddContactFieldControls<TModelNode>(this TModelNode model, IEnumerable<ContactFieldControlDefinition> definitions)
           where TModelNode : ModelNode, IWebpartHostModelNode, new()
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        #endregion
    }
}
