using System;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class FieldModelHandler : CSOMModelHandlerBase
    {
        #region properties

        #region properties

        private static string SiteFieldXmlTemplate = @"<Field " +
                                "ID=\"{0}\" " +
                                "StaticName=\"{1}\" " +
                                "DisplayName=\"{2}\" " +
                                "Title=\"{3}\" " +
                                "Name=\"{4}\" " +
                                "Type=\"{5}\" " +
                                "Group=\"{6}\" " +
                                "/>";

        #endregion

        #endregion

        #region methods

        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            if (!(modelHost is SiteModelHost || modelHost is List))
                throw new ArgumentException("modelHost needs to be SiteModelHost/List instance.");

            var fieldModel = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            Field currentField = null;
            ClientRuntimeContext context = null;

            InvokeOnModelEvents(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(Field),
                ObjectDefinition = model,
                ModelHost = modelHost
            });
            InvokeOnModelEvents<FieldDefinition, Field>(currentField, ModelEventType.OnUpdating);

            if (modelHost is SiteModelHost)
            {
                var siteHost = modelHost as SiteModelHost;
                context = siteHost.HostSite.Context;

                currentField = DeploySiteField(siteHost as SiteModelHost, fieldModel);
            }
            else if (modelHost is List)
            {
                var listHost = modelHost as List;
                context = listHost.Context;

                currentField = DeployListField(modelHost as List, fieldModel);
            }

            InvokeOnModelEvents(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentField,
                ObjectType = typeof(Field),
                ObjectDefinition = model,
                ModelHost = modelHost
            });
            InvokeOnModelEvents<FieldDefinition, Field>(currentField, ModelEventType.OnUpdated);

            currentField.UpdateAndPushChanges(true);
            context.ExecuteQuery();
        }

        private Field DeployListField(List list, FieldDefinition fieldModel)
        {
            var context = list.Context;

            context.Load(list, l => l.Fields);
            context.ExecuteQuery();

            return EnsureField(context, list.Fields, fieldModel);
        }

        private Field DeploySiteField(SiteModelHost siteModelHost, FieldDefinition fieldModel)
        {
            var site = siteModelHost.HostSite;

            var context = site.Context;
            var rootWeb = site.RootWeb;

            context.Load(rootWeb, tmpWeb => tmpWeb.Fields);
            context.ExecuteQuery();

            return EnsureField(context, rootWeb.Fields, fieldModel);
        }

        private Field EnsureField(ClientRuntimeContext context, FieldCollection fieldCollection, FieldDefinition fieldModel)
        {
            var currentField = FindExistingField(fieldCollection, fieldModel.InternalName);

            if (currentField == null)
            {
                var fieldDef = string.Format(SiteFieldXmlTemplate,
                                             new string[]
                                                 {
                                                     fieldModel.Id.ToString("B"),
                                                     fieldModel.InternalName,
                                                     fieldModel.Title,
                                                     fieldModel.Title,
                                                     fieldModel.InternalName,
                                                     fieldModel.FieldType,
                                                     fieldModel.Group
                                                 });

                currentField = fieldCollection.AddFieldAsXml(fieldDef, false, AddFieldOptions.DefaultValue);
            }

            currentField.Title = fieldModel.Title;
            currentField.Description = fieldModel.Description ?? string.Empty;
            currentField.Group = fieldModel.Group ?? string.Empty;

            return currentField;
        }

        protected Field FindListField(List list, FieldDefinition fieldModel)
        {
            var context = list.Context;

            context.Load(list, l => l.Fields);
            context.ExecuteQuery();

            return FindExistingField(list.Fields, fieldModel.InternalName);
        }

        protected Field FindSiteField(SiteModelHost siteModelHost, FieldDefinition fieldModel)
        {
            var site = siteModelHost.HostSite;

            var context = site.Context;
            var rootWeb = site.RootWeb;

            context.Load(rootWeb, tmpWeb => tmpWeb.Fields);
            context.ExecuteQuery();

            return FindExistingField(rootWeb.Fields, fieldModel.InternalName);
        }

        protected Field FindExistingField(FieldCollection fields, string internalFieldName)
        {
            foreach (var field in fields)
            {
                if (String.Compare(field.InternalName, internalFieldName, StringComparison.OrdinalIgnoreCase) == 0)
                    return field;
            }

            return null;
        }

        public override Type TargetType
        {
            get { return typeof(FieldDefinition); }
        }

        #endregion
    }
}
