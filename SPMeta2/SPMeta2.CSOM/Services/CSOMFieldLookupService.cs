using SPMeta2.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.Services
{
    public class CSOMFieldLookupService
    {
        public virtual void EnsureDefaultValues(ListItem item, List<FieldValue> defaultValues)
        {
            foreach (var defaultValue in defaultValues)
            {
                if (!string.IsNullOrEmpty(defaultValue.FieldName))
                {
                    if (item.FieldValues.ContainsKey(defaultValue.FieldName))
                    {
                        if (item[defaultValue.FieldName] == null)
                            item[defaultValue.FieldName] = defaultValue.Value;
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
