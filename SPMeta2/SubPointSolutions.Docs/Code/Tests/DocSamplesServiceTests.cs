using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Utils;
using SubPointSolutions.Docs.Code.Data;
using SubPointSolutions.Docs.Code.Services;
using SubPointSolutions.Docs.Code.Utils;
using SubPointSolutions.Docs.Services;
using SubPointSolutions.Docs.Services.Base;

namespace SubPointSolutions.Docs.Code.Tests
{
    [TestClass]
    public class DocSamplesServiceTests
    {
        #region constructors

        public DocSamplesServiceTests()
        {

        }

        #endregion

        #region classes

        private class SampleMetadata
        {
            public SampleMetadata()
            {
                Resursive = true;
            }

            public string ContentFolderPath { get; set; }
            public string StaticClassName { get; set; }

            public bool Resursive { get; set; }
        }

        #endregion

        #region samples cache

        private List<DocSample> GetAllSamples(string path, bool resursive)
        {
            var result = new List<DocSample>();

            var services = ReflectionUtils.GetTypesFromAssembly<SamplesServiceBase>(GetType().Assembly);

            foreach (var service in services.Select(a => Activator.CreateInstance(a) as SamplesServiceBase))
            {
                result.AddRange(service.LoadSamples(path, resursive));
            }

            return result;
        }


        [TestMethod]
        [TestCategory("Dev.Source")]
        public void Generate_Sample_Ref_Files()
        {
            Can_Create_CS_Samples();

            var docsPrj = @"..\..\Views";
            var allSamples = GetAllSamples(docsPrj, true);

            // sys
            foreach (var sampleFolder in allSamples.GroupBy(s => s.SourceFileFolder))
            {
                foreach (var sampleFiles in sampleFolder.GroupBy(s => s.SourceFileName))
                {
                    var sample = sampleFiles.First();

                    var directoryPath = Path.Combine(sample.SourceFileFolder, "_samples");
                    Directory.CreateDirectory(directoryPath);

                    var sampleFileName = string.Format("{0}-SysAll.sample-ref",
                        sample.SourceFileNameWithoutExtension);

                    var sampleFilePath = Path.Combine(directoryPath, sampleFileName);

                    File.WriteAllText(sampleFilePath, "ref");
                }
            }

            // one-by-one
            foreach (var sample in allSamples)
            {
                var sampleBody = sample.MethodBody;

                var sampleFileName = string.Format("{0}-{1}.sample-ref",
                                    sample.SourceFileNameWithoutExtension,
                                    sample.MethodName);

                var directoryPath = Path.Combine(sample.SourceFileFolder, "_samples");
                Directory.CreateDirectory(directoryPath);

                var sampleFilePath = Path.Combine(directoryPath, sampleFileName);

                try
                {
                    File.WriteAllText(sampleFilePath, sampleBody);
                }
                catch (PathTooLongException e)
                {

                }
            }
        }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("Regression")]
        [TestCategory("Dev.Source")]
        public void Can_Create_JS_Samples()
        {
            var docsPrj = @"..\..\Views";
            var service = new HtmlSamplesService();

            var samples = service.LoadSamples(docsPrj, true);

            foreach (var sample in samples)
            {
                Trace.WriteLine(sample.ClassFullName);
                Trace.WriteLine(sample.ClassName);
                Trace.WriteLine(sample.Language);

                Trace.WriteLine(sample.MethodFullName);
                Trace.WriteLine(sample.MethodName);

                Trace.WriteLine(sample.MethodBody);
                Trace.WriteLine(string.Empty);
            }
        }

        [TestMethod]
        [TestCategory("Regression")]
        [TestCategory("Dev.Source")]
        public void Can_Create_XML_Samples()
        {
            var docsPrj = @"..\..\Views";
            var service = new XmlSamplesService();

            var samples = service.LoadSamples(docsPrj, true);

            foreach (var sample in samples)
            {
                Trace.WriteLine(sample.ClassFullName);
                Trace.WriteLine(sample.ClassName);
                Trace.WriteLine(sample.Language);

                Trace.WriteLine(sample.MethodFullName);
                Trace.WriteLine(sample.MethodName);

                Trace.WriteLine(sample.MethodBody);
                Trace.WriteLine(string.Empty);
            }
        }

        [TestMethod]
        [TestCategory("Dev.Source")]
        public void Can_Create_CS_Samples()
        {
            // http://stackoverflow.com/questions/29567489/can-roslyn-generate-source-code-from-an-object-instance

            var samples = new List<SampleMetadata>();

            samples.Add(new SampleMetadata
            {
                ContentFolderPath = @"..\..\Views\SPMeta2",
                StaticClassName = "m2Samples"
            });

            //samples.Add(new SampleMetadata
            //{
            //    ContentFolderPath = @"..\..\Views\reSP",
            //    StaticClassName = "reSPSamples"
            //});

            //samples.Add(new SampleMetadata
            //{
            //    ContentFolderPath = @"..\..\Views\SPMeta2-Spec",
            //    StaticClassName = "m2SpecSamples"
            //});

            //samples.Add(new SampleMetadata
            //{
            //    ContentFolderPath = @"..\..\Views\SPMeta2-VSExtensions",
            //    StaticClassName = "m2ExtSamples"
            //});

            foreach (var sample in samples)
            {
                var fileContent = GenerateSampleMethodCSFile(
                                        sample.ContentFolderPath,
                                        sample.Resursive,
                                        "SubPointSolutions.Docs.Code.Samples",
                                        sample.StaticClassName);

                var folder = @"..\..\Code\Samples\";
                var file = folder + sample.StaticClassName + ".cs";

                File.WriteAllText(file, fileContent);
            }
        }

        #endregion

        #region utils

        private string GenerateSampleMethodCSFile(
                string path,
                bool recursive,
                string namespaceName,
                string staticClassName)
        {
            //var service = new CSSamplesService();
            //var samples = service.LoadSamples(path, recursive);

            var samples = GetAllSamples(path, recursive);

            var csFile = new StringBuilder();

            csFile.AppendLine("using System;");
            csFile.AppendLine("using System.Text;");
            csFile.AppendLine("using System.Linq;");

            csFile.AppendLine(string.Empty);

            csFile.AppendLine("namespace " + namespaceName);
            csFile.AppendLine("{");

            csFile.AppendLine(string.Format("    public static class {0}", staticClassName));
            csFile.AppendLine("    {");

            // all samples at all

            var tmpSample = samples.First();
            var classFullName = tmpSample.GetType().Namespace + "." + tmpSample.GetType().Name;

            var allSampleInstances = samples.Select(s => GetSampleInstanceAsString(s));
            csFile.AppendLine(string.Format("            public static System.Collections.Generic.List<{0}> SysAllSamples = new System.Collections.Generic.List<{0}>(new {0}[] {{ ", classFullName));
            csFile.AppendLine(string.Join("            ," + Environment.NewLine, allSampleInstances));
            csFile.AppendLine(string.Format("            }});"));

            // by class name
            foreach (var group in samples.GroupBy(s => s.ClassName))
            {
                var className = group.Key;

                csFile.AppendLine(string.Format("        public static class {0}", className));

                csFile.AppendLine("        {");

                // all
                var allInstances = group.Select(s => GetSampleInstanceAsString(s));

                csFile.AppendLine(string.Format("            public static System.Collections.Generic.List<{0}> SysAllSamples = new System.Collections.Generic.List<{0}>(new {0}[] {{ ", classFullName));
                csFile.AppendLine(string.Join("            ," + Environment.NewLine, allInstances));
                csFile.AppendLine(string.Format("            }});"));


                // individual
                foreach (var sample in group)
                {
                    csFile.AppendLine(string.Format("            public static {1} {0} = {2};",
                        sample.MethodName,
                        classFullName,
                        GetSampleInstanceAsString(sample)));
                }

                csFile.AppendLine("        }");
                csFile.AppendLine(string.Empty);
            }

            csFile.AppendLine("    }");

            csFile.AppendLine("}");

            return csFile.ToString();
        }

        private string GetSampleInstanceAsString(DocSample sample)
        {
            var classXmlString = StringUtils.ToLiteral(sample.ToXml());

            return string.Format("{0}.FromXml(\"{1}\")",
                sample.GetType().Namespace + "." + sample.GetType().Name,
                classXmlString);
        }

        #endregion
    }
}
