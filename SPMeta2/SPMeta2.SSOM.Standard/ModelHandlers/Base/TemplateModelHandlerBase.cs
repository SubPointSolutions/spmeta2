using System.Text;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Standard.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.Standard.ModelHandlers.Base
{
    public abstract class TemplateModelHandlerBase : SSOMModelHandlerBase
    {
        #region properties

        public abstract string FileExtension { get; set; }

        #endregion

        #region methods
        protected string GetSafePageFileName(TemplateDefinitionBase page)
        {
            var fileName = page.FileName;
            if (!fileName.EndsWith("." + FileExtension)) fileName += "." + FileExtension;

            return fileName;
        }


        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;
            var templateModel = model.WithAssertAndCast<TemplateDefinitionBase>("model", value => value.RequireNotNull());

            DeployPage(modelHost, listModelHost.CurrentLibrary, folder, templateModel);
        }

        private void DeployPage(object modelHost, SPList list, SPFolder folder, TemplateDefinitionBase definition)
        {
            var web = list.ParentWeb;
            var targetPage = GetCurrentObject(folder, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = targetPage == null ? null : targetPage.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (targetPage == null)
                targetPage = CreateObject(modelHost, folder, definition);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = targetPage.File,
                ObjectType = typeof(SPFile),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            ModuleFileModelHandler.WithSafeFileOperation(list, folder,
                targetPage.Url,
                GetSafePageFileName(definition),
                Encoding.UTF8.GetBytes(PublishingPageTemplates.RedirectionPageMarkup),
                false,
                null,
                afterFile =>
                {
                    var pageItem = afterFile.Item;

                    if (definition.TargetControlTypes.Count > 0)
                    {
                        var multiChoiceValue = new SPFieldMultiChoiceValue();

                        foreach (var value in definition.TargetControlTypes)
                            multiChoiceValue.Add(value);

                        pageItem["TargetControlType"] = multiChoiceValue;
                    }

                    pageItem["Title"] = definition.Title;
                    pageItem["TemplateHidden"] = definition.HiddenTemplate;

                    if (!string.IsNullOrEmpty(definition.Description))
                        pageItem["MasterPageDescription"] = definition.Description;

                    MapProperties(modelHost, pageItem, definition);

                    pageItem.SystemUpdate();
                });
        }

        private SPListItem CreateObject(object modelHost, SPFolder folder, TemplateDefinitionBase definition)
        {
            var pageName = GetSafePageFileName(definition);
            var fileContent = definition.Content;

            ModuleFileModelHandler.DeployModuleFile(folder,
                  SPUrlUtility.CombineUrl(folder.ServerRelativeUrl, pageName),
                  pageName,
                  fileContent,
                  true,
                  null,
                  null);

            return GetCurrentObject(folder, definition);
        }


        protected SPListItem GetCurrentObject(SPFolder folder, TemplateDefinitionBase definition)
        {
            // TODO, CAML query
            var pageName = GetSafePageFileName(definition);

            foreach (SPFile file in folder.Files)
                if (file.Name.ToUpper() == pageName.ToUpper())
                    return file.Item;

            return null;
        }

        protected abstract void MapProperties(object modelHost, SPListItem item, TemplateDefinitionBase definition);

        #endregion
    }
}
