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

            SPField siteField = null;

            // TODO, needs to be changed to using pattern and adjust all model handlers
            InvokeOnModelEvents<FieldDefinition, SPField>(siteField, ModelEventType.OnUpdating);

            if (modelHost is SPSite)
                siteField = EnsureSiteField(modelHost as SPSite, fieldModel);
            else if (modelHost is SPList)
                siteField = DeployListField(modelHost as SPList, fieldModel);
            else
            {
                throw new ArgumentException("modelHost needs to be SPSite/SPList");
            }

            ProcessCommonProperties(siteField, fieldModel);

            InvokeOnModelEvents<FieldDefinition, SPField>(siteField, ModelEventType.OnUpdated);

            siteField.Update(true);
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
            return EnsureFieldInFieldsCollection(list.Fields, fieldModel);
        }

        private static SPField EnsureSiteField(SPSite site, FieldDefinition fieldModel)
        {
            return EnsureFieldInFieldsCollection(site.RootWeb.Fields, fieldModel);
        }

        private static SPField EnsureFieldInFieldsCollection(SPFieldCollection fields, FieldDefinition fieldModel)
        {
            if (!fields.ContainsFieldWithStaticName(fieldModel.InternalName))
            {
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
            }

            return fields[fieldModel.Id];
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
