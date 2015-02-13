using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace SPMeta2.Regression.Tests.Utils
{
    internal static class ResourceReaderUtils
    {
        public static string ReadFromResourceName(string name)
        {
            return ReadFromResourceName(Assembly.GetExecutingAssembly(), name);
        }

        public static string ReadFromResourceName(Assembly asmAssembly, string name)
        {
            using (var reader = new StreamReader(asmAssembly.GetManifestResourceStream(name)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
