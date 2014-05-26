using System.IO;
using System.Reflection;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Tests.Models
{
    public static class ModuleFiles
    {
        public static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];

            using (var ms = new MemoryStream())
            {
                int read;

                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                return ms.ToArray();
            }
        }

        public static byte[] FromResource(string resourceNamme)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(resourceNamme))
            {
                using (var reader = new StreamReader(stream))
                {
                    return ReadFully(reader.BaseStream);
                }
            }
        }

        #region properties

        public static ModuleFileDefinition JQuery = new ModuleFileDefinition
        {
            FileName = "jquery-1.11.0.min.js",
            Content = FromResource("SPMeta2.CSOM.Tests.Modules.js.jquery-1.11.0.min.js")
        };

        #endregion
    }
}
