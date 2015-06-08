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
using SPMeta2.CSOM.ModelHandlers.Base;
using SPMeta2.Standard.Definitions.Base;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientMasterPagePreviewDefinitionValidator : ContentFileModelHandlerBase
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost",
                value => value.RequireNotNull());

            var folder = folderModelHost.CurrentLibraryFolder;
            var definition = model.WithAssertAndCast<MasterPagePreviewDefinition>("model", value => value.RequireNotNull());

            var spFile = GetItemFile(folderModelHost.CurrentList, folder, definition.FileName);
            var spObject = spFile.ListItemAllFields;

            var context = spObject.Context;

            context.Load(spObject);
            context.Load(spFile, f => f.ServerRelativeUrl);
            context.ExecuteQuery();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Title, o => o.GetTitle())
                .ShouldBeEqual(m => m.FileName, o => o.GetFileName());

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = false;

                byte[] dstContent = null;

                using (var stream = File.OpenBinaryDirect(folderModelHost.HostClientContext, spFile.ServerRelativeUrl).Stream)
                    dstContent = ModuleFileUtils.ReadFully(stream);

                isContentValid = dstContent.SequenceEqual(definition.Content);

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = srcProp,
                    // Dst = dstProp,
                    IsValid = isContentValid
                };
            });

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

            #endregion
        }

        public override string FileExtension
        {
            get { return "preview"; }
            set
            {

            }
        }

        protected override void MapProperties(object modelHost, ListItem item, ContentPageDefinitionBase definition)
        {

        }

        public override Type TargetType
        {
            get { return typeof(MasterPagePreviewDefinition); }
        }
    }
}
