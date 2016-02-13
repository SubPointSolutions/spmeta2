using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Definitions;

namespace SPMeta2.SSOM.Services
{
    public class SSOMFieldLookupService
    {
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
