using Microsoft.SharePoint.Client.Taxonomy;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.CSOM.Standard.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Exceptions;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Taxonomy
{
    public class TaxonomyTermDefinitionValidator : TaxonomyTermModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<TaxonomyTermDefinition>("model", value => value.RequireNotNull());

            Term spObject = null;

            if (modelHost is TermModelHost)
            {
                var context = (modelHost as TermModelHost).HostClientContext;
                spObject = FindTermInTerm((modelHost as TermModelHost).HostTerm, definition);

                if (spObject == null && IsSharePointOnlineContext(context))
                {
                    TryRetryService.TryWithRetry(() =>
                    {
                        spObject = FindTermInTerm((modelHost as TermModelHost).HostTerm, definition);
                        return spObject != null;
                    });
                }
            }
            else if (modelHost is TermSetModelHost)
            {
                var context = (modelHost as TermSetModelHost).HostClientContext;
                spObject = FindTermInTermSet((modelHost as TermSetModelHost).HostTermSet, definition);

                if (spObject == null && IsSharePointOnlineContext(context))
                {
                    TryRetryService.TryWithRetry(() =>
                    {
                        spObject = FindTermInTermSet((modelHost as TermSetModelHost).HostTermSet, definition);
                        return spObject != null;
                    });
                }
            }
            else
            {
                throw new SPMeta2UnsupportedModelHostException(string.Format("Model host of type: [{0}] is not supported", modelHost.GetType()));
            }

            TermExtensions.CurrentLCID = definition.LCID;

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                //.ShouldBeEqual(m => m.Name, o => o.Name)
                                 .ShouldBeEqual(m => m.Description, o => o.GetDefaultLCIDDescription());

            assert.SkipProperty(m => m.LCID, "LCID is not accessible from OM. Should be alright while provision.");

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Name);
                var dstProp = d.GetExpressionValue(m => m.Name);

                var isValid = NormalizeTermName(s.Name) == d.Name;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });

            if (!string.IsNullOrEmpty(definition.CustomSortOrder))
                assert.ShouldBeEqual(m => m.CustomSortOrder, o => o.CustomSortOrder);
            else
                assert.SkipProperty(m => m.CustomSortOrder);

            if (definition.Id.HasValue)
            {
                assert.ShouldBeEqual(m => m.Id, o => o.Id);
            }
            else
            {
                assert.SkipProperty(m => m.Id, "Id is null. Skipping property.");
            }

            if (definition.IsAvailableForTagging.HasValue)
            {
                assert.ShouldBeEqual(m => m.IsAvailableForTagging, o => o.IsAvailableForTagging);
            }
            else
            {
                assert.SkipProperty(m => m.IsAvailableForTagging, "IsAvailableForTagging is null. Skipping property.");
            }

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.CustomProperties);

                var isValid = true;

                // missed props, or too much
                // should be equal on the first provision
                if (s.CustomProperties.Count != d.CustomProperties.Count)
                {
                    isValid = false;
                }

                // per prop
                foreach (var customProp in s.CustomProperties)
                {
                    if (!d.CustomProperties.ContainsKey(customProp.Name))
                    {
                        isValid = false;
                        break;
                    }

                    if (d.CustomProperties[customProp.Name] != customProp.Value)
                    {
                        isValid = false;
                        break;
                    }
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    // Dst = dstProp,
                    IsValid = isValid
                };
            });


            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.LocalCustomProperties);

                var isValid = true;

                // missed props, or too much
                // should be equal on the first provision
                if (s.LocalCustomProperties.Count != d.LocalCustomProperties.Count)
                {
                    isValid = false;
                }

                // per prop
                foreach (var customProp in s.LocalCustomProperties)
                {
                    if (!d.LocalCustomProperties.ContainsKey(customProp.Name))
                    {
                        isValid = false;
                        break;
                    }

                    if (d.LocalCustomProperties[customProp.Name] != customProp.Value)
                    {
                        isValid = false;
                        break;
                    }
                }

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    // Dst = dstProp,
                    IsValid = isValid
                };
            });
        }
    }

    internal static class TermExtensions
    {
        public static int CurrentLCID { get; set; }

        public static string GetDefaultLCIDDescription(this Term term)
        {
            var context = term.Context;

            var resultValue = term.GetDescription(CurrentLCID);
            context.ExecuteQueryWithTrace();

            return resultValue.Value;
        }
    }
}
