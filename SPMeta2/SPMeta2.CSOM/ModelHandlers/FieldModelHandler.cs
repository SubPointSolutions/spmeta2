using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
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

        #endregion

        #region methods

        protected SiteModelHost CurrentSiteModelHost { get; set; }

        protected Field FindField(object modelHost, FieldDefinition definition)
        {
            if (modelHost is SiteModelHost)
                return FindSiteField(modelHost as SiteModelHost, definition);

            if (modelHost is ListModelHost)
                return FindListField((modelHost as ListModelHost).HostList, definition);

            throw new SPMeta2NotSupportedException(
                string.Format("Validation for artifact of type [{0}] under model host [{1}] is not supported.",
                    definition.GetType(),
                    modelHost.GetType()));
        }

        protected virtual Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(Field);
        }

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
                ObjectType = GetTargetFieldType(fieldModel),
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

            object typedField = null;

            // emulate context.CastTo<>() call for typed field type
            if (GetTargetFieldType(fieldModel) != currentField.GetType())
            {
                var method = context.GetType().GetMethod("CastTo");
                var generic = method.MakeGenericMethod(GetTargetFieldType(fieldModel));

                typedField = generic.Invoke(context, new object[] { currentField });
            }

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = typedField ?? currentField,
                ObjectType = GetTargetFieldType(fieldModel),
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

            field.Required = fieldModel.Required;
        }

        private Field EnsureField(ClientRuntimeContext context, Field currentField, FieldCollection fieldCollection,
            FieldDefinition fieldModel)
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
              .SetAttribute(BuiltInFieldAttributes.Type, fieldModel.FieldType)
              .SetAttribute(BuiltInFieldAttributes.Group, fieldModel.Group ?? string.Empty);

            // additions
            if (!String.IsNullOrEmpty(fieldModel.JSLink))
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.JSLink, fieldModel.JSLink);

            if (!string.IsNullOrEmpty(fieldModel.DefaultValue))
                fieldTemplate.SetSubNode("Default", fieldModel.DefaultValue);

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Hidden, fieldModel.Hidden.ToString());

            // ShowIn* settings
            if (fieldModel.ShowInDisplayForm.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInDisplayForm, fieldModel.ShowInDisplayForm.Value);

            if (fieldModel.ShowInEditForm.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInEditForm, fieldModel.ShowInEditForm.Value);

            if (fieldModel.ShowInListSettings.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInListSettings, fieldModel.ShowInListSettings.Value);

            if (fieldModel.ShowInNewForm.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInNewForm, fieldModel.ShowInNewForm.Value);

            if (fieldModel.ShowInVersionHistory.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInVersionHistory, fieldModel.ShowInVersionHistory.Value);

            if (fieldModel.ShowInViewForms.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.ShowInViewForms, fieldModel.ShowInViewForms.Value);

            // misc
            if (fieldModel.AllowDeletion.HasValue)
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.AllowDeletion, fieldModel.AllowDeletion.Value);

            fieldTemplate.SetAttribute(BuiltInFieldAttributes.Indexed, fieldModel.Indexed);

        }

        protected virtual string GetTargetSPFieldXmlDefinition(FieldDefinition fieldModel)
        {
            var fieldTemplate = GetNewMinimalSPFieldTemplate();

            ProcessSPFieldXElement(fieldTemplate, fieldModel);

            return fieldTemplate.ToString();
        }

        #endregion
    }
}
