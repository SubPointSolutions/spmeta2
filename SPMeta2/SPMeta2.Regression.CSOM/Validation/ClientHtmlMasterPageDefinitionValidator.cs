using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Standard.Definitions;
using SPMeta2.Utils;
using System.Collections.Generic;
using System.Linq;
using SPMeta2.Syntax.Default.Utils;
using System.Text;
using SPMeta2.CSOM.Extensions;
using SPMeta2.Regression.CSOM.Extensions;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientHtmlMasterPageDefinitionValidator : HtmlMasterPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentListFolder;
            var definition = model.WithAssertAndCast<HtmlMasterPageDefinition>("model", value => value.RequireNotNull());

            var spObject = FindPage(folderModelHost.CurrentList, folder, definition);
            var spFile = FindPageFile(folderModelHost.CurrentList, folder, definition);

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spObject, o => o.DisplayName);

            context.Load(spFile);
            context.Load(spFile, o => o.ServerRelativeUrl);

            context.ExecuteQueryWithTrace();

            var stringCustomContentType = string.Empty;

            if (!string.IsNullOrEmpty(definition.ContentTypeName)
                || !string.IsNullOrEmpty(definition.ContentTypeId))
            {
                if (!string.IsNullOrEmpty(definition.ContentTypeName))
                {
                    stringCustomContentType = ContentTypeLookupService
                                                    .LookupContentTypeByName(folderModelHost.CurrentList, definition.ContentTypeName)
                                                    .Name;
                }
            }


            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.GetFileName())
                                             .ShouldBeEqual(m => m.DefaultCSSFile, o => o.GetDefaultCSSFile())
                                             .ShouldBeEqual(m => m.Description, o => o.GetMasterPageDescription())
                                             .ShouldBeEqual(m => m.Title, o => o.GetTitle());

            // check that there is a .master page with the same name in the library
            var associatedMasterPageName = definition.FileName.ToLower().EndsWith(".html")
                ? definition.FileName.ToLower().Replace(".html", ".master")
                : definition.FileName + ".master";

            var associatedMasterPage = FindPageByName(folderModelHost.CurrentList, folder, associatedMasterPageName);
            assert.ShouldNotBeNull(associatedMasterPage);

            if (definition.UIVersion.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.UIVersion);
                    var dstPropValue = d.GetUIVersion();

                    var isValid = true;

                    foreach (var v in s.UIVersion)
                    {
                        if (!dstPropValue.Contains(v))
                        {
                            isValid = false;
                            break;
                        }
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
                assert.SkipProperty(d => d.UIVersion, "UIVersion.Count is 0. Skipping");
            }


            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = false;

                byte[] dstContent = null;

                folderModelHost.HostClientContext.ExecuteQueryWithTrace();

                using (var stream = File.OpenBinaryDirect(folderModelHost.HostClientContext, spFile.ServerRelativeUrl).Stream)
                    dstContent = ModuleFileUtils.ReadFully(stream);

                var srcStringContent = Encoding.UTF8.GetString(s.Content);
                var dstStringContent = Encoding.UTF8.GetString(dstContent);

                // same or greater, length
                // too tricky to compare the content
                //isContentValid = dstStringContent.Contains(srcStringContent);
                isContentValid = dstStringContent.Length >= srcStringContent.Length;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    // Dst = dstProp,
                    IsValid = isContentValid
                };
            });


            if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                assert.ShouldBeEqual(m => m.ContentTypeName, o => o.GetContentTypeName());
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeName, "ContentTypeName is NULL. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.ContentTypeId))
            {
                // TODO
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeId, "ContentTypeId is null or empty. Skipping.");
            }

            if (!string.IsNullOrEmpty(definition.ContentTypeName))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(def => def.ContentTypeName);
                    var currentContentTypeName = d.ContentType.Name;

                    var isValis = stringCustomContentType == currentContentTypeName;

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        Dst = null,
                        IsValid = isValis
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.ContentTypeName, "ContentTypeName is null or empty. Skipping.");
            }

            if (definition.DefaultValues.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    foreach (var srcValue in s.DefaultValues)
                    {
                        // big TODO here for == != 

                        if (!string.IsNullOrEmpty(srcValue.FieldName))
                        {
                            if (d[srcValue.FieldName].ToString() != srcValue.Value.ToString())
                                isValid = false;
                        }

                        if (!isValid)
                            break;
                    }

                    var srcProp = s.GetExpressionValue(def => def.DefaultValues);

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
                assert.SkipProperty(m => m.DefaultValues, "DefaultValues.Count == 0. Skipping.");
            }

            if (definition.Values.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = true;

                    foreach (var srcValue in s.Values)
                    {
                        // big TODO here for == != 

                        if (!string.IsNullOrEmpty(srcValue.FieldName))
                        {
                            if (d[srcValue.FieldName].ToString() != srcValue.Value.ToString())
                                isValid = false;
                        }

                        if (!isValid)
                            break;
                    }

                    var srcProp = s.GetExpressionValue(def => def.Values);

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
                assert.SkipProperty(m => m.Values, "Values.Count == 0. Skipping.");
            }
        }

        #endregion
    }
}
