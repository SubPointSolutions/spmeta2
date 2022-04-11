using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHandlers.Base;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class MasterPageModelHandler : MasterPageModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(MasterPageDefinition); }
        }

        public override string PageContentTypeId
        {
            get { return BuiltInContentTypeId.MasterPage; }
            set { }
        }

        public override string PageFileExtension
        {
            get { return ".master"; }
            set { }
        }

        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<FolderModelHost>("modelHost", value => value.RequireNotNull());

            var folder = listModelHost.CurrentLibraryFolder;
            var masterPageModel = model.WithAssertAndCast<MasterPageDefinition>("model", value => value.RequireNotNull());

            DeployPage(modelHost, listModelHost.CurrentLibrary, folder, masterPageModel);
        }

        #endregion
    }
}
