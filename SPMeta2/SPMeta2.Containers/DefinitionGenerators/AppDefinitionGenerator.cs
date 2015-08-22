using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Resources;
using SPMeta2.Containers.Services.Base;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Enumerations;
using SPMeta2.Syntax.Default;
using SPMeta2.Containers.Consts;

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class AppDefinitionGenerator : TypedDefinitionGeneratorServiceBase<AppDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.ProductId = DefaultContainers.Apps.ProductId;
                def.Version = new Version(1, 0, 0, 0).ToString();

                def.Content = File.ReadAllBytes(DefaultContainers.Apps.v0.FilePath);
            });
        }

        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            var sideLoadFearture = BuiltInSiteFeatures.EnableAppSideLoading.Inherit(def =>
            {
                def.Enable();
            });

            return new[] { sideLoadFearture };
        }

    }

    internal static class ResourceReaderUtils
    {
        public static byte[] ReadFromResourceName(string name)
        {
            return ReadFromResourceName(Assembly.GetExecutingAssembly(), name);
        }

        public static byte[] ReadFromResourceName(Assembly asmAssembly, string name)
        {
            return ReadFully(asmAssembly.GetManifestResourceStream(name));

        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
