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

namespace SPMeta2.Containers.DefinitionGenerators
{
    public class AppDefinitionGenerator : TypedDefinitionGeneratorServiceBase<AppDefinition>
    {
        public override DefinitionBase GenerateRandomDefinition(Action<DefinitionBase> action)
        {
            return WithEmptyDefinition(def =>
            {
                def.ProductId = new Guid("{d3d5b953-e1a9-438c-a09b-b8cc4df35788}");
                def.Version = "1.0.0.0";

                def.Content = ResourceReaderUtils.ReadFromResourceName(typeof(AppDefinitionGenerator).Assembly, "SPMeta2.Containers.Templates.Apps.SPMeta2.Sandbox.TestSPHostedApp.app");
            });
        }

        public override IEnumerable<DefinitionBase> GetAdditionalArtifacts()
        {
            // TODO
            // sort out sideload feature for O365

            var sideLoadFearture = new FeatureDefinition
            {
                Scope = FeatureDefinitionScope.Site,
                Id = new Guid("AE3A1339-61F5-4f8f-81A7-ABD2DA956A7D"),
                Enable = true
            };

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
