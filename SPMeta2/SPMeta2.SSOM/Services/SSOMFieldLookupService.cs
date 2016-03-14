using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Utils;
using SPMeta2.Exceptions;
using SPMeta2.SSOM.ModelHosts;

namespace SPMeta2.SSOM.Services
{
    public class SSOMFieldLookupService
    {
        public virtual SPField GetField(SPFieldCollection fields, Guid? fieldId, string fieldInternalName, string fieldTitle)
        {
            if (fieldId.HasGuidValue())
                return fields[fieldId.Value];

            if (!string.IsNullOrEmpty(fieldInternalName))
                return fields.GetFieldByInternalName(fieldInternalName);

            if (!string.IsNullOrEmpty(fieldTitle))
                return fields.GetField(fieldTitle);

            throw new SPMeta2Exception("GetField(): one of fieldId / fieldInternalName / fieldTitle need to be defined");
        }

        public virtual T GetFieldAs<T>(SPFieldCollection fields, Guid? fieldId, string fieldInternalName, string fieldTitle) where T : SPField
        {
            var field = GetField(fields, fieldId, fieldInternalName, fieldTitle);

            return field as T;
        }

        public virtual SPFieldCollection GetFieldCollection(object modelHost)
        {
            if (modelHost is SiteModelHost)
                return (modelHost as SiteModelHost).HostSite.RootWeb.Fields;
            else if (modelHost is WebModelHost)
                return (modelHost as WebModelHost).HostWeb.Fields;
            else if (modelHost is ListModelHost)
                return (modelHost as ListModelHost).HostList.Fields;

            throw new SPMeta2Exception("Unsupported model host");
        }

        public virtual void EnsureDefaultValues(SPListItem item, List<FieldValue> defaultValues)
        {
            EnsureValues(item, defaultValues, false);
        }

        public virtual void EnsureValues(SPListItem item,
            List<FieldValue> defaultValues, bool shouldOverride)
        {
            foreach (var defaultValue in defaultValues)
            {
                if (!string.IsNullOrEmpty(defaultValue.FieldName))
                {
                    if (item.Fields.ContainsFieldWithStaticName(defaultValue.FieldName))
                    {
                        if (!shouldOverride)
                        {
                            if (item[defaultValue.FieldName] == null)
                                item[defaultValue.FieldName] = defaultValue.Value;
                        }
                        else
                        {
                            item[defaultValue.FieldName] = defaultValue.Value;
                        }
                    }
                }
                else if (defaultValue.FieldId.HasValue && defaultValue.FieldId != default(Guid))
                {
                    if (item.Fields.OfType<SPField>().Any(f => f.Id == defaultValue.FieldId.Value))
                    {
                        if (!shouldOverride)
                        {
                            if (item[defaultValue.FieldId.Value] == null)
                                item[defaultValue.FieldId.Value] = defaultValue.Value;
                        }
                        else
                        {
                            item[defaultValue.FieldId.Value] = defaultValue.Value;
                        }
                    }
                }
            }
        }
    }
}
