using System;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Syntax.Default.Extensions;
using System.Collections.Generic;

namespace SPMeta2.Syntax.Default
{
    public static class SecurityGroupDefinitionSyntax
    {
        #region methods

        public static SiteModelNode AddSecurityGroup(this SiteModelNode model, SecurityGroupDefinition definition)
        {
            return AddSecurityGroup(model, definition, null);
        }

        public static SiteModelNode AddSecurityGroup(this SiteModelNode model, SecurityGroupDefinition definition, Action<ModelNode> action)
        {
            return model.AddTypedDefinitionNode(definition, action);
        }

        //public static WebModelNode AddSecurityGroup(this WebModelNode model, SecurityGroupDefinition definition)
        //{
        //    return AddSecurityGroup(model, definition, null);
        //}

        //public static WebModelNode AddSecurityGroup(this WebModelNode model, SecurityGroupDefinition definition, Action<ModelNode> action)
        //{
        //    return model.AddTypedDefinitionNode(definition, action);
        //}

        #endregion

        #region array overload

        public static SiteModelNode AddSecurityGroups(this SiteModelNode model, IEnumerable<SecurityGroupDefinition> definitions)
        {
            foreach (var definition in definitions)
                model.AddDefinitionNode(definition);

            return model;
        }

        //public static WebModelNode AddSecurityGroups(this WebModelNode model, IEnumerable<SecurityGroupDefinition> definitions)
        //{
        //    foreach (var definition in definitions)
        //        model.AddDefinitionNode(definition);

        //    return model;
        //}

        #endregion
    }
}
