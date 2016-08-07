using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.ModelHandlers;
using SPMeta2.Services;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using System.Globalization;
using SPMeta2.Exceptions;
using System.Diagnostics;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FieldModelHandler : SSOMModelHandlerBase
    {
        #region construactors

        static FieldModelHandler()
        {
        }

        #endregion

        #region properties

        protected static XElement GetNewMinimalSPFieldTemplate()
        {
            return new XElement("Field",
                new XAttribute(BuiltInFieldAttributes.ID, String.Empty),
                new XAttribute(BuiltInFieldAttributes.StaticName, String.Empty),
                new XAttribute(BuiltInFieldAttributes.DisplayName, String.Empty),
                new XAttribute(BuiltInFieldAttributes.Title, String.Empty),
                new XAttribute(BuiltInFieldAttributes.Name, String.Empty),
                new XAttribute(BuiltInFieldAttributes.Type, String.Empty),
                new XAttribute(BuiltInFieldAttributes.Group, String.Empty),
                new XAttribute(BuiltInFieldAttributes.CanToggleHidden, "TRUE"));
        }

        public override Type TargetType
        {
            get { return typeof(FieldDefinition); }
        }

        #endregion

        #region methods

        // TODO

        protected object ModelHost { get; set; }

        protected SPSite GetCurrentSite()
        {
            if (ModelHost is SiteModelHost)
                return (ModelHost as SiteModelHost).HostSite;

            if (ModelHost is WebModelHost)
                return (ModelHost as WebModelHost).HostWeb.Site;

            if (ModelHost is ListModelHost)
                return (ModelHost as ListModelHost).HostList.ParentWeb.Site;

            return null;
        }

        protected SPWeb GetCurrentWeb()
        {
            if (ModelHost is SiteModelHost)
                return (ModelHost as SiteModelHost).HostSite.RootWeb;

            if (ModelHost is ListModelHost)
                return (ModelHost as ListModelHost).HostList.ParentWeb;

            if (ModelHost is WebModelHost)
                return (ModelHost as WebModelHost).HostWeb;

            return null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            ModelHost = modelHost;

            CheckValidModelHost(modelHost);

            var fieldModel = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            SPField field = null;

            var isListField = false;

            if (modelHost is SiteModelHost)
            {
                field = DeploySiteField((modelHost as SiteModelHost).HostSite, fieldModel);
            }
            else if (modelHost is WebModelHost)
            {
                field = DeployWebField((modelHost as WebModelHost).HostWeb, fieldModel);
                isListField = true;
            }
            else if (modelHost is ListModelHost)
            {
                field = DeployListField((modelHost as ListModelHost).HostList, fieldModel);
                isListField = true;
            }
            else
            {
                throw new ArgumentException("modelHost needs to be SiteModelHost/WebModelHost/ListModelHost");
            }

            ProcessFieldProperties(field, fieldModel);
            ProcessFieldLocalization(field, fieldModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = field,
                ObjectType = GetTargetFieldType(fieldModel),
                ObjectDefinition = fieldModel,
                ModelHost = modelHost
            });

            // no promotion for the list field, and force push for the site fields
            if (isListField)
            {
                field.Update();
            }
            else
            {
                field.Update(true);
            }
        }

        private SPField DeployWebField(SPWeb web, FieldDefinition fieldModel)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying list field");

            return EnsureFieldInFieldsCollection(web, web.Fields, fieldModel);
        }

        private void CheckValidModelHost(object modelHost)
        {
            if (!(modelHost is SiteModelHost ||
                  modelHost is ListModelHost ||
                  modelHost is WebModelHost ||
                  modelHost is SPSite ||
                  modelHost is SPList))
            {
                throw new ArgumentException("modelHost needs to be SiteModelHost/WebModelHost/ListModelHost");
            }
        }

        private SPField DeployListField(SPList list, FieldDefinition fieldModel)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying list field");

            return EnsureFieldInFieldsCollection(list, list.Fields, fieldModel);
        }

        private SPField DeploySiteField(SPSite site, FieldDefinition fieldModel)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying site field");

            return EnsureFieldInFieldsCollection(site, site.RootWeb.Fields, fieldModel);
        }

        protected SPList ExtractListFromHost(object modelHost)
        {
            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList;

            return null;
        }

        protected SPWeb ExtractWebFromHost(object modelHost)
        {
            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.ParentWeb;

            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb;

            return null;
        }

        protected SPField GetField(object modelHost, FieldDefinition definition)
        {
            var fields = FieldLookupService.GetFieldCollection(modelHost);

            return FieldLookupService.GetField(fields, definition.Id, definition.InternalName, definition.Title);
        }

        protected virtual void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            // minimal set
            fieldTemplate
              .SetAttribute(BuiltInFieldAttributes.ID, fieldModel.Id.ToString("B"))
              .SetAttribute(BuiltInFieldAttributes.DisplayName, fieldModel.Title)
              .SetAttribute(BuiltInFieldAttributes.Title, fieldModel.Title)
              .SetAttribute(BuiltInFieldAttributes.Name, fieldModel.InternalName)
              .SetAttribute(BuiltInFieldAttributes.Required, fieldModel.Required.ToString().ToUpper())
              .SetAttribute(BuiltInFieldAttributes.Description, fieldModel.Description)
              .SetAttribute(BuiltInFieldAttributes.Type, fieldModel.FieldType)
              .SetAttribute(BuiltInFieldAttributes.Group, fieldModel.Group ?? string.Empty);

            // static name is by defaul gets InternalName
            if (!string.IsNullOrEmpty(fieldModel.StaticName))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.StaticName, fieldModel.StaticName);
            else
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.StaticName, fieldModel.InternalName);

            // additions
#if !NET35
            if (!String.IsNullOrEmpty(fieldModel.JSLink))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.JSLink, fieldModel.JSLink);
#endif

            if (!string.IsNullOrEmpty(fieldModel.DefaultValue))
                fieldTemplate.SetSubNode("Default", fieldModel.DefaultValue);

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Hidden, fieldModel.Hidden.ToString().ToUpper());

            // ShowIn* settings
            if (fieldModel.ShowInDisplayForm.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInDisplayForm, fieldModel.ShowInDisplayForm.Value.ToString().ToUpper());

            if (fieldModel.ShowInEditForm.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInEditForm, fieldModel.ShowInEditForm.Value.ToString().ToUpper());

            if (fieldModel.ShowInListSettings.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInListSettings, fieldModel.ShowInListSettings.Value.ToString().ToUpper());

            if (fieldModel.ShowInNewForm.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInNewForm, fieldModel.ShowInNewForm.Value.ToString().ToUpper());

            if (fieldModel.ShowInVersionHistory.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInVersionHistory, fieldModel.ShowInVersionHistory.Value.ToString().ToUpper());

            if (fieldModel.ShowInViewForms.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInViewForms, fieldModel.ShowInViewForms.Value.ToString().ToUpper());

            // misc
            if (fieldModel.AllowDeletion.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.AllowDeletion, fieldModel.AllowDeletion.Value.ToString().ToUpper());

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Indexed, fieldModel.Indexed.ToString().ToUpper());

            // add up additional attributes
            if (fieldModel.AdditionalAttributes.Any())
                foreach (var fieldAttr in fieldModel.AdditionalAttributes)
                    fieldTemplate.SetAttribute(fieldAttr.Name, fieldAttr.Value);
        }

        protected virtual string GetTargetSPFieldXmlDefinition(FieldDefinition fieldModel)
        {
            var fieldTemplate = GetNewMinimalSPFieldTemplate();

            if (!string.IsNullOrEmpty(fieldModel.RawXml))
                fieldTemplate = XDocument.Parse(fieldModel.RawXml).Root;

            ProcessSPFieldXElement(fieldTemplate, fieldModel);

            return fieldTemplate.ToString();
        }

        protected virtual Type GetTargetFieldType(FieldDefinition fieldModel)
        {
            return typeof(SPField);
        }

        private SPField EnsureFieldInFieldsCollection(
            object modelHost,
            SPFieldCollection fields, FieldDefinition fieldModel)
        {
            // by ID?
            var currentField = fields.OfType<SPField>()
                                        .FirstOrDefault(f => f.Id == fieldModel.Id);

            // by internal name?
            if (currentField == null && !string.IsNullOrEmpty(fieldModel.InternalName))
            {
                TraceService.Information((int)LogEventId.CoreCalls, "Could not find field by ID, fallback on InternalName");

                currentField = fields.OfType<SPField>()
                                           .FirstOrDefault(f => String.Equals(f.InternalName, fieldModel.InternalName,
                                                                StringComparison.OrdinalIgnoreCase));
            }

            if (currentField == null)
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingNewObject, "Processing new field");

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = currentField,
                    ObjectType = GetTargetFieldType(fieldModel),
                    ObjectDefinition = fieldModel,
                    ModelHost = modelHost
                });

                var fieldDef = GetTargetSPFieldXmlDefinition(fieldModel);
                var addFieldOptions = (SPAddFieldOptions)(int)fieldModel.AddFieldOptions;

                fields.AddFieldAsXml(fieldDef, fieldModel.AddToDefaultView, addFieldOptions);

                currentField = fields[fieldModel.Id];
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing field");

                currentField = fields.OfType<SPField>()
                                        .FirstOrDefault(f => f.Id == fieldModel.Id);

                if (currentField == null && !string.IsNullOrEmpty(fieldModel.InternalName))
                {
                    TraceService.Information((int)LogEventId.CoreCalls, "Could not find existing field by ID, fallback on InternalName");

                    currentField = fields.OfType<SPField>()
                                               .FirstOrDefault(f => String.Equals(f.InternalName, fieldModel.InternalName,
                                                                    StringComparison.OrdinalIgnoreCase));
                }

                if (currentField == null)
                {
                    throw new SPMeta2Exception(
                        string.Format("Cannot find existing field neither by ID [{0}] or InternalName [{1}]. Definition:[{2}]",
                        fieldModel.Id, fieldModel.InternalName, fieldModel));
                }

                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = currentField,
                    ObjectType = GetTargetFieldType(fieldModel),
                    ObjectDefinition = fieldModel,
                    ModelHost = modelHost
                });
            }

            return currentField;
        }

        protected virtual void HandleIncorectlyDeletedTaxonomyField(FieldDefinition fieldModel,
            SPFieldCollection fields)
        {
            return;

            // excluded due ot potential data corruption
            // such issues shoud be handled by end user manually

            //var isTaxField =
            //    fieldModel.FieldType.ToUpper() == BuiltInFieldTypes.TaxonomyFieldType.ToUpper()
            //    || fieldModel.FieldType.ToUpper() == BuiltInFieldTypes.TaxonomyFieldTypeMulti.ToUpper();

            //if (!isTaxField)
            //    return;

            //var existingIndexedFieldName = fieldModel.Title.ToUpper() + "_";
            //var existingIndexedField = fields.OfType<SPField>()
            //    .FirstOrDefault(f => f.Title.ToUpper().StartsWith(existingIndexedFieldName));

            //if (existingIndexedField != null && existingIndexedField.Type == SPFieldType.Note)
            //{
            //    // tmp fix
            //    // https://github.com/SubPointSolutions/spmeta2/issues/521
            //    try
            //    {
            //        existingIndexedField.Delete();
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
        }

        protected virtual void ProcessFieldProperties(SPField field, FieldDefinition definition)
        {
            field.Title = definition.Title;

            field.Description = definition.Description ?? string.Empty;
            field.Group = string.IsNullOrEmpty(definition.Group) ? "Custom" : definition.Group;

            field.Required = definition.Required;

            if (!string.IsNullOrEmpty(definition.StaticName))
                field.StaticName = definition.StaticName;

            if (!string.IsNullOrEmpty(definition.ValidationMessage))
                field.ValidationMessage = definition.ValidationMessage;

            if (!string.IsNullOrEmpty(definition.ValidationFormula))
                field.ValidationFormula = definition.ValidationFormula;

            if (definition.AllowDeletion.HasValue)
                field.AllowDeletion = definition.AllowDeletion.Value;

            if (!string.IsNullOrEmpty(definition.DefaultValue))
                field.DefaultValue = definition.DefaultValue;

            if (!string.IsNullOrEmpty(definition.DefaultFormula))
                field.DefaultFormula = definition.DefaultFormula;

            if (definition.EnforceUniqueValues.HasValue)
                field.EnforceUniqueValues = definition.EnforceUniqueValues.Value;

            // Setting the index property will throw an exception for dependent lookup fields
            if (!(field is SPFieldLookup) || !((SPFieldLookup)field).IsDependentLookup)
            {
                field.Indexed = definition.Indexed;
            }
#if !NET35
            field.JSLink = definition.JSLink;
#endif

            if (definition.ShowInEditForm.HasValue)
                field.ShowInEditForm = definition.ShowInEditForm.Value;

            if (definition.ShowInDisplayForm.HasValue)
                field.ShowInDisplayForm = definition.ShowInDisplayForm.Value;

            if (definition.ShowInListSettings.HasValue)
                field.ShowInListSettings = definition.ShowInListSettings.Value;

            if (definition.ShowInViewForms.HasValue)
                field.ShowInViewForms = definition.ShowInViewForms.Value;

            if (definition.ShowInNewForm.HasValue)
                field.ShowInNewForm = definition.ShowInNewForm.Value;

            if (definition.ShowInVersionHistory.HasValue)
                field.ShowInVersionHistory = definition.ShowInVersionHistory.Value;


            // process localiation
        }

        protected virtual void ProcessFieldLocalization(SPField obj, FieldDefinition definition)
        {
            if (definition.TitleResource.Any())
            {
                foreach (var locValue in definition.TitleResource)
                    LocalizationService.ProcessUserResource(obj, obj.TitleResource, locValue);
            }

            if (definition.DescriptionResource.Any())
            {
                foreach (var locValue in definition.DescriptionResource)
                    LocalizationService.ProcessUserResource(obj, obj.DescriptionResource, locValue);
            }
        }

        #endregion
    }
}
