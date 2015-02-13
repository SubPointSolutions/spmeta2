using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client.Utilities;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class ScriptEditorWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ScriptEditorWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var wpModel = webPartModel.WithAssertAndCast<ScriptEditorWebPartDefinition>("model", value => value.RequireNotNull());
            var wpXml = WebpartXmlExtensions
                .LoadWebpartXmlDocument(BuiltInWebPartTemplates.ScriptEditorWebPart)
                //.SetOrUpdateProperty("Content", HttpUtility.HtmlEncode(wpModel.Content))
                .SetOrUpdateProperty("Content", wpModel.Content, true)
                .ToString();

            return wpXml;
        }

        #endregion
    }
}
