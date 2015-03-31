using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.ModelHandlers.Webparts
{
    public class UserCodePartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(UserCodeWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            throw new SPMeta2NotSupportedException("UserCode web part provision is not supported by CSOM (SP API limitations) - https://officespdev.uservoice.com/forums/224641-general/suggestions/7300326-add-support-for-spusercodewebpart-import-via-limit");

            var wpModel = webPartModel.WithAssertAndCast<UserCodeWebPartDefinition>("model", value => value.RequireNotNull());
            var wpXml = WebpartXmlExtensions
                .LoadWebpartXmlDocument(BuiltInWebPartTemplates.UserCodeWebPart)
                .SetOrUpdateMetadataPropertyAttribute("type", "name", wpModel.AssemblyFullName)
                .SetOrUpdateMetadataPropertyAttribute("Solution", "SolutionId", wpModel.SolutionId.ToString("D"))
                //.SetOrUpdateMetadataPropertyAttribute("Solution", "xmlns", "http://schemas.microsoft.com/sharepoint/")
                .ToString();

            return wpXml;
        }

        #endregion
    }
}
