using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Exceptions;
using SPMeta2.Services;
using SPMeta2.Utils;

namespace SPMeta2.CSOM.ModelHandlers.Fields
{
    public class DependentLookupFieldModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(DependentLookupFieldDefinition); }
        }
        #endregion

        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<DependentLookupFieldDefinition>("model", value => value.RequireNotNull());

            if (modelHost is SiteModelHost)
                DeploySiteDependentLookup(modelHost, modelHost as SiteModelHost, definition);
            else if (modelHost is WebModelHost)
                DeployWebDependentLookup(modelHost, modelHost as WebModelHost, definition);
            else if (modelHost is ListModelHost)
                DeployListDependentLookup(modelHost, modelHost as ListModelHost, definition);
        }

        private void DeployListDependentLookup(object modelHost, ListModelHost listModelHost, DependentLookupFieldDefinition definition)
        {
            DeployDependentLookupField(modelHost, listModelHost.HostList.Fields, definition);
        }

        private void DeployWebDependentLookup(object modelHost, WebModelHost webModelHost, DependentLookupFieldDefinition definition)
        {
            DeployDependentLookupField(modelHost, webModelHost.HostWeb.Fields, definition);
        }

        private void DeploySiteDependentLookup(object modelHost, SiteModelHost siteModelHost, DependentLookupFieldDefinition definition)
        {
            DeployDependentLookupField(modelHost, siteModelHost.HostSite.RootWeb.Fields, definition);
        }

        protected virtual FieldLookup GetField(object modelHost, DependentLookupFieldDefinition definition)
        {
            if (modelHost is SiteModelHost)
                return GetDependentLookupField((modelHost as SiteModelHost).HostSite.RootWeb.Fields, definition);
            else if (modelHost is WebModelHost)
                return GetDependentLookupField((modelHost as WebModelHost).HostWeb.Fields, definition);
            else if (modelHost is ListModelHost)
                return GetDependentLookupField((modelHost as ListModelHost).HostList.Fields, definition);

            throw new SPMeta2Exception("Unsupported model host");
        }

        protected virtual FieldLookup GetPrimaryField(object modelHost, DependentLookupFieldDefinition definition)
        {
            if (modelHost is SiteModelHost)
                return GetPrimaryLookupField((modelHost as SiteModelHost).HostSite.RootWeb.Fields, definition);
            else if (modelHost is WebModelHost)
                return GetPrimaryLookupField((modelHost as WebModelHost).HostWeb.Fields, definition);
            else if (modelHost is ListModelHost)
                return GetPrimaryLookupField((modelHost as ListModelHost).HostList.Fields, definition);

            throw new SPMeta2Exception("Unsupported model host");
        }

        private void DeployDependentLookupField(object modelHost, FieldCollection fields, DependentLookupFieldDefinition definition)
        {
            var context = fields.Context;

            var primaryLookupField = GetPrimaryLookupField(fields, definition);
            var dependentLookupField = GetDependentLookupField(fields, definition);

            if (dependentLookupField == null)
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = null,
                    ObjectType = typeof(FieldLookup),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                var internalName = fields.AddDependentLookup(definition.InternalName, primaryLookupField, definition.LookupField);

                dependentLookupField = GetDependentLookupField(fields, definition);
                dependentLookupField.Title = definition.Title;

                dependentLookupField.LookupList = primaryLookupField.LookupList;
                dependentLookupField.LookupWebId = primaryLookupField.LookupWebId;

                if (!string.IsNullOrEmpty(primaryLookupField.LookupList) &&
                string.IsNullOrEmpty(dependentLookupField.LookupList))
                {
                    dependentLookupField.LookupList = primaryLookupField.LookupList;
                }

                if (string.IsNullOrEmpty(dependentLookupField.PrimaryFieldId))
                    dependentLookupField.PrimaryFieldId = primaryLookupField.Id.ToString();

                dependentLookupField.ReadOnlyField = true;
                dependentLookupField.AllowMultipleValues = primaryLookupField.AllowMultipleValues;

                // unsuppoeted in CSOM yet
                //dependentLookupField.UnlimitedLengthInDocumentLibrary = primaryLookupField.UnlimitedLengthInDocumentLibrary;
                dependentLookupField.Direction = primaryLookupField.Direction;
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = dependentLookupField,
                    ObjectType = typeof(FieldLookup),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

            }

            dependentLookupField.Title = definition.Title;

            dependentLookupField.LookupField = definition.LookupField;

            if (!string.IsNullOrEmpty(primaryLookupField.LookupList) &&
                string.IsNullOrEmpty(dependentLookupField.LookupList))
            {
                dependentLookupField.LookupList = primaryLookupField.LookupList;
            }
            dependentLookupField.LookupWebId = primaryLookupField.LookupWebId;

            dependentLookupField.Group = string.IsNullOrEmpty(definition.Group) ? "Custom" : definition.Group; 
            dependentLookupField.Description = definition.Description ?? string.Empty;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = dependentLookupField,
                ObjectType = typeof(FieldLookup),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            dependentLookupField.UpdateAndPushChanges(true);

            context.ExecuteQuery();
        }

        protected virtual FieldLookup GetDependentLookupField(FieldCollection fields, 
            DependentLookupFieldDefinition definition)
        {
            var context = fields.Context;

            Field field = null;

            var scope = new ExceptionHandlingScope(context);

            using (scope.StartScope())
            {
                using (scope.StartTry())
                {
                    fields.GetByInternalNameOrTitle(definition.InternalName);
                }

                using (scope.StartCatch())
                {

                }
            }

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");
            context.ExecuteQueryWithTrace();

            if (!scope.HasException)
            {
                field = fields.GetByInternalNameOrTitle(definition.InternalName);
                context.Load(field);

                context.ExecuteQueryWithTrace();

                return context.CastTo<FieldLookup>(field);
            }

            return null;
        }

        protected virtual FieldLookup GetPrimaryLookupField(FieldCollection fields, DependentLookupFieldDefinition definition)
        {
            var context = fields.Context;

            Field result = null;

            if (definition.PrimaryLookupFieldId.HasGuidValue())
                result = fields.GetById(definition.PrimaryLookupFieldId.Value);

            if (!string.IsNullOrEmpty(definition.PrimaryLookupFieldInternalName))
                result = fields.GetByInternalNameOrTitle(definition.PrimaryLookupFieldInternalName);

            if (!string.IsNullOrEmpty(definition.PrimaryLookupFieldTitle))
                result = fields.GetByTitle(definition.PrimaryLookupFieldTitle);

            context.Load(result);
            context.ExecuteQuery();

            return context.CastTo<FieldLookup>(result);
        }

        #endregion
    }
}
