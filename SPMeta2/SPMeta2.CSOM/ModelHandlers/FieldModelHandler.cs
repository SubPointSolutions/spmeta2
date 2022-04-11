using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.SharePoint.Client;
using SPMeta2.Common;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Services;
using SPMeta2.Utils;
using System.Collections.Generic;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class FieldModelHandler : CSOMModelHandlerBase
    {
        #region constructors

        static FieldModelHandler()
        {

        }

        #endregion

        #region properties

        public override Type TargetType
        {
            get { return typeof(FieldDefinition); }
        }

        protected ClientContext CurrentHostClientContext { get; set; }
        protected CSOMModelHostBase CurrentModelHost { get; set; }

        #endregion

        #region methods

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

        protected virtual bool PreloadProperties(Field field)
        {
            return false;
        }

        protected List ExtractListFromHost(object modelHost)
        {
            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList;

            return null;
        }

        protected Field GetField(object modelHost, FieldDefinition definition)
        {
            FieldCollection fields = FieldLookupService.GetFieldCollection(modelHost);

            return FieldLookupService.GetField(fields, definition.Id, definition.InternalName, definition.Title);
        }


        protected virtual Type GetTargetFieldType(FieldDefinition model)
        {
            return typeof(Field);
        }


        protected Site ExtractSiteFromHost(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostSite;

            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostSite;

            return null;
        }

        protected Web ExtractWebFromHost(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostWeb;

            if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb;

            if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostWeb;

            return null;
        }

        protected Site HostSite { get; set; }
        protected Web HostWeb { get; set; }

        protected object ModelHost { get; set; }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            this.ModelHost = modelHost;

            if (!(modelHost is SiteModelHost
                || modelHost is WebModelHost
                || modelHost is ListModelHost))
                throw new ArgumentException("modelHost needs to be SiteModelHost/WebModelHost/ListModelHost instance.");

            CurrentHostClientContext = (modelHost as CSOMModelHostBase).HostClientContext;
            CurrentModelHost = modelHost.WithAssertAndCast<CSOMModelHostBase>("modelHost", value => value.RequireNotNull());

            HostSite = ExtractSiteFromHost(modelHost);
            HostWeb = ExtractWebFromHost(modelHost);

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Casting field model definition");
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

            if (modelHost is ListModelHost)
            {
                var listHost = modelHost as ListModelHost;
                context = listHost.HostList.Context;

                currentField = DeployListField(modelHost as ListModelHost, fieldModel);
            }
            else if (modelHost is WebModelHost)
            {
                var webHost = modelHost as WebModelHost;
                context = webHost.HostWeb.Context;

                currentField = DeployWebField(webHost as WebModelHost, fieldModel);
            }

            else if (modelHost is SiteModelHost)
            {
                var siteHost = modelHost as SiteModelHost;
                context = siteHost.HostSite.Context;

                currentField = DeploySiteField(siteHost as SiteModelHost, fieldModel);
            }
            else
            {
                throw new ArgumentException("modelHost needs to be SiteModelHost/WebModelHost/ListModelHost instance.");
            }

            object typedField = null;

            // emulate context.CastTo<>() call for typed field type
            if (GetTargetFieldType(fieldModel) != currentField.GetType())
            {
                var targetFieldType = GetTargetFieldType(fieldModel);

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Calling context.CastTo() to field type: [{0}]", targetFieldType);

                var method = context.GetType().GetMethod("CastTo");
                var generic = method.MakeGenericMethod(targetFieldType);

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

            if (fieldModel.PushChangesToLists.HasValue)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall,
                    string.Format("UpdateAndPushChanges({0})", fieldModel.PushChangesToLists));

                currentField.UpdateAndPushChanges(fieldModel.PushChangesToLists.Value);
            }
            else
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "UpdateAndPushChanges(true)");
                // Why does SSOM handler distinguish between list and web/site fields and csom doesn't?
                currentField.UpdateAndPushChanges(true);
            }

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");
            context.ExecuteQueryWithTrace();

            CurrentHostClientContext = null;
        }

        private Field DeployWebField(WebModelHost webModelHost, FieldDefinition fieldModel)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying web field");

            var web = webModelHost.HostWeb;
            var context = web.Context;

            var field = GetField(webModelHost, fieldModel);

            return EnsureField(context, field, web.Fields, fieldModel);
        }

        private Field DeployListField(ListModelHost modelHost, FieldDefinition fieldModel)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying list field");

            var list = modelHost.HostList;
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

            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");
            context.ExecuteQueryWithTrace();

            if (!scope.HasException)
            {
                field = list.Fields.GetById(fieldModel.Id);
                context.Load(field);

                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Found site list with Id: [{0}]", fieldModel.Id);
                TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "ExecuteQuery()");

                context.ExecuteQueryWithTrace();

                return EnsureField(context, field, list.Fields, fieldModel);
            }

            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Cannot find list field with Id: [{0}]", fieldModel.Id);
            return EnsureField(context, null, list.Fields, fieldModel);
        }

        private Field DeploySiteField(SiteModelHost siteModelHost, FieldDefinition fieldModel)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "Deploying site field");

            var site = siteModelHost.HostSite;
            var context = site.Context;

            var field = GetField(siteModelHost, fieldModel);

            return EnsureField(context, field, site.RootWeb.Fields, fieldModel);
        }

        protected virtual void ProcessFieldProperties(Field field, FieldDefinition definition)
        {
            field.Title = definition.Title;

            field.Description = definition.Description ?? string.Empty;
            field.Group = string.IsNullOrEmpty(definition.Group) ? "Custom" : definition.Group;

            field.Required = definition.Required;

            if (definition.ReadOnlyField.HasValue)
                field.ReadOnlyField = definition.ReadOnlyField.Value;

            if (!string.IsNullOrEmpty(definition.StaticName))
                field.StaticName = definition.StaticName;

            if (!string.IsNullOrEmpty(definition.ValidationMessage))
                field.ValidationMessage = definition.ValidationMessage;

            if (!string.IsNullOrEmpty(definition.ValidationFormula))
                field.ValidationFormula = definition.ValidationFormula;

            if (!string.IsNullOrEmpty(definition.DefaultValue))
                field.DefaultValue = definition.DefaultValue;

            if (definition.EnforceUniqueValues.HasValue)
                field.EnforceUniqueValues = definition.EnforceUniqueValues.Value;

#if !NET35
            // Setting the index property will throw an exception for dependent lookup fields
            if (!(definition is DependentLookupFieldDefinition))
            {
                field.Indexed = definition.Indexed;
            }

            // setting up JS link seems to crash fields provision in some cases
            // Issue deploying fields to Site attached to Office 365 Group #945
            // https://github.com/SubPointSolutions/spmeta2/issues/945
            if (!string.IsNullOrEmpty(definition.JSLink))
                field.JSLink = definition.JSLink;
#endif

#if !NET35

            // NASTY CALLS
            // http://sharepoint.stackexchange.com/questions/94911/hide-a-field-in-list-when-new-item-created-but-show-it-when-edit

            if (definition.ShowInEditForm.HasValue)
            {
                field.SetShowInEditForm(definition.ShowInEditForm.Value);
                field.Context.ExecuteQueryWithTrace();
            }

            if (definition.ShowInDisplayForm.HasValue)
            {
                field.SetShowInDisplayForm(definition.ShowInDisplayForm.Value);
                field.Context.ExecuteQueryWithTrace();
            }

            if (definition.ShowInNewForm.HasValue)
            {
                field.SetShowInNewForm(definition.ShowInNewForm.Value);
                field.Context.ExecuteQueryWithTrace();
            }

#endif

            //if (definition.AllowDeletion.HasValue)
            //    field.AllowDeletion = definition.AllowDeletion.Value;

            //if (definition.ShowInListSettings.HasValue)
            //    field.ShowInListSettings = definition.ShowInListSettings.Value;

            //if (definition.ShowInViewForms.HasValue)
            //    field.ShowInViewForms = definition.ShowInViewForms.Value;

            //if (definition.ShowInVersionHistory.HasValue)
            //    field.ShowInVersionHistory = definition.ShowInVersionHistory.Value;

            ProcessLocalization(field, definition);
        }

        private Field EnsureField(ClientRuntimeContext context, Field currentField, FieldCollection fieldCollection,
            FieldDefinition fieldModel)
        {
            TraceService.Verbose((int)LogEventId.ModelProvisionCoreCall, "EnsureField()");

            if (currentField == null)
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionProcessingNewObject, "Current field is NULL. Creating new");

                var fieldDef = GetTargetSPFieldXmlDefinition(fieldModel);

                var addFieldOptions = (AddFieldOptions)(int)fieldModel.AddFieldOptions;
                var resultField = fieldCollection.AddFieldAsXml(fieldDef, fieldModel.AddToDefaultView, addFieldOptions);

                if (PreloadProperties(resultField))
                {
                    context.ExecuteQueryWithTrace();
                }

                ProcessFieldProperties(resultField, fieldModel);

                return resultField;
            }
            else
            {
                TraceService.Verbose((int)LogEventId.ModelProvisionProcessingExistingObject, "Processing existing field");

                ProcessFieldProperties(currentField, fieldModel);

                return currentField;
            }
        }

        protected virtual void HandleIncorectlyDeletedTaxonomyField(FieldDefinition fieldModel, FieldCollection fields)
        {
            return;

            // excluded due ot potential data corruption
            // such issues shoud be handled by end user manually

            //var context = fields.Context;

            //var isTaxField =
            //      fieldModel.FieldType.ToUpper() == BuiltInFieldTypes.TaxonomyFieldType.ToUpper()
            //      || fieldModel.FieldType.ToUpper() == BuiltInFieldTypes.TaxonomyFieldTypeMulti.ToUpper();

            //if (!isTaxField)
            //    return;

            //var existingIndexedFieldName = fieldModel.Title + "_0";
            //var query = from field in fields
            //            where field.Title == existingIndexedFieldName
            //            select field;


            //var result = context.LoadQuery(query);
            //context.ExecuteQueryWithTrace();

            //if (result.Count() > 0)
            //{
            //    var existingIndexedField = result.FirstOrDefault();
            //    if (existingIndexedField != null && existingIndexedField.FieldTypeKind == FieldType.Note)
            //    {
            //        // tmp fix
            //        // https://github.com/SubPointSolutions/spmeta2/issues/521
            //        try
            //        {
            //            existingIndexedField.DeleteObject();
            //            context.ExecuteQueryWithTrace();
            //        }
            //        catch (Exception e)
            //        {

            //        }
            //    }
            //}
        }

        protected virtual void ProcessSPFieldXElement(XElement fieldTemplate, FieldDefinition fieldModel)
        {
            // minimal set
            fieldTemplate
              .SetAttribute(BuiltInFieldAttributes.ID, fieldModel.Id.ToString("B"))
              .SetAttribute(BuiltInFieldAttributes.DisplayName, fieldModel.Title)
              .SetAttribute(BuiltInFieldAttributes.Title, fieldModel.Title)
              .SetAttribute(BuiltInFieldAttributes.Name, fieldModel.InternalName)
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

            // TODO: maybe this can be done in a generic way?
            if (!(fieldModel is DependentLookupFieldDefinition))
            {
                fieldTemplate.SetAttribute(BuiltInFieldAttributes.Indexed, fieldModel.Indexed.ToString().ToUpper());
            }

            // Enhance FieldDefinition provision - investigate DefaultFormula property support for CSOM #842
            // https://github.com/SubPointSolutions/spmeta2/issues/842
            if (!string.IsNullOrEmpty(fieldModel.DefaultFormula))
            {
                var defaultFormulaNode = new XElement("DefaultFormula");
                defaultFormulaNode.Value = fieldModel.DefaultFormula;

                fieldTemplate.Add(defaultFormulaNode);
            }
        }

        protected virtual string GetTargetSPFieldXmlDefinition(FieldDefinition fieldModel)
        {
            var fieldTemplate = GetNewMinimalSPFieldTemplate();

            if (!string.IsNullOrEmpty(fieldModel.RawXml))
                fieldTemplate = XDocument.Parse(fieldModel.RawXml).Root;

            ProcessSPFieldXElement(fieldTemplate, fieldModel);

            // add up additional attributes
            if (fieldModel.AdditionalAttributes.Any())
                foreach (var fieldAttr in fieldModel.AdditionalAttributes)
                    fieldTemplate.SetAttribute(fieldAttr.Name, fieldAttr.Value);

            return fieldTemplate.ToString();
        }

        protected virtual void ProcessLocalization(Field obj, FieldDefinition definition)
        {
            ProcessGenericLocalization(obj, new Dictionary<string, List<ValueForUICulture>>
            {
                { "TitleResource", definition.TitleResource },
                { "DescriptionResource", definition.DescriptionResource },
            });
        }

        #endregion
    }
}
