using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.Extensions;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using System.Web.UI.WebControls.WebParts;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Common;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class WebPartModelHandler : SSOMModelHandlerBase
    {
        #region properties

        private const string WebPartPageCmdTemplate =
                                          "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                                          "<Method ID=\"0, NewWebPage\">" +
                                          "<SetList Scope=\"Request\">{0}</SetList>" +
                                          "<SetVar Name=\"ID\">New</SetVar>" +
                                          "<SetVar Name=\"Title\">{1}</SetVar>" +
                                          "<SetVar Name=\"Cmd\">NewWebPage</SetVar>" +
                                          "<SetVar Name=\"WebPartPageTemplate\">{2}</SetVar>" +
                                          "<SetVar Name=\"Type\">WebPartPage</SetVar>" +
                                          "<SetVar Name=\"Overwrite\">{3}</SetVar>" +
                                          "</Method>";

        #endregion

        #region methods

        public override Type TargetType
        {
            get { return typeof(WebPartDefinition); }
        }

        protected virtual void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {

        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var host = modelHost.WithAssertAndCast<WebpartPageModelHost>("modelHost", value => value.RequireNotNull());
            var webpartModel = model.WithAssertAndCast<WebPartDefinition>("model", value => value.RequireNotNull());

            OnBeforeDeployModel(host, webpartModel);

            InvokeOnModelEvent<FieldDefinition, SPField>(null, ModelEventType.OnUpdating);
            WebPartExtensions.DeployWebPartToPage(host.SPLimitedWebPartManager, webpartModel,
                onUpdatingWebpartInstnce =>
                {
                    if (onUpdatingWebpartInstnce == null)
                        TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new web part");
                    else
                        TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing web part");

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioning,
                        Object = onUpdatingWebpartInstnce,
                        ObjectType = typeof(System.Web.UI.WebControls.WebParts.WebPart),
                        ObjectDefinition = model,
                        ModelHost = modelHost
                    });

                    InvokeOnModelEvent<WebPartDefinition, WebPart>(onUpdatingWebpartInstnce, ModelEventType.OnUpdating);

                },
                onUpdatedWebpartInstnce =>
                {
                    HandleWikiOrPublishingPageProvision(host.HostFile.ListItemAllFields, webpartModel);

                    InvokeOnModelEvent(this, new ModelEventArgs
                    {
                        CurrentModelNode = null,
                        Model = null,
                        EventType = ModelEventType.OnProvisioned,
                        Object = onUpdatedWebpartInstnce,
                        ObjectType = typeof(System.Web.UI.WebControls.WebParts.WebPart),
                        ObjectDefinition = model,
                        ModelHost = modelHost
                    });

                    InvokeOnModelEvent<WebPartDefinition, WebPart>(onUpdatedWebpartInstnce, ModelEventType.OnUpdated);
                },
                ProcessWebpartProperties);
        }

        private static Guid PageLayout = new Guid("0f800910-b30d-4c8f-b011-8189b2297094");
        private static Guid PublishingPageContent = new Guid("f55c4d88-1f2e-4ad9-aaa8-819af4ee7ee8");

        private static void HandleWikiOrPublishingPageProvision(SPListItem listItem, WebPartDefinition webpartModel)
        {
            if (!webpartModel.AddToPageContent)
                return;

            var targetFieldId = Guid.Empty;

            if (listItem.Fields.Contains(SPBuiltInFieldId.WikiField))
                targetFieldId = SPBuiltInFieldId.WikiField;
            else if (listItem.Fields.Contains(PageLayout) && webpartModel.AddToPageContent)
                targetFieldId = PublishingPageContent;
            else
            {
                return;
            }

            var wpRichTextTemplate = new StringBuilder();

            var wpId = webpartModel.Id
                                   .Replace("g_", string.Empty)
                                   .Replace("_", "-");

            var content = listItem[targetFieldId] == null
                ? string.Empty
                : listItem[targetFieldId].ToString();

            wpRichTextTemplate.AppendFormat("​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
            wpRichTextTemplate.AppendFormat("     <div class='ms-rtestate-read {0}' id='div_{0}'>", wpId);
            wpRichTextTemplate.AppendFormat("     </div>");
            wpRichTextTemplate.AppendFormat("</div>");

            var wikiResult = wpRichTextTemplate.ToString();

            if (string.IsNullOrEmpty(content))
            {
                content = wikiResult;

                listItem[targetFieldId] = content;
                listItem.Update();
            }
            else
            {
                if (content.ToUpper().IndexOf(wpId.ToUpper()) == -1)
                {
                    content += wikiResult;

                    listItem[targetFieldId] = content;
                    listItem.Update();
                }
            }

        }

        protected virtual void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartPageModel)
        {

        }

        #endregion
    }
}
