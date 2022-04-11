using System;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class DependentLookupFieldModelHandler : LookupFieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DependentLookupFieldDefinition); }
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedField = field as SPFieldLookup;
            var typedFieldModel = fieldModel.WithAssertAndCast<DependentLookupFieldDefinition>("model", value => value.RequireNotNull());

            var primaryLookupField = GetPrimaryField(typedFieldModel);

            typedField.LookupList = primaryLookupField.LookupList;
            typedField.LookupWebId = primaryLookupField.LookupWebId;
            typedField.ReadOnlyField = true;
            typedField.UnlimitedLengthInDocumentLibrary = primaryLookupField.UnlimitedLengthInDocumentLibrary;
            typedField.Direction = primaryLookupField.Direction;
            typedField.LookupField = typedFieldModel.LookupField;
        }
        
        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<DependentLookupFieldDefinition>("model", value => value.RequireNotNull());
            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Mult, typedFieldModel.AllowMultipleValues.ToString().ToUpper());

            SPField primaryField = GetPrimaryField(typedFieldModel);
            if (primaryField == null)
            {
                throw new SPMeta2Exception("PrimaryLookupField needs to be defined when creating a DependentLookupField");
            }

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.FieldReference, primaryField.Id.ToString("B"));
        }

        protected virtual SPFieldLookup GetPrimaryField(DependentLookupFieldDefinition definition)
        {
            var fields = FieldLookupService.GetFieldCollection(this.ModelHost);

            return FieldLookupService.GetFieldAs<SPFieldLookup>(
                fields, definition.PrimaryLookupFieldId, definition.PrimaryLookupFieldInternalName, definition.PrimaryLookupFieldTitle);
        }

        #endregion
    }
}
