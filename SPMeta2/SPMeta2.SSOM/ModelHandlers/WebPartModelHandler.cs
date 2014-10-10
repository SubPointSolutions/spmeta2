using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.Extensions;
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
                    HandleWikiOrPublishingPageProvision(host, webpartModel);

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

        private static void HandleWikiOrPublishingPageProvision(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var targetFieldId = Guid.Empty;

            if (host.PageListItem.Fields.Contains(SPBuiltInFieldId.WikiField))
                targetFieldId = SPBuiltInFieldId.WikiField;
            else if (host.PageListItem.Fields.Contains(BuiltInPublishingFieldId.PageLayout) && webpartModel.AddToPublishingPageContent)
                targetFieldId = BuiltInPublishingFieldId.PublishingPageContent;
            else
            {
                return;
            }

            var wpRichTextTemplate = new StringBuilder();

            var wpId = webpartModel.Id
                                   .Replace("g_", string.Empty)
                                   .Replace("_", "-");

            var content = host.PageListItem[targetFieldId] == null
                ? string.Empty
                : host.PageListItem[targetFieldId].ToString();

            wpRichTextTemplate.AppendFormat("​​​​​​​​​​​​​​​​​​​​​​<div class='ms-rtestate-read ms-rte-wpbox' contentEditable='false'>");
            wpRichTextTemplate.AppendFormat("     <div class='ms-rtestate-read {0}' id='div_{0}'>", wpId);
            wpRichTextTemplate.AppendFormat("     </div>");
            wpRichTextTemplate.AppendFormat("</div>");

            var wikiResult = wpRichTextTemplate.ToString();

            if (string.IsNullOrEmpty(content))
            {
                content = wikiResult;

                host.PageListItem[targetFieldId] = content;
                host.PageListItem.Update();
            }
            else
            {
                if (content.ToUpper().IndexOf(wpId.ToUpper()) == -1)
                {
                    content += wikiResult;

                    host.PageListItem[targetFieldId] = content;
                    host.PageListItem.Update();
                }
            }

        }

        protected virtual void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartPageModel)
        {

        }

        #endregion
    }
}
