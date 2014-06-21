using System;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.SSOM.ModelHandlers
{
    public class FieldModelHandler : SSOMModelHandlerBase
    {
        #region properties

        private const string MinimalSPFieldTemplate = "<Field ID=\"{0}\" StaticName=\"{1}\" DisplayName=\"{2}\" Title=\"{3}\" Name=\"{4}\" Type=\"{5}\" />";

        public override Type TargetType
        {
            get { return typeof(FieldDefinition); }
        }

        #endregion

        #region methods

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            CheckValidModelHost(modelHost);

            var fieldModel = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            SPField field = null;

            // TODO, needs to be changed to using pattern and adjust all model handlers
            InvokeOnModelEvent<FieldDefinition, SPField>(field, ModelEventType.OnUpdating);

            if (modelHost is SPSite)
                field = EnsureSiteField(modelHost as SPSite, fieldModel);
            else if (modelHost is SPList)
                field = DeployListField(modelHost as SPList, fieldModel);
            else
            {
                throw new ArgumentException("modelHost needs to be SPSite/SPList");
            }

            ProcessCommonProperties(field, fieldModel);

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = field,
                ObjectType = typeof(SPField),
                ObjectDefinition = fieldModel,
                ModelHost = modelHost
            });
            InvokeOnModelEvent<FieldDefinition, SPField>(field, ModelEventType.OnUpdated);

            field.Update(true);
        }

        private void CheckValidModelHost(object modelHost)
        {
            if (!(modelHost is SPSite || modelHost is SPList))
            {
                throw new ArgumentException("modelHost needs to be SPSite/SPList");
            }
        }

        private SPField DeployListField(SPList list, FieldDefinition fieldModel)
        {
            return EnsureFieldInFieldsCollection(list, list.Fields, fieldModel);
        }

        private  SPField EnsureSiteField(SPSite site, FieldDefinition fieldModel)
        {
            return EnsureFieldInFieldsCollection(site, site.RootWeb.Fields, fieldModel);
        }

        protected SPField GetField(object modelHost, FieldDefinition definition)
        {
            if (modelHost is SPSite)
                return GetSiteField(modelHost as SPSite, definition);
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
                    ObjectType = typeof(SPField),
                    ObjectDefinition = fieldModel,
                    ModelHost = modelHost
                });

                // first creation
                var fieldDef = string.Format(MinimalSPFieldTemplate, new string[]
                                                                         {
                                                                             fieldModel.Id.ToString("B"),
                                                                             fieldModel.InternalName,
                                                                             fieldModel.Title,
                                                                             fieldModel.Title,
                                                                             fieldModel.InternalName,
                                                                             fieldModel.FieldType
                                                                         });




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
                    ObjectType = typeof(SPField),
                    ObjectDefinition = fieldModel,
                    ModelHost = modelHost
                });
            }

            return currentField;
        }

        private static void ProcessCommonProperties(SPField siteField, FieldDefinition fieldModel)
        {
            siteField.Title = fieldModel.Title;
            siteField.Group = fieldModel.Group;

            // SPBug, description cannot be null
            siteField.Description = fieldModel.Description ?? string.Empty;
        }

        #endregion
    }
}
