using System.IO;
using System.Reflection;

namespace SPMeta2.Utils
{
    public static class ResourceUtils
    {
        public static string ReadFromResourceName(string name)
        {
            return ReadFromResourceName(Assembly.GetExecutingAssembly(), name);
        }

        public static string ReadFromResourceName(Assembly asm, string name)
        {
            using (var reader = new StreamReader(asm.GetManifestResourceStream(name)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
