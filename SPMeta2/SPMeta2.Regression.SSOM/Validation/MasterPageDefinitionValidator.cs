using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Collections.Generic;
using System.Text;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Regression.SSOM.Validation
{
    public class MasterPageDefinitionValidator : MasterPageModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<MasterPageDefinition>("model", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);

            var assert = ServiceFactory.AssertService
                                       .NewAssert(definition, spObject)
                                             .ShouldNotBeNull(spObject)
                                             .ShouldBeEqual(m => m.FileName, o => o.Name)
                                             .ShouldBeEqual(m => m.Description, o => o.GetMasterPageDescription())
                                             .ShouldBeEqual(m => m.DefaultCSSFile, o => o.GetDefaultCSSFile())
                                             .ShouldBeEqual(m => m.Title, o => o.Title);

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

                var srcStringContent = Encoding.UTF8.GetString(s.Content);
                var dstStringContent = Encoding.UTF8.GetString(d.GetContent());

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
    }

    public static class SPListItemHelper
    {
        public static List<string> GetUIVersion(this SPListItem item)
        {
            var v = new SPFieldMultiChoiceValue(item["UIVersion"] as string);
            var result = new List<string>();

            for (var i = 0; i < v.Count; i++)
                result.Add(v[i]);

            return result;
        }

        public static string GetMasterPageDescription(this SPListItem item)
        {
            return item["MasterPageDescription"] as string;
        }

        public static string GetDefaultCSSFile(this SPListItem item)
        {
            return item["DefaultCssFile"] as string;
        }
    }
}
