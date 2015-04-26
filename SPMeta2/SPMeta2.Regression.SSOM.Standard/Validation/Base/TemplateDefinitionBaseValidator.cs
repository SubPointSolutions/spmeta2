using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Regression.SSOM.Standard.Validation.DisplayTemplates;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.SSOM.Standard.ModelHandlers.Base;
using SPMeta2.SSOM.Standard.ModelHandlers.DisplayTemplates;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;
using System.Collections;

namespace SPMeta2.Regression.SSOM.Standard.Validation.Base
{
    public abstract class ItemControlTemplateDefinitionBaseValidator : TemplateDefinitionBaseValidator
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost",
                value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<ItemControlTemplateDefinitionBase>("model",
                value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject);


            #region preview field

            if (!string.IsNullOrEmpty(definition.PreviewURL))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = false;
                    var srcProp = s.GetExpressionValue(m => m.PreviewURL);

                    var previewValue = d.GetPreviewURL();
                    isValid = (previewValue != null) &&
                              (d.GetPreviewURL().Url == s.PreviewURL);

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
                assert.SkipProperty(m => m.PreviewURL, "PreviewURL is NULL. Skipping");
            }

            if (!string.IsNullOrEmpty(definition.PreviewDescription))
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var isValid = false;
                    var srcProp = s.GetExpressionValue(m => m.PreviewDescription);

                    var previewValue = d.GetPreviewURL();
                    isValid = (previewValue != null) &&
                              (d.GetPreviewURL().Description == s.PreviewDescription);

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
                assert.SkipProperty(m => m.PreviewDescription, "PreviewDescription is NULL. Skipping");
            }

            #endregion

            #region TargetControlTypes

            if (definition.TargetControlTypes.Count > 0)
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var srcProp = s.GetExpressionValue(m => m.TargetControlTypes);
                    var isValid = true;

                    var targetControlTypeValue = new SPFieldMultiChoiceValue(d["TargetControlType"].ToString());
                    var targetControlTypeValues = new List<string>();

                    for (var i = 0; i < targetControlTypeValue.Count; i++)
                        targetControlTypeValues.Add(targetControlTypeValue[i].ToUpper());

                    foreach (var v in s.TargetControlTypes)
                    {
                        if (!targetControlTypeValues.Contains(v.ToUpper()))
                            isValid = false;
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
                assert.SkipProperty(m => m.TargetControlTypes, "TargetControlTypes count is 0. Skipping");
            }

            #endregion
        }

        public override string FileExtension
        {
            get { return string.Empty; }
            set { }
        }

        protected override void MapProperties(object modelHost, Hashtable fileProperties,
            ContentPageDefinitionBase definition)
        {

        }

    }

    public abstract class TemplateDefinitionBaseValidator : TemplateModelHandlerBase
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost",
                value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<TemplateDefinitionBase>("model",
                value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;

            var spObject = GetCurrentObject(folder, definition);

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldNotBeNull(spObject)
                .ShouldBeEqual(m => m.Title, o => o.Title)
                .ShouldBeEqual(m => m.FileName, o => o.Name)
                .ShouldBeEqual(m => m.HiddenTemplate, o => o.GetHiddenTemplate());

            if (!string.IsNullOrEmpty(definition.Description))
                assert.ShouldBeEqual(m => m.Description, o => o.GetMasterPageDescription());
            else
                assert.SkipProperty(m => m.Description);



            #region preview field

            #endregion

            #region TargetControlTypes



            #endregion
        }

        public override string FileExtension
        {
            get { return string.Empty; }
            set { }
        }

        protected override void MapProperties(object modelHost, Hashtable fileProperties,
            ContentPageDefinitionBase definition)
        {

        }
    }

}
