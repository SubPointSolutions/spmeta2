using SPMeta2.Definitions;
using System;
using System.Collections.Generic;

using Microsoft.SharePoint.Client;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.CSOM.Extensions;

namespace SPMeta2.CSOM.Services
{
    public class CSOMFieldLookupService
    {
        public virtual FieldCollection GetFieldCollection(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb.Fields;
            else if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb.Fields;
            else if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.Fields;

            throw new SPMeta2Exception("Unsupported model host");
        }

        public virtual Field GetField(FieldCollection fields, Guid? fieldId, string fieldInternalName, string fieldTitle)
        {
            if (fieldId.HasGuidValue())
                return fields.GetById(fieldId.Value);

            if (!string.IsNullOrEmpty(fieldInternalName))
                return fields.GetByInternalNameOrTitle(fieldInternalName);

            if (!string.IsNullOrEmpty(fieldTitle))
                return fields.GetByTitle(fieldTitle);

            throw new SPMeta2Exception("GetField(): on of fieldId / fieldInternalName / fieldTitle needs to be defined");
        }

        public virtual T GetFieldAs<T>(FieldCollection fields, Guid? fieldId, string fieldInternalName, string fieldTitle) where T : Field
        {
            var field = GetField(fields, fieldId, fieldInternalName, fieldTitle);

            return fields.Context.CastTo<T>(field);
        }

        public virtual Field FindField(FieldCollection fields, Guid? fieldId, string fieldInternalName, string fieldTitle)
        {
            var context = fields.Context;

            var scope = new ExceptionHandlingScope(context);

            Field field = null;

            if (fieldId.HasGuidValue())
            {
                var id = fieldId.Value;

                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        fields.GetById(id);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }
            }
            else if (!string.IsNullOrEmpty(fieldInternalName))
            {
                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        fields.GetByInternalNameOrTitle(fieldInternalName);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }
            }
            else if (!string.IsNullOrEmpty(fieldTitle))
            {
                using (scope.StartScope())
                {
                    using (scope.StartTry())
                    {
                        fields.GetByTitle(fieldTitle);
                    }

                    using (scope.StartCatch())
                    {

                    }
                }
            }

            context.ExecuteQueryWithTrace();

            if (!scope.HasException)
            {
                if (fieldId.HasGuidValue())
                {
                    field = fields.GetById(fieldId.Value);
                }
                else if (!string.IsNullOrEmpty(fieldInternalName))
                {
                    field = fields.GetByInternalNameOrTitle(fieldInternalName);
                }
                else if (!string.IsNullOrEmpty(fieldTitle))
                {
                    field = fields.GetByTitle(fieldTitle);
                }

                context.Load(field);
                context.Load(field, f => f.SchemaXml);

                context.ExecuteQueryWithTrace();
            }

            return field;
        }

        public virtual void EnsureDefaultValues(ListItem item, List<FieldValue> defaultValues)
        {
            EnsureValues(item, defaultValues, false);
        }

        public virtual void EnsureValues(ListItem item, List<FieldValue> defaultValues,
            bool shouldOverwrite)
        {
            foreach (var defaultValue in defaultValues)
            {
                if (!string.IsNullOrEmpty(defaultValue.FieldName))
                {
                    if (item.FieldValues.ContainsKey(defaultValue.FieldName))
                    {
                        if (!shouldOverwrite)
                        {
                            if (item[defaultValue.FieldName] == null)
                                item[defaultValue.FieldName] = defaultValue.Value;
                        }
                        else
                        {
                            item[defaultValue.FieldName] = defaultValue.Value;
                        }
                    }
                    else
                    {
                        item[defaultValue.FieldName] = defaultValue.Value;
                    }
                }
                else if (defaultValue.FieldId.HasValue && defaultValue.FieldId != default(Guid))
                {
                    // unsupported by CSOM API yet
                }
            }
        }
    }
}
