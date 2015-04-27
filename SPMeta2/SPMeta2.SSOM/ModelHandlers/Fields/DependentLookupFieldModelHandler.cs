using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;
using SPMeta2.Exceptions;

namespace SPMeta2.SSOM.ModelHandlers.Fields
{
    public class DependentLookupFieldModelHandler : SSOMModelHandlerBase
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

        protected virtual SPFieldLookup GetField(object modelHost, DependentLookupFieldDefinition definition)
        {
            if (modelHost is SiteModelHost)
                return GetDependentLookupField((modelHost as SiteModelHost).HostSite.RootWeb.Fields, definition);
            else if (modelHost is WebModelHost)
                return GetDependentLookupField((modelHost as WebModelHost).HostWeb.Fields, definition);
            else if (modelHost is ListModelHost)
                return GetDependentLookupField((modelHost as ListModelHost).HostList.Fields, definition);

            throw new SPMeta2Exception("Unsupported model host");
        }

        protected virtual SPFieldLookup GetPrimaryField(object modelHost, DependentLookupFieldDefinition definition)
        {
            if (modelHost is SiteModelHost)
                return GetPrimaryLookupField((modelHost as SiteModelHost).HostSite.RootWeb.Fields, definition);
            else if (modelHost is WebModelHost)
                return GetPrimaryLookupField((modelHost as WebModelHost).HostWeb.Fields, definition);
            else if (modelHost is ListModelHost)
                return GetPrimaryLookupField((modelHost as ListModelHost).HostList.Fields, definition);

            throw new SPMeta2Exception("Unsupported model host");
        }

        private void DeployDependentLookupField(object modelHost, SPFieldCollection fields, DependentLookupFieldDefinition definition)
        {
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
                    ObjectType = typeof(SPFieldLookup),
                    ObjectDefinition = definition,
                    ModelHost = modelHost
                });

                var internalName = fields.AddDependentLookup(definition.InternalName, primaryLookupField.Id);

                //dependentLookupField = (SPFieldLookup)fields.CreateNewField(SPFieldType.Lookup.ToString(), definition.InternalName);

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
                dependentLookupField.UnlimitedLengthInDocumentLibrary = primaryLookupField.UnlimitedLengthInDocumentLibrary;
                dependentLookupField.Direction = primaryLookupField.Direction;

                //dependentLookupField.Update(true);
                //dependentLookupField = GetDependentLookupField(fields, definition);
            }
            else
            {
                InvokeOnModelEvent(this, new ModelEventArgs
                {
                    CurrentModelNode = null,
                    Model = null,
                    EventType = ModelEventType.OnProvisioning,
                    Object = dependentLookupField,
                    ObjectType = typeof(SPFieldLookup),
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

            dependentLookupField.Group = definition.Group ?? string.Empty;
            dependentLookupField.Description = definition.Description ?? string.Empty;

            InvokeOnModelEvent(this, new ModelEventArgs
            {
                CurrentModelNode = null,
                Model = null,
                EventType = ModelEventType.OnProvisioned,
                Object = dependentLookupField,
                ObjectType = typeof(SPFieldLookup),
                ObjectDefinition = definition,
                ModelHost = modelHost
            });

            if (fields.List != null)
                dependentLookupField.Update();
            else
                dependentLookupField.Update(true);
        }

        protected virtual SPFieldLookup GetDependentLookupField(SPFieldCollection fields, DependentLookupFieldDefinition definition)
        {
            var targetName = definition.InternalName.ToUpper();

            return fields
                     .OfType<SPField>()
                     .FirstOrDefault(f => f.InternalName.ToUpper() == targetName) as SPFieldLookup;
        }

        protected virtual SPFieldLookup GetPrimaryLookupField(SPFieldCollection fields, DependentLookupFieldDefinition definition)
        {
            if (definition.PrimaryLookupFieldId.HasGuidValue())
                return fields[definition.PrimaryLookupFieldId.Value] as SPFieldLookup;

            if (!string.IsNullOrEmpty(definition.PrimaryLookupFieldInternalName))
                return fields.GetFieldByInternalName(definition.PrimaryLookupFieldInternalName) as SPFieldLookup; ;

            if (!string.IsNullOrEmpty(definition.PrimaryLookupFieldTitle))
                return fields.GetField(definition.PrimaryLookupFieldTitle) as SPFieldLookup; ;

            throw new SPMeta2Exception("PrimaryLookupFieldTitle / PrimaryLookupFieldInternalName / PrimaryLookupFieldId need to be defined");
        }

        #endregion
    }
}
