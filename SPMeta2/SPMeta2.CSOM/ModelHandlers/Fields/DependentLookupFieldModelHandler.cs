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
    public class DependentLookupFieldModelHandler : LookupFieldModelHandler
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DependentLookupFieldDefinition); }
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

            var primaryLookupField = GetPrimaryField(typedFieldModel);

            typedField.Context.Load(primaryLookupField);
            typedField.Context.ExecuteQueryWithTrace();
            
            if (string.IsNullOrEmpty(typedField.PrimaryFieldId))
            {
                typedField.PrimaryFieldId = primaryLookupField.Id.ToString();
            }
            typedField.ReadOnlyField = true;

            if (!string.IsNullOrEmpty(typedFieldModel.RelationshipDeleteBehavior))
            {
                var value = (RelationshipDeleteBehaviorType)Enum.Parse(typeof(RelationshipDeleteBehaviorType), typedFieldModel.RelationshipDeleteBehavior);
                typedField.RelationshipDeleteBehavior = value;
            }

            // unsupported in CSOM yet
            //dependentLookupField.UnlimitedLengthInDocumentLibrary = primaryLookupField.UnlimitedLengthInDocumentLibrary;
            typedField.Direction = primaryLookupField.Direction;
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

            base.ProcessSPFieldXElement(fieldTemplate, fieldModel);

            var typedFieldModel = fieldModel.WithAssertAndCast<DependentLookupFieldDefinition>("model", value => value.RequireNotNull());

            Field primaryField = GetPrimaryField(typedFieldModel);
            if (primaryField == null)
            {
                throw new SPMeta2Exception("PrimaryLookupField needs to be defined when creating a DependentLookupField");
            }

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.FieldReference, primaryField.Id.ToString("B"));
        }

        #endregion
    }
}