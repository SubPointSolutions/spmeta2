using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FieldModelHandler : SSOMModelHandlerBase
    {
        #region properties

        protected const string MinimalSPFieldTemplate = "<Field ID=\"{0}\" StaticName=\"{1}\" DisplayName=\"{2}\" Title=\"{3}\" Name=\"{4}\" Type=\"{5}\" />";

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
                field = EnsureSiteField((modelHost as SiteModelHost).HostSite, fieldModel);
            else if (modelHost is SPList)
            {
                field = DeployListField(modelHost as SPList, fieldModel);
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
            if (!(modelHost is SiteModelHost || modelHost is SPList))
            {
                throw new ArgumentException("modelHost needs to be SPSite/SPList");
            }
        }

        private SPField DeployListField(SPList list, FieldDefinition fieldModel)
        {
            return EnsureFieldInFieldsCollection(list, list.Fields, fieldModel);
        }

        private SPField EnsureSiteField(SPSite site, FieldDefinition fieldModel)
        {
            return EnsureFieldInFieldsCollection(site, site.RootWeb.Fields, fieldModel);
        }

        protected SPField GetField(object modelHost, FieldDefinition definition)
        {
            if (modelHost is SiteModelHost)
                return GetSiteField((modelHost as SiteModelHost).HostSite, definition);
            else if (modelHost is SPList)
                return GetListField(modelHost as SPList, definition);
            else
            {
                throw new ArgumentException("modelHost needs to be SPSite/SPList");
            }
        }

        private SPField GetSiteField(SPSite site, FieldDefinition definition)
        {
            return site.RootWeb.Fields[definition.Id];
        }

        private SPField GetListField(SPList list, FieldDefinition definition)
        {
            return list.Fields[definition.Id];
        }

        protected virtual string GetTargetSPFieldXmlDefinition(FieldDefinition fieldModel)
        {
            return string.Format(MinimalSPFieldTemplate, new string[]
                                                                         {
                                                                             fieldModel.Id.ToString("B"),
                                                                             fieldModel.InternalName,
                                                                             fieldModel.Title,
                                                                             fieldModel.Title,
                                                                             fieldModel.InternalName,
                                                                             fieldModel.FieldType
                                                                         });
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
            field.Group = fieldModel.Group ?? string.Empty;

            field.Required = fieldModel.Required;

            field.Description = fieldModel.Description ?? string.Empty;
        }

        #endregion
    }
}
