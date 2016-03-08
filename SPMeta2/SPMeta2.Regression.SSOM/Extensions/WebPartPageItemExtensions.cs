using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Regression.SSOM.Extensions
{
    internal static class WebPartPageItemExtensions
    {
        public static byte[] GetContent(this SPListItem item)
        {
            byte[] result = null;

            using (var stream = item.File.OpenBinaryStream())
                result = ModuleFileUtils.ReadFully(stream);

            return result;
        }

        public static byte[] GetWebPartPageTemplateContent(this WebPartPageDefinition definition)
        {
            return Encoding.UTF8.GetBytes(new WebPartPageModelHandler().GetWebPartPageTemplateContent(definition));
        }

        public static byte[] GetCustomnPageContent(this WebPartPageDefinition definition)
        {
            return Encoding.UTF8.GetBytes(definition.CustomPageLayout);
        }
    }
}
