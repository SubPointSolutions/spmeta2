using SPMeta2.CSOM.ModelHandlers.Fields;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Regression.CSOM.Utils;
using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;
using System.Xml.Linq;

namespace SPMeta2.Regression.CSOM.Validation.Fields
{
    public class ChoiceFieldDefinitionValidator : MultiChoiceFieldDefinitionValidator
    {
        public override Type TargetType
        {
            get
            {
                return typeof(ChoiceFieldDefinition);
            }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            base.DeployModel(modelHost, model);

            var definition = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());
            var spObject = GetField(modelHost, definition);

            var textField = spObject.Context.CastTo<FieldChoice>(spObject);
            var textDefinition = model.WithAssertAndCast<ChoiceFieldDefinition>("model", value => value.RequireNotNull());

            var textFieldAssert = ServiceFactory.AssertService.NewAssert(model, textDefinition, textField);

            textFieldAssert.ShouldBeEqual(m => m.EditFormat, o => o.GetEditFormat());
        }
    }

}
