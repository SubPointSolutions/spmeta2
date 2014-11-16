using System;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Fields
{
    public class BooleanFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(BooleanFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(Field);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

        }

        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<BooleanFieldDefinition>("model", value => value.RequireNotNull());
        }

        #endregion
    }
}
