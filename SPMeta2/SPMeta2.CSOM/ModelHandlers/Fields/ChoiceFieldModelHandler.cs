using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.ModelHandlers.Fields
{
    public class ChoiceFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ChoiceFieldDefinition); }
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

        }

        protected override string GetTargetSPFieldXmlDefinition(FieldDefinition fieldModel)
        {
            var businessFieldModel = fieldModel.WithAssertAndCast<ChoiceFieldDefinition>("model", value => value.RequireNotNull());


            return string.Empty;
        }

        #endregion
    }
}
