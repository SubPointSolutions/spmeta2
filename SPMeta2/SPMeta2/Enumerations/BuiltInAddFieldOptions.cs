using System;

namespace SPMeta2.Enumerations
{
    /// <summary>
    /// Reflects built-in SharePoint AddFieldOptions class.
    /// </summary>

    [Flags]
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
