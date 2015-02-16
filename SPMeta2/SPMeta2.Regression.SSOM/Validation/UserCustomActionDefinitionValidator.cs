using System;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;

using SPMeta2.Utils;
using SPMeta2.SSOM.ModelHosts;
using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class UserCustomActionDefinitionValidator : UserCustomActionModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<UserCustomActionDefinition>("model", value => value.RequireNotNull());
            var spObject = GetCurrentCustomUserAction(modelHost, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, definition, spObject)
                                            .ShouldNotBeNull(spObject)
                                            .ShouldBeEqual(m => m.Name, o => o.Name)
                                            .ShouldBeEqual(m => m.Title, o => o.Title)
                                            .ShouldBeEqual(m => m.Description, o => o.Description)
                                            .ShouldBeEqual(m => m.Group, o => o.Group)
                                            .ShouldBeEqual(m => m.Location, o => o.Location)

                                            .ShouldBeEqual(m => m.Sequence, o => o.Sequence)
                                            .ShouldBeEqual(m => m.Url, o => o.Url)
                //.ShouldBeEqual(m => m.RegistrationId, o => o.RegistrationId)
                                            .ShouldBeEqual(m => m.RegistrationType, o => o.GetRegistrationType());

            assert
                .ShouldBeEqual(m => m.ScriptSrc, o => o.ScriptSrc)
                .ShouldBeEqual(m => m.ScriptBlock, o => o.ScriptBlock);

            var registrationIdIsGuid = ConvertUtils.ToGuid(spObject.RegistrationId);

            if (!string.IsNullOrEmpty(definition.CommandUIExtension))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.CommandUIExtension);
                    var dstProp = d.GetExpressionValue(ct => ct.CommandUIExtension);

                    var isValid =
                        srcProp.Value.ToString()
                                     .Replace(" ", "")
                                     .Replace(Environment.NewLine, "")
                                     .Replace("\n", "") ==
                        dstProp.Value.ToString()
                                     .Replace(" ", "")
                                     .Replace(Environment.NewLine, "")
                                     .Replace("\n", "");

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
                assert.SkipProperty(m => m.CommandUIExtension, "CommandUIExtension is null or empty. Skipping.");
            }

            if (registrationIdIsGuid.HasValue)
            {
                // this is list scoped user custom action reg
                // skipping validation
                assert.SkipProperty(m => m.RegistrationId, "RegistrationId is GUID. List scope user custom action. Skipping validation.");
            }
            else
            {
                assert.ShouldBeEqual(m => m.RegistrationId, o => o.RegistrationId);
            }

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(def => def.Rights);
                var dstProp = d.GetExpressionValue(ct => ct.Rights);

                var hasCorrectRights = true;

                foreach (var srcRight in s.Rights)
                {
                    var srcPermission = (SPBasePermissions)Enum.Parse(typeof(SPBasePermissions), srcRight);

                    var tmpRight = d.Rights.HasFlag(srcPermission);

                    if (tmpRight == false)
                        hasCorrectRights = false;
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = hasCorrectRights
                };
            });

        }
    }

    internal static class SPUserActionHelpers
    {
        public static string GetRegistrationType(this SPUserCustomAction action)
        {
            return action.RegistrationType.ToString();
        }
    }
}
