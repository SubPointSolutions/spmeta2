using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Reflects built-in SharePoint AddFieldOptions class.
    /// </summary>
    public enum BuiltInAddFieldOptions
    {
        DefaultValue = 0,
        AddToDefaultContentType = 1,
        AddToNoContentType = 2,
        AddToAllContentTypes = 4,
        AddFieldInternalNameHint = 8,
        AddFieldToDefaultView = 16,
        AddFieldCheckDisplayName = 32,
    }
}
