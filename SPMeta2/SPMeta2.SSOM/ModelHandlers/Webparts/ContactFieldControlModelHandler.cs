using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SharePoint.WebPartPages;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Webparts;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using WebPart = System.Web.UI.WebControls.WebParts.WebPart;

namespace SPMeta2.SSOM.ModelHandlers.Webparts
{
    public class ContactFieldControlModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ContactFieldControlDefinition); }
        }

        #endregion

        #region methods

        protected override void OnBeforeDeployModel(WebpartPageModelHost host, WebPartDefinition webpartModel)
        {
            var typedModel = webpartModel.WithAssertAndCast<ContactFieldControlDefinition>("webpartModel", value => value.RequireNotNull());
            typedModel.WebpartType = typeof(ContentEditorWebPart).AssemblyQualifiedName;
        }

        protected override void ProcessWebpartProperties(WebPart webpartInstance, WebPartDefinition webpartModel)
        {
            //base.ProcessWebpartProperties(webpartInstance, webpartModel);

            //var typedWebpart = webpartInstance.WithAssertAndCast<ContactFieldControl>("webpartInstance", value => value.RequireNotNull());
            //var typedModel = webpartModel.WithAssertAndCast<ContactFieldControlDefinition>("webpartModel", value => value.RequireNotNull());

            //typedWebpart.ContentLink = typedModel.ContentLink ?? string.Empty;

            //var xmlDoc = new XmlDocument();
            //var xmlElement = xmlDoc.CreateElement("ContentElement");
            //xmlElement.InnerText = typedModel.Content ?? string.Empty;

            //typedWebpart.Content = xmlElement;
        }

        #endregion

       
    }
}
