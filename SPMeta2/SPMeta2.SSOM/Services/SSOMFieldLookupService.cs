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
            SPField result = null;

            if (fieldId.HasGuidValue())
            {
                result = fields.OfType<SPField>().FirstOrDefault(f => f.Id == fieldId.Value);
            }

            if (result == null && !string.IsNullOrEmpty(fieldInternalName))
            {
                result = fields.OfType<SPField>()
                                           .FirstOrDefault(f => String.Equals(f.InternalName, fieldInternalName,
                                                                StringComparison.OrdinalIgnoreCase));
            }

            if (result == null && !string.IsNullOrEmpty(fieldTitle))
            {
                result = fields.OfType<SPField>()
                                           .FirstOrDefault(f => String.Equals(f.Title, fieldTitle,
                                                                StringComparison.OrdinalIgnoreCase));
            }

            if (result != null)
            {
                return result;
            }

            throw new SPMeta2Exception(string.Format("SSOMFieldLookupService.GetField(): cannot find field by fieldId:[{0}],  fieldInternalName:[{1}], fieldTitle:[{2}]",
                fieldId, fieldInternalName, fieldTitle));
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
