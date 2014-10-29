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

        public override Type TargetType
        {
            get { return typeof(FieldDefinition); }
        }

        // TODO, replace with XElement generation later.
        private static string MinimalSPFieldTemplate =
                                @"<Field " +
                                    "ID=\"{0}\" " +
                                    "StaticName=\"{1}\" " +
                                    "DisplayName=\"{2}\" " +
                                    "Title=\"{3}\" " +
                                    "Name=\"{4}\" " +
                                    "Type=\"{5}\" " +
                                    "Group=\"{6}\" " +
                                    "/>";

        #endregion

        #region methods

        protected SiteModelHost CurrentSiteModelHost { get; set; }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            if (!(modelHost is SiteModelHost || modelHost is List))
                throw new ArgumentException("modelHost needs to be SiteModelHost/List instance.");

            CurrentSiteModelHost = modelHost as SiteModelHost;

            var fieldModel = model.WithAssertAndCast<FieldDefinition>("model", value => value.RequireNotNull());

            Field currentField = null;
            ClientRuntimeContext context = null;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioning,
                Object = null,
                ObjectType = typeof(Field),
                ObjectDefinition = model,
                ModelHost = modelHost
            });
            InvokeOnModelEvent<FieldDefinition, Field>(currentField, ModelEventType.OnUpdating);

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

            currentField.Required = fieldModel.Required;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = currentField,
                ObjectType = typeof(Field),
                ObjectDefinition = model,
                ModelHost = modelHost
            });
            InvokeOnModelEvent<FieldDefinition, Field>(currentField, ModelEventType.OnUpdated);



            currentField.UpdateAndPushChanges(true);
            context.ExecuteQuery();
        }

        private Field DeployListField(List list, FieldDefinition fieldModel)
        {
            var context = list.Context;

            var scope = new ExceptionHandlingScope(context);

            Field field;

            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    field = list.Fields.GetById(fieldModel.Id);
                    context.Load(field);
                }

                using (scope.StartCatch())
                {

                }
            }

            context.ExecuteQuery();

            return EnsureField(context, field, list.Fields, fieldModel);
        }

        private Field FindExistingSiteField(Web rootWeb, Guid id)
        {
            var context = rootWeb.Context;
            var scope = new ExceptionHandlingScope(context);

            Field field = null;

            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    rootWeb.Fields.GetById(id);
                }

                using (scope.StartCatch())
                {

                }
            }

            context.ExecuteQuery();

            if (!scope.HasException)
            {
                field = rootWeb.Fields.GetById(id);
                context.Load(field);
                context.ExecuteQuery();
            }

            return field;
        }

        private Field DeploySiteField(SiteModelHost siteModelHost, FieldDefinition fieldModel)
        {
            var site = siteModelHost.HostSite;
            var context = site.Context;

            var field = FindExistingSiteField(site.RootWeb, fieldModel.Id);

            return EnsureField(context, field, site.RootWeb.Fields, fieldModel);
        }

        protected virtual void ProcessFieldProperties(Field field, FieldDefinition fieldModel)
        {
            field.Title = fieldModel.Title;

            field.Description = fieldModel.Description ?? string.Empty;
            field.Group = fieldModel.Group ?? string.Empty;
        }

        private Field EnsureField(ClientRuntimeContext context, Field currentField, FieldCollection fieldCollection, FieldDefinition fieldModel)
        {
            if (currentField == null)
            {
                var fieldDef = GetTargetSPFieldXmlDefinition(fieldModel);
                var resultField = fieldCollection.AddFieldAsXml(fieldDef, false, AddFieldOptions.DefaultValue);

                ProcessFieldProperties(resultField, fieldModel);

                return resultField;
            }
            else
            {
                ProcessFieldProperties(currentField, fieldModel);

                return currentField;
            }
        }

        protected Field FindListField(List list, FieldDefinition fieldModel)
        {
            var context = list.Context;

            Field field;

            var scope = new ExceptionHandlingScope(context);

            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    field = list.Fields.GetById(fieldModel.Id);
                    context.Load(field);
                }
                using (scope.StartCatch())
                {
                    field = null;
                    //context.Load(field);
                }
            }

            context.ExecuteQuery();

            return field;
        }

        protected Field FindSiteField(SiteModelHost siteModelHost, FieldDefinition fieldModel)
        {
            var site = siteModelHost.HostSite;

            var context = site.Context;
            var rootWeb = site.RootWeb;

            var scope = new ExceptionHandlingScope(context);
            Field field = null;

            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    rootWeb.Fields.GetById(fieldModel.Id);
                }
                using (scope.StartCatch())
                {

                }
            }

            context.ExecuteQuery();

            if (!scope.HasException)
            {
                field = rootWeb.Fields.GetById(fieldModel.Id);
                context.Load(field);
                context.ExecuteQuery();
            }

            return field;

            //return FindExistingField(rootWeb.Fields, fieldModel.InternalName);
        }

        //protected Field FindExistingField(FieldCollection fields, string internalFieldName)
        //{
        //    foreach (var field in fields)
        //    {
        //        if (String.Compare(field.InternalName, internalFieldName, StringComparison.OrdinalIgnoreCase) == 0)
        //            return field;
        //    }

        //    return null;
        //}


        protected virtual string GetTargetSPFieldXmlDefinition(FieldDefinition fieldModel)
        {
            return string.Format(MinimalSPFieldTemplate, new string[]
                                                                         {
                                                                             fieldModel.Id.ToString("B"),
                                                                             fieldModel.InternalName,
                                                                             fieldModel.Title,
                                                                             fieldModel.Title,
                                                                             fieldModel.InternalName,
                                                                             fieldModel.FieldType,
                                                                             fieldModel.Group ?? string.Empty
                                                                         });
        }

        #endregion
    }
}
