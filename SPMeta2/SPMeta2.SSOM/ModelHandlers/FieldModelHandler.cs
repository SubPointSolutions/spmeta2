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

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FieldModelHandler : SSOMModelHandlerBase
    {
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
                new XAttribute(BuiltInFieldAttributes.Group, String.Empty));
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

            if (ModelHost is ListModelHost)
                return (ModelHost as ListModelHost).HostList.ParentWeb.Site;

            return null;
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            ModelHost = modelHost;

            CheckValidModelHost(modelHost);

            var fieldModel = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            SPField field = null;

            // TODO, needs to be changed to using pattern and adjust all model handlers
            InvokeOnModelEvent<FieldDefinition, SPField>(field, ModelEventType.OnUpdating);

            var isListField = false;

            if (modelHost is SiteModelHost)
                field = DeploySiteField((modelHost as SiteModelHost).HostSite, fieldModel);
            else if (modelHost is ListModelHost)
            {
                field = DeployListField((modelHost as ListModelHost).HostList, fieldModel);
                isListField = true;
            }
            else
            {
                throw new ArgumentException("modelHost needs to be SPSite/SPList");
            }

            ProcessFieldProperties(field, fieldModel);

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

            InvokeOnModelEvent<FieldDefinition, SPField>(field, ModelEventType.OnUpdated);

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

        private void CheckValidModelHost(object modelHost)
        {
            if (!(modelHost is SiteModelHost ||
                  modelHost is ListModelHost ||
                  modelHost is SPSite ||
                  modelHost is SPList))
            {
                throw new ArgumentException("modelHost needs to be SPSite/SPList");
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

        protected SPField GetField(object modelHost, FieldDefinition definition)
        {
            if (modelHost is SiteModelHost)
                return GetSiteField((modelHost as SiteModelHost).HostSite, definition);
            else if (modelHost is ListModelHost)
                return GetListField((modelHost as ListModelHost).HostList, definition);
            else
            {
                throw new ArgumentException("modelHost needs to be SPSite/SPList");
            }
        }

        private SPField GetSiteField(SPSite site, FieldDefinition definition)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving site field by ID: [{0}]", definition.Id);

            return site.RootWeb.Fields[definition.Id];
        }

        private SPField GetListField(SPList list, FieldDefinition definition)
        {
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving list field by ID: [{0}]", definition.Id);

            return list.Fields[definition.Id];
        }

        protected virtual void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            // minimal set
            fieldTemplate
              .SetAttribute(BuiltInFieldAttributes.ID, fieldModel.Id.ToString("B"))
              .SetAttribute(BuiltInFieldAttributes.StaticName, fieldModel.InternalName)
              .SetAttribute(BuiltInFieldAttributes.DisplayName, fieldModel.Title)
              .SetAttribute(BuiltInFieldAttributes.Title, fieldModel.Title)
              .SetAttribute(BuiltInFieldAttributes.Name, fieldModel.InternalName)
              .SetAttribute(BuiltInFieldAttributes.Required, fieldModel.Required.ToString().ToUpper())
              .SetAttribute(BuiltInFieldAttributes.Description, fieldModel.Description)
              .SetAttribute(BuiltInFieldAttributes.Type, fieldModel.FieldType)
              .SetAttribute(BuiltInFieldAttributes.Group, fieldModel.Group ?? string.Empty);

            // additions
            if (!String.IsNullOrEmpty(fieldModel.JSLink))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.JSLink, fieldModel.JSLink);

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
            SPField currentField = null;

            if (!fields.ContainsFieldWithStaticName(fieldModel.InternalName))
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
                fields.AddFieldAsXml(fieldDef);

                currentField = fields[fieldModel.Id];
            }
            else
            {
                TraceService.Information((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing field");

                currentField = fields[fieldModel.Id];

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

        protected virtual void ProcessFieldProperties(SPField field, FieldDefinition fieldModel)
        {
            field.Title = fieldModel.Title;

            field.Description = fieldModel.Description ?? string.Empty;
            field.Group = fieldModel.Group ?? string.Empty;

            field.Required = fieldModel.Required;
        }

        #endregion
    }
}
