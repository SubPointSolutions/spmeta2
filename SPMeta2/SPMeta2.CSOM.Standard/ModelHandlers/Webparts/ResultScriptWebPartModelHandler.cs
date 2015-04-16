using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Webparts
{
    public class ResultScriptWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ResultScriptWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var wpModel = webPartModel.WithAssertAndCast<ResultScriptWebPartDefinition>("model", value => value.RequireNotNull());
          
            var wpXml = WebpartXmlExtensions
                .LoadWebpartXmlDocument(BuiltInWebPartTemplates.ResultScriptWebPart)
                .ToString();

            return wpXml;
        }

        #endregion
    }
}
