using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.SharePoint;
using SPMeta2.Syntax.Default.Utils;

namespace SPMeta2.Regression.SSOM.Extensions
{
    public static class SPFileExtensions
    {
        public static byte[] GetContent(this SPFile file)
        {
            byte[] result = null;

            using (var stream = file.OpenBinaryStream())
                result = ModuleFileUtils.ReadFully(stream);

            return result;
        }
    }
}
