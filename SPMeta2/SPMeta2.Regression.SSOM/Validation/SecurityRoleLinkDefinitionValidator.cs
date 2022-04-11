﻿using System;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Containers.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Regression.SSOM.Extensions;
using SPMeta2.Utils;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class SecurityRoleLinkDefinitionValidator : SPMeta2.SSOM.ModelHandlers.SecurityRoleLinkModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var modelHostContext = modelHost.WithAssertAndCast<SPMeta2.SSOM.ModelHosts.SecurityGroupModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<SecurityRoleLinkDefinition>("model", value => value.RequireNotNull());

            var securableObject = modelHostContext.SecurableObject;
            var securityGroup = modelHostContext.SecurityGroup;

            SPWeb web = null;

            if (securableObject != null)
                web = ExtractWeb(securableObject);
            else
                web = securityGroup.ParentWeb;

            var securityRole = ResolveSecurityRole(web, definition);

            // security group -> roles
            if (securableObject == null)
                securableObject = web;

            var roleAssignments = securableObject.RoleAssignments;
            var spObject = roleAssignments.OfType<SPRoleAssignment>()
                                          .FirstOrDefault(r => r.Member.ID == securityGroup.ID);

            var assert = ServiceFactory.AssertService.NewAssert(definition, spObject);

            assert
                 .ShouldNotBeNull(spObject);

            if (definition.RegIsMustBeSingleItem())
            {
                if (roleAssignments.Count != 1)
                {
                    throw new SPMeta2Exception("There must be only one RoleAssignments. RegIsMustBeSingleItem() == true");
                }
            }

            if (!string.IsNullOrEmpty(definition.SecurityRoleName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.SecurityRoleName);
                    var dstProp = d.GetExpressionValue(o => o.GetRoleDefinitionBindings());

                    var hasRoleDefinitionBinding = spObject.RoleDefinitionBindings.Contains(securityRole);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = hasRoleDefinitionBinding
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.SecurityRoleName, "SecurityRoleName is null or empty. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.SecurityRoleType))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.SecurityRoleType);
                    var dstProp = d.GetExpressionValue(o => o.GetRoleDefinitionBindings());

                    var hasRoleDefinitionBinding = spObject.RoleDefinitionBindings.Contains(securityRole);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = hasRoleDefinitionBinding
                    };
                });

            }
            else
            {
                assert.SkipProperty(m => m.SecurityRoleType, "SecurityRoleType is null or empty. Skipping.");
            }

            if (definition.SecurityRoleId > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.SecurityRoleId);
                    var dstProp = d.GetExpressionValue(o => o.GetRoleDefinitionBindings());

                    var hasRoleDefinitionBinding = spObject.RoleDefinitionBindings.Contains(securityRole);

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = hasRoleDefinitionBinding
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.SecurityRoleId, "SecurityRoleId == 0. Skipping.");
            }

        }


        #endregion
    }
}
