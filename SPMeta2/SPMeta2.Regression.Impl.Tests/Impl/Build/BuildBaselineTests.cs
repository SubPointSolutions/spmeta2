using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.ModelHandlers;
using SPMeta2.Services.Impl;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Utils;
using SPMeta2.Regression.Utils;
using System.IO;

namespace SPMeta2.Regression.Impl.Tests.Impl.Build
{
    [DataContract]
    public class BuildBaseline
    {
        public BuildBaseline()
        {
            DefinitionTypeFullNames = new List<string>();
            ModelHandlerTypeFullNames = new List<string>();
        }

        [DataMember]
        public string AssemblyFullName { get; set; }

        [DataMember]
        public List<String> DefinitionTypeFullNames { get; set; }

        [DataMember]
        public List<String> ModelHandlerTypeFullNames { get; set; }

        [DataMember]
        public string AssemblyFileName { get; set; }
    }

    [TestClass]
    public class BuildBaselineTests
    {


        [TestMethod]
        [TestCategory("CI.Core")]
        public void Create_BuildBaseline()
        {
            var assemblies = new List<Assembly>();

            // SPMeta2
            // SPMeta2.Standard
            assemblies.Add(typeof(FieldDefinition).Assembly);
            assemblies.Add(typeof(TaxonomyFieldDefinition).Assembly);

            // SPMeta2.CSOM
            // SPMeta2.CSOM.Standard
            assemblies.Add(typeof(SPMeta2.CSOM.ModelHandlers.FieldModelHandler).Assembly);
            assemblies.Add(typeof(SPMeta2.CSOM.Standard.ModelHandlers.Fields.TaxonomyFieldModelHandler).Assembly);

            // SPMeta2.SSOM
            // SPMeta2.SSOM.Standard
            assemblies.Add(typeof(SPMeta2.SSOM.ModelHandlers.FieldModelHandler).Assembly);
            assemblies.Add(typeof(SPMeta2.SSOM.Standard.ModelHandlers.Fields.TaxonomyFieldModelHandler).Assembly);

            var result = new List<BuildBaseline>();

            foreach (var assembly in assemblies)
            {
                result.Add(BuildBaselineFromAssembly(assembly));
            }

            var fileName = "m2.buildbaseline.xml";

            var xmlService = new DefaultXMLSerializationService();
            xmlService.RegisterKnownType(typeof(BuildBaseline));

            var content = xmlService.Serialize(result);

            RegressionUtils.WriteLine(content);

            var paths = new[]
            {
                System.IO.Path.GetFullPath(fileName),
                System.IO.Path.GetFullPath("../../../SPMeta2.Build/" + fileName)
            };

            foreach (var path in paths)
            {
                var dirPath = Path.GetDirectoryName(path);
                Directory.CreateDirectory(dirPath);

                System.IO.File.WriteAllText(path, content);
            }
        }

        private BuildBaseline BuildBaselineFromAssembly(Assembly assembly)
        {
            var result = new BuildBaseline();

            result.AssemblyFullName = assembly.FullName;
            result.AssemblyFileName = assembly.ManifestModule.Name;

            result.DefinitionTypeFullNames = ReflectionUtils.GetTypesFromAssembly<DefinitionBase>(assembly)
                                                            .Where(t => !t.IsAbstract)
                                                            .Select(t => t.AssemblyQualifiedName)
                                                            .ToList();

            result.ModelHandlerTypeFullNames = ReflectionUtils.GetTypesFromAssembly<ModelHandlerBase>(assembly)
                                                              .Where(t => !t.IsAbstract)
                                                              .Select(t => t.AssemblyQualifiedName)
                                                              .ToList();

            RegressionUtils.WriteLine(string.Format("AssemblyFullName:[{0}]", result.AssemblyFullName));
            RegressionUtils.WriteLine(string.Format("DefinitionTypeFullNames:[{0}]", result.DefinitionTypeFullNames.Count));
            RegressionUtils.WriteLine(string.Format("ModelHandlerTypeFullNames:[{0}]", result.ModelHandlerTypeFullNames.Count));

            RegressionUtils.WriteLine("Definitions:");
            foreach (var name in result.DefinitionTypeFullNames)
                RegressionUtils.WriteLine(" " + name);

            RegressionUtils.WriteLine("Model handlers:");
            foreach (var name in result.ModelHandlerTypeFullNames)
                RegressionUtils.WriteLine(" " + name);

            RegressionUtils.WriteLine(string.Empty);
            RegressionUtils.WriteLine(string.Empty);

            return result;
        }
    }
}
