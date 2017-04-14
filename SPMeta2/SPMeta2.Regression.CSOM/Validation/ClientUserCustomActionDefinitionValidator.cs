using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Services;
using SPMeta2.Utils;


namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientUserCustomActionDefinitionValidator : UserCustomActionModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<UserCustomActionDefinition>("model", value => value.RequireNotNull());
            var spObject = GetCurrentCustomUserAction(modelHost, definition);

            var shouldCheckRegistrationType = true;

            if (modelHost is ListModelHost)
            {
                //// skipping setup for List script 
                //// System.NotSupportedException: Setting this property is not supported.  A value of List has already been set and cannot be changed.
                shouldCheckRegistrationType = false;
            }

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, definition, spObject)
                                            .ShouldBeEqual(m => m.Name, o => o.Name)
                                            .ShouldBeEqual(m => m.Title, o => o.Title)
                                            .ShouldBeEqual(m => m.Description, o => o.Description)
                                            .ShouldBeEqual(m => m.Group, o => o.Group)
                                            .ShouldBeEqual(m => m.Location, o => o.Location)
                                            .ShouldBeEqual(m => m.ScriptSrc, o => o.ScriptSrc)
                                            .ShouldBeEqual(m => m.ScriptBlock, o => o.ScriptBlock)
                                            .ShouldBeEqual(m => m.Sequence, o => o.Sequence)
                                            .ShouldBeEqual(m => m.Url, o => o.Url);
            //.ShouldBeEqual(m => m.RegistrationId, o => o.RegistrationId)
            //.ShouldBeEqual(m => m.RegistrationType, o => o.GetRegistrationType());

            if (shouldCheckRegistrationType)
                assert.ShouldBeEqual(m => m.RegistrationType, o => o.GetRegistrationType());
            else
                assert.SkipProperty(m => m.RegistrationType, "Skipping validation");

            var context = spObject.Context;

            var registrationIdIsGuid = ConvertUtils.ToGuid(spObject.RegistrationId);

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

            if (!string.IsNullOrEmpty(definition.CommandUIExtension))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.CommandUIExtension);
                    var dstProp = d.GetExpressionValue(ct => ct.CommandUIExtension);

                    var isValid = GetCommandUIString(srcProp.Value.ToString()) ==
                                  GetCommandUIString(dstProp.Value.ToString());

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

            assert
                .ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.Rights);
                    var dstProp = d.GetExpressionValue(ct => ct.Rights);

                    var hasCorrectRights = true;

                    foreach (var srcRight in s.Rights)
                    {
                        var srcPermission = (PermissionKind)Enum.Parse(typeof(PermissionKind), srcRight);

                        var tmpRight = d.Rights.Has(srcPermission);

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



            var supportsLocalization = ReflectionUtils.HasProperties(spObject, new[]
            {
                "TitleResource", "DescriptionResource", "CommandUIExtensionResource"
            });

            if (supportsLocalization)
            {
                if (definition.TitleResource.Any())
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.TitleResource);
                        var isValid = true;

                        foreach (var userResource in s.TitleResource)
                        {
                            var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                            var resourceObject = ReflectionUtils.GetPropertyValue(spObject, "TitleResource");

                            var value = ReflectionUtils.GetMethod(resourceObject, "GetValueForUICulture")
                                                    .Invoke(resourceObject, new[] { culture.Name }) as ClientResult<string>;

                            context.ExecuteQuery();

                            isValid = userResource.Value == value.Value;

                            if (!isValid)
                                break;
                        }

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = null,
                            IsValid = isValid
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.TitleResource, "TitleResource is NULL or empty. Skipping.");
                }

                if (definition.DescriptionResource.Any())
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.DescriptionResource);
                        var isValid = true;

                        foreach (var userResource in s.DescriptionResource)
                        {
                            var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                            var resourceObject = ReflectionUtils.GetPropertyValue(spObject, "DescriptionResource");

                            var value = ReflectionUtils.GetMethod(resourceObject, "GetValueForUICulture")
                                                       .Invoke(resourceObject, new[] { culture.Name }) as ClientResult<string>;

                            context.ExecuteQuery();

                            isValid = userResource.Value == value.Value;

                            if (!isValid)
                                break;
                        }

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = null,
                            IsValid = isValid
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.DescriptionResource, "DescriptionResource is NULL or empty. Skipping.");
                }

                if (definition.CommandUIExtensionResource.Any())
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(def => def.CommandUIExtensionResource);
                        var isValid = true;

                        foreach (var userResource in s.CommandUIExtensionResource)
                        {
                            var culture = LocalizationService.GetUserResourceCultureInfo(userResource);
                            var resourceObject = ReflectionUtils.GetPropertyValue(spObject, "CommandUIExtensionResource");

                            var value = ReflectionUtils.GetMethod(resourceObject, "GetValueForUICulture")
                                                       .Invoke(resourceObject, new[] { culture.Name }) as ClientResult<string>;

                            context.ExecuteQuery();

                            isValid = GetCommandUIString(userResource.Value) == GetCommandUIString(value.Value);

                            if (!isValid)
                                break;
                        }

                        return new PropertyValidationResult
                        {
                            Tag = p.Tag,
                            Src = srcProp,
                            Dst = null,
                            IsValid = isValid
                        };
                    });
                }
                else
                {
                    assert.SkipProperty(m => m.CommandUIExtensionResource, "DescriptionResource is NULL or empty. Skipping.");
                }

            }
            else
            {
                TraceService.Critical((int)LogEventId.ModelProvisionCoreCall,
                      "CSOM runtime doesn't have Web.TitleResource and Web.DescriptionResource() methods support. Skipping validation.");

                assert.SkipProperty(m => m.TitleResource, "TitleResource is null or empty. Skipping.");
                assert.SkipProperty(m => m.DescriptionResource, "DescriptionResource is null or empty. Skipping.");
                assert.SkipProperty(m => m.CommandUIExtensionResource, "CommandUIExtensionResource is null or empty. Skipping.");
            }
        }

        protected string GetCommandUIString(string value)
        {
            return value.Replace(" ", "")
                .Replace(Environment.NewLine, "")
                .Replace("\n", "");
        }
    }

    internal static class SPUserActionHelpers
    {
        public static string GetRegistrationType(this UserCustomAction action)
        {
            return action.RegistrationType.ToString();
        }
    }
}
