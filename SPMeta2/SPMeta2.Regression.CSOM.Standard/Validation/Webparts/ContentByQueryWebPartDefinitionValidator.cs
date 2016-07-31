using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;
using SPMeta2.Regression.CSOM.Validation;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers.Fields;

namespace SPMeta2.Regression.CSOM.Standard.Validation.Webparts
{
    public class ContentByQueryWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        public override Type TargetType
        {
            get { return typeof(ContentByQueryWebPartDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listItemModelHost = modelHost.WithAssertAndCast<ListItemModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<ContentByQueryWebPartDefinition>("model", value => value.RequireNotNull());

            var pageItem = listItemModelHost.HostListItem;

            WithExistingWebPart(listItemModelHost.HostFile, typedDefinition, spObject =>
            {
                var assert = ServiceFactory.AssertService
                                           .NewAssert(model, typedDefinition, spObject)
                                                 .ShouldNotBeNull(spObject);

                if (!string.IsNullOrEmpty(typedDefinition.ContentTypeBeginsWithId))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ContentTypeBeginsWithId, "ContentTypeBeginsWithId is null or empty, skipping.");

                // mappings
                if (!string.IsNullOrEmpty(typedDefinition.DataMappings))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.DataMappings);
                        var isValid = false;

                        isValid = s.DataMappings == CurrentWebPartXml.GetProperty("DataMappings");

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
                    assert.SkipProperty(m => m.DataMappings, "DataMappings is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.DataMappingViewFields))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.DataMappingViewFields);
                        var isValid = false;

                        isValid = s.DataMappingViewFields == CurrentWebPartXml.GetProperty("DataMappingViewFields");

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
                    assert.SkipProperty(m => m.DataMappingViewFields, "DataMappingViewFields is null or empty, skipping.");

                // filter display values

                if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue1))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterDisplayValue1);
                        var isValid = false;

                        isValid = s.FilterDisplayValue1 == CurrentWebPartXml.GetProperty("FilterDisplayValue1");

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
                    assert.SkipProperty(m => m.FilterDisplayValue1, "FilterDisplayValue1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue2))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterDisplayValue2);
                        var isValid = false;

                        isValid = s.FilterDisplayValue2 == CurrentWebPartXml.GetProperty("FilterDisplayValue2");

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
                    assert.SkipProperty(m => m.FilterDisplayValue2, "FilterDisplayValue2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue3))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterDisplayValue3);
                        var isValid = false;

                        isValid = s.FilterDisplayValue3 == CurrentWebPartXml.GetProperty("FilterDisplayValue3");

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
                    assert.SkipProperty(m => m.FilterDisplayValue3, "FilterDisplayValue3 is null or empty, skipping.");

                // filter operator

                if (!string.IsNullOrEmpty(typedDefinition.FilterOperator1))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterOperator1);
                        var isValid = false;

                        isValid = s.FilterOperator1 == CurrentWebPartXml.GetProperty("FilterOperator1");

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
                    assert.SkipProperty(m => m.FilterOperator1, "FilterOperator1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterOperator2))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterOperator2);
                        var isValid = false;

                        isValid = s.FilterOperator2 == CurrentWebPartXml.GetProperty("FilterOperator2");

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
                    assert.SkipProperty(m => m.FilterOperator2, "FilterOperator2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterOperator3))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterOperator3);
                        var isValid = false;

                        isValid = s.FilterOperator3 == CurrentWebPartXml.GetProperty("FilterOperator3");

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
                    assert.SkipProperty(m => m.FilterOperator3, "FilterOperator3 is null or empty, skipping.");

                // filter types

                if (!string.IsNullOrEmpty(typedDefinition.FilterType1))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterType1);
                        var isValid = false;

                        isValid = s.FilterType1 == CurrentWebPartXml.GetProperty("FilterType1");

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
                    assert.SkipProperty(m => m.FilterType1, "FilterType1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterType2))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterType2);
                        var isValid = false;

                        isValid = s.FilterType2 == CurrentWebPartXml.GetProperty("FilterType2");

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
                    assert.SkipProperty(m => m.FilterType2, "FilterType2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterType3))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterType3);
                        var isValid = false;

                        isValid = s.FilterType3 == CurrentWebPartXml.GetProperty("FilterType3");

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
                    assert.SkipProperty(m => m.FilterType3, "FilterType3 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.SortBy))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.SortBy);
                        var isValid = false;

                        isValid = s.SortBy == CurrentWebPartXml.GetProperty("SortBy");

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
                    assert.SkipProperty(m => m.SortBy, "SortBy is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.SortByFieldType))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.SortByFieldType);
                        var isValid = false;

                        isValid = s.SortByFieldType == CurrentWebPartXml.GetProperty("SortByFieldType");

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
                    assert.SkipProperty(m => m.SortByFieldType, "SortByFieldType is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.SortByDirection))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.SortByDirection);
                        var isValid = false;

                        isValid = s.SortByDirection == CurrentWebPartXml.GetProperty("SortByDirection");

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
                    assert.SkipProperty(m => m.SortByDirection, "SortByDirection is null or empty, skipping.");

                // filter values

                if (!string.IsNullOrEmpty(typedDefinition.FilterValue1))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterValue1);
                        var isValid = false;

                        isValid = s.FilterValue1 == CurrentWebPartXml.GetProperty("FilterValue1");

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
                    assert.SkipProperty(m => m.FilterValue1, "FilterValue1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterValue2))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterValue2);
                        var isValid = false;

                        isValid = s.FilterValue2 == CurrentWebPartXml.GetProperty("FilterValue2");

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
                    assert.SkipProperty(m => m.FilterValue2, "FilterValue2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterValue3))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterValue3);
                        var isValid = false;

                        isValid = s.FilterValue3 == CurrentWebPartXml.GetProperty("FilterValue3");

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
                    assert.SkipProperty(m => m.FilterValue3, "FilterValue3 is null or empty, skipping.");

                // styles

                if (!string.IsNullOrEmpty(typedDefinition.GroupStyle))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.GroupStyle);
                        var isValid = false;

                        isValid = s.GroupStyle == CurrentWebPartXml.GetGroupStyle();

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
                    assert.SkipProperty(m => m.GroupStyle, "GroupStyle is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.ItemStyle))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ItemStyle);
                        var isValid = false;

                        isValid = s.ItemStyle == CurrentWebPartXml.GetItemStyle();

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
                    assert.SkipProperty(m => m.ItemStyle, "ItemStyle is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.ItemXslLink))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ItemXslLink, "ItemXslLink is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.MainXslLink))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.MainXslLink, "MainXslLink is null or empty, skipping.");

                // list bindings

                if (!string.IsNullOrEmpty(typedDefinition.WebUrl))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.WebUrl);
                        var isValid = false;

                        isValid = s.WebUrl == CurrentWebPartXml.GetWebUrl();

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
                    assert.SkipProperty(m => m.WebUrl, "WebUrl is null or empty, skipping.");

                if (typedDefinition.WebId.HasValue)
                {
                    throw new NotImplementedException();
                }
                else
                    assert.SkipProperty(m => m.WebId, "WebId is null or empty, skipping.");

                if (typedDefinition.ListGuid.HasGuidValue())
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ListGuid, "ListGuid is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.ListName))
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ListName, "ListName is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.ListUrl))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ListUrl);
                        var isValid = false;

                        // resolve web / url by list URL
                        // check ListId

                        var webLookup = new LookupFieldModelHandler();

                        var targetWeb = webLookup.GetTargetWeb(listItemModelHost.HostSite,
                            typedDefinition.WebUrl,
                            typedDefinition.WebId);

                        var list = targetWeb.QueryAndGetListByUrl(typedDefinition.ListUrl);
                        isValid = CurrentWebPartXml.GetListGuid() == list.Id.ToString("D");

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
                    assert.SkipProperty(m => m.ListUrl, "ListUrl is null or empty, skipping.");

                // misc

                if (typedDefinition.ServerTemplate.HasValue)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ServerTemplate);
                        var isValid = false;

                        isValid = ConvertUtils.ToInt(CurrentWebPartXml.GetProperty("ServerTemplate"))
                                        == typedDefinition.ServerTemplate;

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
                    assert.SkipProperty(m => m.ServerTemplate, "ServerTemplate is null or empty, skipping.");
                }

                if (typedDefinition.ItemLimit.HasValue)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ItemLimit);
                        var isValid = false;

                        isValid = ConvertUtils.ToInt(CurrentWebPartXml.GetProperty("ItemLimit"))
                                        == typedDefinition.ItemLimit;

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
                    assert.SkipProperty(m => m.ItemLimit, "ItemLimit is null or empty, skipping.");
                }

                if (typedDefinition.PlayMediaInBrowser.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.PlayMediaInBrowser, "PlayMediaInBrowser is null or empty, skipping.");

                if (typedDefinition.ShowUntargetedItems.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.ShowUntargetedItems, "ShowUntargetedItems is null or empty, skipping.");

                if (typedDefinition.UseCopyUtil.HasValue)
                    throw new NotImplementedException();
                else
                    assert.SkipProperty(m => m.UseCopyUtil, "UseCopyUtil is null or empty, skipping.");

            });
        }
    }
}
