using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Taxonomy;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.Taxonomy;
using SPMeta2.SSOM.Standard.ModelHosts;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Taxonomy
{
    public class TaxonomyTermDefinitionValidator : TaxonomyTermModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var termSetModelHost = modelHost.WithAssertAndCast<TermSetModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TaxonomyTermDefinition>("model", value => value.RequireNotNull());

            var spObject = FindTermInTermSet(termSetModelHost.HostTermSet, definition);

            var assert = ServiceFactory.AssertService
                           .NewAssert(definition, spObject)
                                 .ShouldNotBeNull(spObject)
                //.ShouldBeEqual(m => m.Name, o => o.Name)
                                 .ShouldBeEqual(m => m.Description, o => o.GetDescription());

            assert.SkipProperty(m => m.LCID, "Can't get LCID withon OM. Should be set while provision.");

            if (!string.IsNullOrEmpty(definition.CustomSortOrder))
            {
                assert.ShouldBeEqual(m => m.CustomSortOrder, o => o.CustomSortOrder);
            }
            else
            {
                assert.SkipProperty(m => m.CustomSortOrder, "CustomSortOrder is null. CustomSortOrder property.");
            }

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
}
