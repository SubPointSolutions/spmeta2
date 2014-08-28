using System;
using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;
using SPMeta2.CSOM.ModelHosts;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWebPartPageDefinitionValidator : WebPartPageModelHandler
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var listModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var webPartPageModel = model.WithAssertAndCast<WebPartPageDefinition>("model", value => value.RequireNotNull());

            var list = listModelHost.HostList;
            //if (!string.IsNullOrEmpty(webPartPageModel.FolderUrl))
            //    throw new NotImplementedException("FolderUrl for the web part page model is not supported yet");

            var pageName = GetSafeWebPartPageFileName(webPartPageModel);
            var pageItem = list.QueryAndGetItemByFileName(pageName);

            TraceUtils.WithScope(traceScope =>
            {
                traceScope.WriteLine(string.Format("Validate model:[{0}] web part page:[{1}]", webPartPageModel, pageItem));
                Assert.IsNotNull(pageItem);

                traceScope.WithTraceIndent(trace =>
                {
                    var fileName = pageItem["FileLeafRef"];

                    traceScope.WriteLine(string.Format("Validate FileName model:[{0}] web part page:[{1}]", pageName, fileName));
                    Assert.AreEqual(pageName, fileName);
                });
            });
        }

        #endregion
    }
}
