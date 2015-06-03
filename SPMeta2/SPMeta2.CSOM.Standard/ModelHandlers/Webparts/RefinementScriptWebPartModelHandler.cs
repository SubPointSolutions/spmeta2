using System;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.Standard.ModelHandlers.Webparts
{
    public class RefinementScriptWebPartModelHandler : WebPartModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(RefinementScriptWebPartDefinition); }
        }

        #endregion

        #region methods

        protected override string GetWebpartXmlDefinition(ListItemModelHost listItemModelHost, WebPartDefinitionBase webPartModel)
        {
            var definition = webPartModel.WithAssertAndCast<RefinementScriptWebPartDefinition>("model", value => value.RequireNotNull());
            var xml = WebpartXmlExtensions.LoadWebpartXmlDocument(ProcessCommonWebpartProperties(BuiltInWebPartTemplates.RefinementScriptWebPart, webPartModel));

            if (!string.IsNullOrEmpty(definition.SelectedRefinementControlsJson))
                xml.SetOrUpdateProperty("SelectedRefinementControlsJson", definition.SelectedRefinementControlsJson);

            if (!string.IsNullOrEmpty(definition.EmptyMessage))
                xml.SetOrUpdateProperty("EmptyMessage", definition.EmptyMessage);


            return xml.ToString();
        }

        #endregion
    }
}
