using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Utils;
using SPMeta2.CSOM.Extensions;
using System.Xml.Linq;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.ModelHandlers.Fields
{
    public class DependentLookupFieldModelHandler : FieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DependentLookupFieldDefinition); }
        }

        protected override Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(FieldLookup);
        }

        #endregion

        #region methods

        protected override void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            var site = HostSite;
            var context = site.Context;

            // let base setting be setup
            base.ProcessFieldProperties(field, fieldModel);

            var typedField = field.Context.CastTo<FieldLookup>(field);
            var typedFieldModel = fieldModel.WithAssertAndCast<DependentLookupFieldDefinition>("model", value => value.RequireNotNull());

            if (!typedField.IsPropertyAvailable("PrimaryFieldId"))
            {
                typedField.Context.Load(typedField, f => f.PrimaryFieldId);
                typedField.Context.ExecuteQueryWithTrace();
            }

            var primaryLookupField = GetPrimaryField(typedFieldModel);

            typedField.Context.Load(primaryLookupField);
            typedField.Context.ExecuteQueryWithTrace();

            typedField.AllowMultipleValues = primaryLookupField.AllowMultipleValues;
            typedField.TypeAsString = primaryLookupField.TypeAsString;
            typedField.LookupList = primaryLookupField.LookupList;
            typedField.LookupWebId = primaryLookupField.LookupWebId;
            if (string.IsNullOrEmpty(typedField.PrimaryFieldId))
            {
                typedField.PrimaryFieldId = primaryLookupField.Id.ToString();
            }
            typedField.ReadOnlyField = true;

            // unsupported in CSOM yet
            //dependentLookupField.UnlimitedLengthInDocumentLibrary = primaryLookupField.UnlimitedLengthInDocumentLibrary;
            typedField.Direction = primaryLookupField.Direction;
            typedField.LookupField = typedFieldModel.LookupField;
        }

        protected virtual FieldLookup GetPrimaryField(DependentLookupFieldDefinition definition)
        {
            var fields = FieldLookupService.GetFieldCollection(this.ModelHost);

            return FieldLookupService.GetFieldAs<FieldLookup>(
                fields, definition.PrimaryLookupFieldId, definition.PrimaryLookupFieldInternalName, definition.PrimaryLookupFieldTitle);
        }
        
        protected override void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            var site = HostSite;
            var context = HostSite.Context;

            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<DependentLookupFieldDefinition>("model", value => value.RequireNotNull());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Mult, typedFieldModel.AllowMultipleValues.ToString().ToUpper());

            if (typedFieldModel.LookupWebId.HasGuidValue())
            {
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.WebId, typedFieldModel.LookupWebId.Value.ToString("B"));
            }

            if (!string.IsNullOrEmpty(typedFieldModel.LookupList))
            {
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.List, typedFieldModel.LookupList);
            }
            
            if (!string.IsNullOrEmpty(typedFieldModel.LookupField))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowField, typedFieldModel.LookupField);

            Field primaryField = GetPrimaryField(typedFieldModel);
            if (primaryField == null)
            {
                throw new SPMeta2Exception("PrimaryLookupField need to be defined when creating a DependentLookupField");
            }

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.FieldReference, primaryField.Id.ToString("B"));
        }

        #endregion
    }
}