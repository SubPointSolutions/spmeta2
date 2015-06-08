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

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientMasterPageDefinitionValidator : MasterPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<MasterPageDefinition>("model", value => value.RequireNotNull());

            var spObject = FindPage(folderModelHost.CurrentList, folder, definition);
            var spFile = FindPageFile(folderModelHost.CurrentList, folder, definition);

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spObject, o => o.DisplayName);

            context.Load(spFile);
            context.Load(spFile, o => o.ServerRelativeUrl);

            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.GetFileName())
                                             .ShouldBeEqual(m => m.DefaultCSSFile, o => o.GetDefaultCSSFile())
                                             .ShouldBeEqual(m => m.Description, o => o.GetMasterPageDescription())
                                             .ShouldBeEqual(m => m.Title, o => o.GetTitle());

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

                using (var stream = File.OpenBinaryDirect(folderModelHost.HostClientContext, spFile.ServerRelativeUrl).Stream)
                    dstContent = ModuleFileUtils.ReadFully(stream);

                var srcStringContent = Encoding.UTF8.GetString(s.Content);
                var dstStringContent = Encoding.UTF8.GetString(dstContent);

                isContentValid = dstStringContent.Contains(srcStringContent);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    // Dst = dstProp,
                    IsValid = isContentValid
                };
            });
        }

        #endregion
    }

    public static class SPListItemHelper
    {
        public static List<string> GetUIVersion(this ListItem item)
        {
            var result = new List<string>();

            var values = item["UIVersion"] as string[];

            if (values != null && values.Length > 0)
                result.AddRange(values);

            return result;
        }

        public static string GetTitle(this ListItem item)
        {
            return item["Title"] as string;
        }

        public static string GetContentTypeName(this ListItem item)
        {
            return item.ContentType.Name;
        }

        public static string GetDefaultCSSFile(this ListItem item)
        {
            return item["DefaultCssFile"] as string;
        }

        public static string GetFileName(this ListItem item)
        {
            return item["FileLeafRef"] as string;
        }

        public static string GetPublishingPageDescription(this ListItem item)
        {
            return item["Comments"] as string;
        }

        public static string GetPublishingPagePageLayoutFileName(this ListItem item)
        {
            var result = item["PublishingPageLayout"] as FieldUrlValue;

            if (result != null)
                return result.Url;

            return string.Empty;
        }

        public static string GetMasterPageDescription(this ListItem item)
        {
            return item["MasterPageDescription"] as string;
        }
    }
}
