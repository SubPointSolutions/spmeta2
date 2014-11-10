using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;
using System.Xml.Linq;
using SPMeta2.Enumerations;
using Microsoft.SharePoint;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class ChoiceFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(ChoiceFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(SPFieldChoice);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            // TODO
        }

        protected override string GetTargetSPFieldXmlDefinition(FieldDefinition fieldModel)
        {
            var typedFieldModel = fieldModel.WithAssertAndCast<ChoiceFieldDefinition>("model", value => value.RequireNotNull());

            // TODOS

            return string.Empty;
        }

        #endregion
    }


}
