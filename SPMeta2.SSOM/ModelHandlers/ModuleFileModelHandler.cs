using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class ModuleFileModelHandler : SSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ModuleFileDefinition); }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folder = modelHost.WithAssertAndCast<SPFolder>("modelHost", value => value.RequireNotNull());
            var moduleFile = model.WithAssertAndCast<ModuleFileDefinition>("model", value => value.RequireNotNull());

            ProcessFile(modelHost, folder, moduleFile);
        }

        private string GetSafeFileUrl(SPFolder folder, ModuleFileDefinition moduleFile)
        {
            return folder.ServerRelativeUrl + "/" + moduleFile.FileName;
        }

        private void ProcessFile(
            object modelHost,
            SPFolder folder, ModuleFileDefinition moduleFile)
        {
            var web = folder.ParentWeb;
            var list = folder.DocumentLibrary;

            var file = web.GetFile(GetSafeFileUrl(folder, moduleFile));

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = file.Exists ? file : null,
                ObjectType = typeof(SPFile),
                ObjectDefinition = moduleFile,
                ModelHost = modelHost
            });

            var fileName = moduleFile.FileName;
            var fileContent = moduleFile.Content;

            // for file deployment to the folder, root web folder or under _cts or other spccial folders
            // list == null

            // big todo with correct update and punblishing
            // get prev SPMeta2 impl for publishing pages
            if (list != null && (file.Exists && file.CheckOutType != SPFile.SPCheckOutType.None))
                file.UndoCheckOut();

            if (list != null && (list.EnableMinorVersions && file.Exists && file.Level == SPFileLevel.Published))
                file.UnPublish("Provision");

            if (list != null && (file.Exists && file.CheckOutType == SPFile.SPCheckOutType.None))
                file.CheckOut();

            var spFile = folder.Files.Add(fileName, fileContent, file.Exists);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = spFile,
                ObjectType = typeof(SPFile),
                ObjectDefinition = moduleFile,
                ModelHost = modelHost
            });

            spFile.Update();

            if (list != null && (file.Exists && file.CheckOutType != SPFile.SPCheckOutType.None))
                spFile.CheckIn("Provision");

            if (list != null && (list.EnableMinorVersions || list.EnableVersioning))
                spFile.Publish("Provision");

            if (list != null && list.EnableModeration)
                spFile.Approve("Provision");
        }

        #endregion
    }
}
