using SPMeta2.Definitions.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml.Linq;

using SPMeta2.Utils;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.CSOM.ModelHandlers.Fields
{
    public class BusinessDataFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(BusinessDataFieldDefinition); }
        }


        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            // can't be casted to BusinessDataField yet.

            // process bcs field specific properties
            //var bcsField = field.WithAssertAndCast<BusinessDataField>("field", value => value.RequireNotNull());
            //var bcsFieldModel = fieldModel.WithAssertAndCast<BusinessDataFieldDefinition>("model", value => value.RequireNotNull());

            //bcsField.SystemInstanceName = bcsFieldModel.SystemInstanceName;
            //bcsField.EntityNamespace = bcsFieldModel.EntityNamespace;

            //bcsField.EntityName = bcsFieldModel.EntityName;
            //bcsField.BdcFieldName = bcsFieldModel.BdcFieldName;
        }


        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var businessFieldModel = fieldModel.WithAssertAndCast<BusinessDataFieldDefinition>("model", value => value.RequireNotNull());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.SystemInstance, businessFieldModel.SystemInstanceName);
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.EntityNamespace, businessFieldModel.EntityNamespace);
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.EntityName, businessFieldModel.EntityName);
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.BdcField, businessFieldModel.BdcFieldName);
        }

        #endregion
    }
}
