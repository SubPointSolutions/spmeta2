using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Standard.Validation.Base;
using SPMeta2.Regression.SSOM.Validation;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Standard.Definitions.DisplayTemplates;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Standard.Validation.DisplayTemplates
{
    public class ItemDisplayTemplateDefinitionValidator : ItemControlTemplateDefinitionBaseValidator
    {
        public override string FileExtension
        {
            get { return "html"; }
            set { }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ItemDisplayTemplateDefinition>("model", value => value.RequireNotNull());
            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);
            var file = spObject.File;

            var assert = ServiceFactory.AssertService
                                  .NewAssert(definition, spObject);

            if (!string.IsNullOrEmpty(definition.ManagedPropertyMappings))
                assert.ShouldBeEqual(m => m.ManagedPropertyMappings, o => o.GetManagedPropertyMapping());
            else
                assert.SkipProperty(m => m.ManagedPropertyMappings);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var srcProp = s.GetExpressionValue(m => m.Content);
                //var dstProp = d.GetExpressionValue(ct => ct.GetId());

                var isContentValid = false;

                var srcStringContent = Encoding.UTF8.GetString(s.Content);
                var dstStringContent = Encoding.UTF8.GetString(file.GetContent());

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

        public override System.Type TargetType
        {
            get { return typeof(ItemDisplayTemplateDefinition); }
        }
    }
}
