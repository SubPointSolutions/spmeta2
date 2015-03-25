using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class LookupFieldDefinitionValidator : ClientFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(LookupFieldDefinition);
            }
        }

        protected override void CustomFieldTypeValidation(AssertPair<FieldDefinition, Field> assert, Field spObject, FieldDefinition definition)
        {
            var typedObject = spObject.Context.CastTo<FieldLookup>(spObject);
            var typedDefinition = definition.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            // https://github.com/SubPointSolutions/spmeta2/issues/310
            // AllowMultipleValues - TRUE - LookupMulti
            // AllowMultipleValues - FALSE - Lookup
            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.FieldType);
                var dstProp = d.GetExpressionValue(m => d.TypeAsString);

                var isValid = typedDefinition.AllowMultipleValues
                    ? typedObject.TypeAsString == "LookupMulti"
                    : typedObject.TypeAsString == "Lookup";

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    Dst = dstProp,
                    IsValid = isValid
                };
            });
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            HostList = ExtractListFromHost(modelHost);
            HostSite = ExtractSiteFromHost(modelHost);

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            ValidateField(assert, spObject, definition);

            var typedField = spObject.Context.CastTo<FieldLookup>(spObject);
            var typedDefinition = model.WithAssertAndCast<LookupFieldDefinition>("model", value => value.RequireNotNull());

            var typedFieldAssert = ServiceFactory.AssertService.NewAssert(model, typedDefinition, typedField);

            typedFieldAssert.ShouldBeEqual(m => m.AllowMultipleValues, o => o.AllowMultipleValues);

            if (typedDefinition.LookupWebId.HasValue)
            {
                typedFieldAssert.ShouldBeEqual(m => m.LookupWebId, o => o.LookupWebId);
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.LookupWebId, "LookupWebId is NULL. Skipping.");
            }


            if (!string.IsNullOrEmpty(typedDefinition.LookupListTitle))
            {
                var site = HostSite;
                var context = site.Context;

                var web = typedDefinition.LookupWebId.HasValue
                    ? site.OpenWebById(typedDefinition.LookupWebId.Value)
                    : site.RootWeb;

                context.Load(web);
                context.ExecuteQuery();

                var list = web.Lists.GetByTitle(typedDefinition.LookupListTitle);

                context.Load(list);
                context.ExecuteQuery();

                typedFieldAssert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.LookupListTitle);

                    var isValid = list.Id == new Guid(typedField.LookupList);

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
                typedFieldAssert.SkipProperty(m => m.LookupListTitle, "LookupListTitle is NULL. Skipping.");
            }

            if (!string.IsNullOrEmpty(typedDefinition.LookupListUrl))
            {
                var site = HostSite;
                var context = site.Context;

                var web = typedDefinition.LookupWebId.HasValue
                    ? site.OpenWebById(typedDefinition.LookupWebId.Value)
                    : site.RootWeb;

                context.Load(web);
                context.ExecuteQuery();

                var list = web.QueryAndGetListByUrl(UrlUtility.CombineUrl(web.ServerRelativeUrl, typedDefinition.LookupListUrl));

                context.Load(list);
                context.ExecuteQuery();

                typedFieldAssert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.LookupListUrl);

                    var isValid = list.Id == new Guid(typedField.LookupList);

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
                typedFieldAssert.SkipProperty(m => m.LookupListUrl, "LookupListUrl is NULL. Skipping.");
            }

            if (!string.IsNullOrEmpty(typedDefinition.LookupList))
            {
                if (typedDefinition.LookupList.ToUpper() == "USERINFO")
                {
                    typedFieldAssert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.LookupList);

                        var site = HostSite;
                        var context = site.Context;

                        var userInfoList = site.RootWeb.SiteUserInfoList;
                        context.Load(userInfoList);
                        context.ExecuteQuery();

                        var isValid = userInfoList.Id == new Guid(typedField.LookupList);

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
                    typedFieldAssert.ShouldBeEqual(m => m.LookupList, o => o.LookupList);
                }
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.LookupList, "LookupList is NULL. Skipping.");
            }

            if (!string.IsNullOrEmpty(typedDefinition.LookupField))
            {
                typedFieldAssert.ShouldBeEqual(m => m.LookupField, o => o.LookupField);
            }
            else
            {
                typedFieldAssert.SkipProperty(m => m.LookupField, "LookupField is NULL. Skipping.");
            }
        }
    }
}
