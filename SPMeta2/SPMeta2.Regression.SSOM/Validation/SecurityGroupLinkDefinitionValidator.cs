using System.Linq;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class SecurityGroupLinkDefinitionValidator : SecurityGroupLinkModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var securableObject = ExtractSecurableObject(modelHost);
            var definition = model.WithAssertAndCast<SecurityGroupLinkDefinition>("model", value => value.RequireNotNull());

            var web = GetWebFromSPSecurableObject(securableObject);
            var spObject = ResolveSecurityGroup(web, definition);

            var assert = ServiceFactory.AssertService
                    .NewAssert(definition, spObject)
                          .ShouldNotBeNull(spObject);


            if (!string.IsNullOrEmpty(definition.SecurityGroupName))
                assert.ShouldBeEqual(m => m.SecurityGroupName, o => o.Name);
            else
                assert.SkipProperty(m => m.SecurityGroupName, "SecurityGroupName is null or empty. Skipping.");

            if (definition.IsAssociatedMemberGroup)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IsAssociatedMemberGroup);
                    var dstProp = d.GetExpressionValue(o => o.GetAssociatedMemberGroup());

                    var isValid = spObject.ID == web.AssociatedMemberGroup.ID;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.IsAssociatedMemberGroup, "IsAssociatedMemberGroup is false. Skipping.");
            }

            if (definition.IsAssociatedOwnerGroup)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IsAssociatedOwnerGroup);
                    var dstProp = d.GetExpressionValue(o => o.GetAssociatedOwnerGroup());

                    var isValid = spObject.ID == web.AssociatedOwnerGroup.ID;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.IsAssociatedOwnerGroup, "IsAssociatedOwnerGroup is false. Skipping.");
            }

            if (definition.IsAssociatedVisitorGroup)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.IsAssociatedVisitorGroup);
                    var dstProp = d.GetExpressionValue(o => o.GetAssociatedVisitorGroup());

                    var isValid = spObject.ID == web.AssociatedVisitorGroup.ID;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.IsAssociatedVisitorGroup, "IsAssociatedVisitorsGroup is false. Skipping.");
            }
        }

        #endregion
    }

    internal static class SPGroupLinkExtensions
    {
        public static SPGroup GetAssociatedVisitorGroup(this SPGroup group)
        {
            return group.ParentWeb.AssociatedVisitorGroup;
        }

        public static SPGroup GetAssociatedOwnerGroup(this SPGroup group)
        {
            return group.ParentWeb.AssociatedOwnerGroup;
        }

        public static SPGroup GetAssociatedMemberGroup(this SPGroup group)
        {
            return group.ParentWeb.AssociatedMemberGroup;
        }
    }
}
