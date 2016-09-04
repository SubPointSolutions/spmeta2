using System;
using Microsoft.SharePoint.Publishing.WebControls;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.Extensions;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;
using SPMeta2.Containers.Assertion;
using SPMeta2.SSOM.ModelHandlers.Fields;
using Microsoft.SharePoint.Utilities;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Webparts
{
    public class ContentByQueryWebPartDefinitionValidator : WebPartDefinitionValidator
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContentByQueryWebPartDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            // base validation
            base.DeployModel(modelHost, model);

            // content editor specific validation

            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var typedDefinition = model.WithAssertAndCast<ContentByQueryWebPartDefinition>("model", value => value.RequireNotNull());

            CurrentHost = host;

            //var item = host.PageListItem;

            WebPartExtensions.WithExistingWebPart(host.HostFile, typedDefinition, (spWebPartManager, spObject) =>
            {
                var typedWebPart = spObject as ContentByQueryWebPart;

                var assert = ServiceFactory.AssertService
                                           .NewAssert(typedDefinition, typedWebPart)
                                           .ShouldNotBeNull(typedWebPart);

                if (!string.IsNullOrEmpty(typedDefinition.ContentTypeBeginsWithId))
                    assert.ShouldBeEqual(m => m.ContentTypeBeginsWithId, o => o.ContentTypeBeginsWithId);
                else
                    assert.SkipProperty(m => m.ContentTypeBeginsWithId, "ContentTypeBeginsWithId is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.DataMappings))
                    assert.ShouldBeEqual(m => m.DataMappings, o => o.DataMappings);
                else
                    assert.SkipProperty(m => m.DataMappings, "DataMappings is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.DataMappingViewFields))
                    assert.ShouldBeEqual(m => m.DataMappingViewFields, o => o.DataMappingViewFields);
                else
                    assert.SkipProperty(m => m.DataMappingViewFields, "DataMappingViewFields is null or empty, skipping.");

                // filter display value 1-2-3
                if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue1))
                    assert.ShouldBeEqual(m => m.FilterDisplayValue1, o => o.FilterDisplayValue1);
                else
                    assert.SkipProperty(m => m.FilterDisplayValue1, "FilterDisplayValue1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue2))
                    assert.ShouldBeEqual(m => m.FilterDisplayValue2, o => o.FilterDisplayValue2);
                else
                    assert.SkipProperty(m => m.FilterDisplayValue2, "FilterDisplayValue2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterDisplayValue3))
                    assert.ShouldBeEqual(m => m.FilterDisplayValue3, o => o.FilterDisplayValue3);
                else
                    assert.SkipProperty(m => m.FilterDisplayValue3, "FilterDisplayValue3 is null or empty, skipping.");

                // operators 1-2-3
                if (!string.IsNullOrEmpty(typedDefinition.FilterOperator1))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.FilterOperator1);
                        var isValid = false;

                        isValid = s.FilterOperator1.ToLower() == d.FilterOperator1.ToString().ToLower();

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

                        isValid = s.FilterOperator2.ToLower() == d.FilterOperator2.ToString().ToLower();

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

                        isValid = s.FilterOperator3.ToLower() == d.FilterOperator3.ToString().ToLower();

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

                // type 1-2-3
                if (!string.IsNullOrEmpty(typedDefinition.FilterType1))
                    assert.ShouldBeEqual(m => m.FilterType1, o => o.FilterType1);
                else
                    assert.SkipProperty(m => m.FilterType1, "FilterType1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterType2))
                    assert.ShouldBeEqual(m => m.FilterType2, o => o.FilterType2);
                else
                    assert.SkipProperty(m => m.FilterType2, "FilterType2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterType3))
                    assert.ShouldBeEqual(m => m.FilterType3, o => o.FilterType3);
                else
                    assert.SkipProperty(m => m.FilterType3, "FilterType3 is null or empty, skipping.");

                // filter values 1-2-3
                if (!string.IsNullOrEmpty(typedDefinition.FilterValue1))
                    assert.ShouldBeEqual(m => m.FilterValue1, o => o.FilterValue1);
                else
                    assert.SkipProperty(m => m.FilterValue1, "FilterValue1 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterValue2))
                    assert.ShouldBeEqual(m => m.FilterValue2, o => o.FilterValue2);
                else
                    assert.SkipProperty(m => m.FilterValue2, "FilterValue2 is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.FilterValue3))
                    assert.ShouldBeEqual(m => m.FilterValue3, o => o.FilterValue3);
                else
                    assert.SkipProperty(m => m.FilterValue3, "FilterValue3 is null or empty, skipping.");

                // sorting

                if (!string.IsNullOrEmpty(typedDefinition.SortBy))
                    assert.ShouldBeEqual(m => m.SortBy, o => o.SortBy);
                else
                    assert.SkipProperty(m => m.SortBy, "SortBy is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.SortByFieldType))
                    assert.ShouldBeEqual(m => m.SortByFieldType, o => o.SortByFieldType);
                else
                    assert.SkipProperty(m => m.SortByFieldType, "SortByFieldType is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.SortByDirection))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.SortByDirection);
                        var isValid = false;

                        isValid = s.SortByDirection.ToLower() == d.SortByDirection.ToString().ToLower();

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

                // xslt
                if (!string.IsNullOrEmpty(typedDefinition.GroupStyle))
                    assert.ShouldBeEqual(m => m.GroupStyle, o => o.GroupStyle);
                else
                    assert.SkipProperty(m => m.GroupStyle, "GroupStyle is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.ItemStyle))
                    assert.ShouldBeEqual(m => m.ItemStyle, o => o.ItemStyle);
                else
                    assert.SkipProperty(m => m.ItemStyle, "ItemStyle is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.ItemXslLink))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.ItemXslLink, "ItemXslLink is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.MainXslLink))
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.MainXslLink, "MainXslLink is null or empty, skipping.");


                // list name/url/id
                if (!string.IsNullOrEmpty(typedDefinition.ListName))
                {
                    // TODO
                }
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

                        using (var targetWeb = webLookup.GetTargetWeb(CurrentHost.HostFile.Web.Site,
                                                           typedDefinition.WebUrl,
                                                           typedDefinition.WebId))
                        {
                            var listUrl = SPUrlUtility.CombineUrl(targetWeb.ServerRelativeUrl, typedDefinition.ListUrl);
                            var list = targetWeb.GetList(listUrl);

                            isValid = typedWebPart.ListGuid == list.ID.ToString("D");
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
                    assert.SkipProperty(m => m.ListUrl, "ListUrl is null or empty, skipping.");

                if (typedDefinition.ListGuid.HasValue)
                {
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.ListGuid, "ListGuid is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.WebUrl))
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.WebUrl);
                        var isValid = false;

                        isValid = s.WebUrl == d.WebUrl;

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
                    // TODO
                }
                else
                    assert.SkipProperty(m => m.WebId, "WebId is null or empty, skipping.");

                // misc

                if (typedDefinition.ItemLimit.HasValue)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ItemLimit);
                        var isValid = false;

                        isValid = s.ItemLimit == d.ItemLimit;

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
                    assert.SkipProperty(m => m.ItemLimit, "ItemLimit is null or empty, skipping.");

                if (typedDefinition.PlayMediaInBrowser.HasValue)
                {
                    assert.ShouldBeEqual(m => m.PlayMediaInBrowser, o => o.PlayMediaInBrowser);
                }
                else
                    assert.SkipProperty(m => m.PlayMediaInBrowser, "PlayMediaInBrowser is null or empty, skipping.");

                if (typedDefinition.ShowUntargetedItems.HasValue)
                {
                    assert.ShouldBeEqual(m => m.ShowUntargetedItems, o => o.ShowUntargetedItems);
                }
                else
                    assert.SkipProperty(m => m.ShowUntargetedItems, "ShowUntargetedItems is null or empty, skipping.");

                if (typedDefinition.UseCopyUtil.HasValue)
                {
                    assert.ShouldBeEqual(m => m.UseCopyUtil, o => o.UseCopyUtil);
                }
                else
                    assert.SkipProperty(m => m.UseCopyUtil, "UseCopyUtil is null or empty, skipping.");

                if (typedDefinition.ServerTemplate.HasValue)
                {
                    assert.ShouldBeEqual((p, s, d) =>
                    {
                        var srcProp = s.GetExpressionValue(m => m.ServerTemplate);
                        var isValid = false;

                        isValid = s.ServerTemplate == ConvertUtils.ToInt(d.ServerTemplate);

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
                    assert.SkipProperty(m => m.ServerTemplate, "ServerTemplate is null or empty, skipping.");

                if (!string.IsNullOrEmpty(typedDefinition.GroupBy))
                    assert.ShouldBeEqual(m => m.GroupBy, o => o.GroupBy);
                else
                    assert.SkipProperty(m => m.GroupBy, "GroupBy is null or empty, skipping.");

                if (typedDefinition.DisplayColumns.HasValue)
                    assert.ShouldBeEqual(m => m.DisplayColumns, o => o.DisplayColumns);
                else
                    assert.SkipProperty(m => m.DisplayColumns, "DisplayColumns is null or empty, skipping.");

            });
        }

        #endregion
    }
}
