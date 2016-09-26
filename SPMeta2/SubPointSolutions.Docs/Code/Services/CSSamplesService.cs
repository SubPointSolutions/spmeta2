using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SubPointSolutions.Docs.Code.Data;
using SubPointSolutions.Docs.Services.Base;
using SubPointSolutions.Docs.Code.Metadata;
using SubPointSolutions.Docs.Code.API;
using SubPointSolutions.Docs.Code.Enumerations;

namespace SubPointSolutions.Docs.Code.Services
{
    public class CSClassSamplesService : CSSamplesService
    {
        #region methods
        public override IEnumerable<DocSample> CreateSamplesFromSourceFile(string filePath)
        {
            var result = new List<DocSample>();

            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
            var treeRoot = tree.GetRoot() as CompilationUnitSyntax;

            var csClasses = tree.GetRoot()
                                .DescendantNodes()
                                .OfType<ClassDeclarationSyntax>()
                                .ToList();

            foreach (var csClass in csClasses)
            {
                var className = csClass.Identifier.ToString();
                var classComment = string.Empty;

                var trivia = csClass.GetLeadingTrivia();

                if (trivia != null)
                {
                    var commentXml = trivia.ToString();

                    try
                    {
                        classComment = XElement.Parse(trivia.ToString()
                                                            .Replace(@"///", string.Empty)
                                                            .Trim())
                                          .FirstNode
                                          .ToString()
                                          .Trim()
                                          .Replace("     ", "");
                    }
                    catch (Exception)
                    {

                    }
                }

                var namespaceName = (csClass.Parent as NamespaceDeclarationSyntax).Name.ToString();

                var sample = new DocSample();

                sample.IsClass = true;
                sample.IsMethod = false;

                // namespace
                sample.Namespace = namespaceName;
                sample.Language = "cs";

                // class level
                sample.ClassName = className;
                sample.ClassFullName = namespaceName + "." + className;

                sample.ClassComment = classComment;


                var classBody = "    " + csClass.ToString();

                // cleaning up attributes
                foreach (var classAttr in csClass.AttributeLists.ToList())
                {
                    classBody = classBody.Replace(classAttr.ToString(), string.Empty);
                }

                // method
                sample.MethodBodyWithFunction = classBody;
                sample.MethodBody = classBody;

                sample.MethodName = className + "Class";
                sample.MethodFullName = "Class" + sample.MethodName;

                sample.SourceFileName = Path.GetFileName(filePath);
                sample.SourceFileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                sample.SourceFileFolder = Path.GetDirectoryName(filePath);
                sample.SourceFilePath = filePath;

                // TODO, metadata
                sample.Title = string.Empty;
                sample.Description = string.Empty;

                // load from SampleMetadata attr
                var instanceType = Type.GetType(sample.ClassFullName);
                var method = instanceType;

                if (method != null)
                {
                    var methodMetadata = method.GetCustomAttributes(typeof(SampleMetadataAttribute), false)
                                               .FirstOrDefault() as SampleMetadataAttribute;

                    if (methodMetadata != null)
                    {
                        sample.Title = methodMetadata.Title;
                        sample.Description = methodMetadata.Description;
                    }
                    else
                    {
                        // fallbak on the method name
                        sample.Title = method.Name;
                    }

                    // tags


                    var sampleTags = (method.GetCustomAttributes(typeof(SampleMetadataTagAttribute), false)
                                           as SampleMetadataTagAttribute[]).ToList();


                    // addint top-class tags
                    sampleTags.AddRange(instanceType.GetCustomAttributes(typeof(SampleMetadataTagAttribute), false)
                                           as SampleMetadataTagAttribute[]);


                    foreach (var tagNames in sampleTags.GroupBy(tag => tag.Name))
                    {
                        var newTag = new DocSampleTag
                        {
                            Name = tagNames.Key,
                            Values = tagNames.Select(t => t.Value).ToList()
                        };

                        sample.Tags.Add(newTag);
                    }
                }

                result.Add(sample);
                //}
            }


            return result;
        }
        #endregion
    }

    public class CSSamplesService : SamplesServiceBase
    {
        #region constructors

        public CSSamplesService()
        {
            FileExtension = "*.cs";
        }

        #endregion

        #region methods
        public override IEnumerable<DocSample> CreateSamplesFromSourceFile(string filePath)
        {
            var result = new List<DocSample>();

            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
            var treeRoot = tree.GetRoot() as CompilationUnitSyntax;

            var csClasses = tree.GetRoot()
                                .DescendantNodes()
                                .OfType<ClassDeclarationSyntax>()
                                .ToList();

            foreach (var csClass in csClasses)
            {
                var className = csClass.Identifier.ToString();
                var classComment = string.Empty;

                var trivia = csClass.GetLeadingTrivia();

                if (trivia != null)
                {
                    var commentXml = trivia.ToString();

                    try
                    {
                        classComment = XElement.Parse(trivia.ToString()
                                                            .Replace(@"///", string.Empty)
                                                            .Trim())
                                          .FirstNode
                                          .ToString()
                                          .Trim()
                                          .Replace("     ", "");
                    }
                    catch (Exception)
                    {

                    }
                }

                // methods
                var csMethods = csClass
                                  .DescendantNodes()
                                  .OfType<MethodDeclarationSyntax>().ToList();

                foreach (var csMethod in csMethods)
                {
                    var namespaceName = ((csMethod.Parent as ClassDeclarationSyntax).Parent as NamespaceDeclarationSyntax).Name.ToString();

                    var methodName = csMethod.Identifier.Text;
                    var methodFullName = namespaceName + "." + className + "." + methodName;

                    // cleaning up attributes
                    var methodBodyWithFuntion = csMethod.ToString();

                    foreach (var methodAttr in csMethod.AttributeLists.ToList())
                    {
                        methodBodyWithFuntion = methodBodyWithFuntion.Replace(methodAttr.ToString(), string.Empty);
                    }

                    var sample = new DocSample();

                    // namespace
                    sample.Namespace = namespaceName;
                    sample.Language = "cs";

                    // class level
                    sample.ClassName = className;
                    sample.ClassFullName = namespaceName + "." + className;

                    sample.ClassComment = classComment;

                    // method
                    sample.MethodBodyWithFunction = methodBodyWithFuntion;
                    sample.MethodName = methodName;
                    sample.MethodFullName = methodFullName;

                    sample.MethodParametersCount = csMethod.ParameterList.ChildNodes().Count();

                    var hasOverload = csMethods.Count(m => m.Identifier.Text == methodName) > 1;

                    if (hasOverload)
                    {
                        sample.MethodName = methodName + "_" + sample.MethodParametersCount + "_Params";
                    }

                    sample.MethodBody = csMethod.Body
                                                .ToString()
                                                .TrimStart('{')
                                                .TrimEnd('}');




                    sample.SourceFileName = Path.GetFileName(filePath);
                    sample.SourceFileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                    sample.SourceFileFolder = Path.GetDirectoryName(filePath);
                    sample.SourceFilePath = filePath;

                    // TODO, metadata
                    sample.Title = string.Empty;
                    sample.Description = string.Empty;

                    // load from SampleMetadata attr
                    var instanceType = Type.GetType(sample.ClassFullName);
                    var method = instanceType.GetMethods().FirstOrDefault(m => m.Name == methodName);

                    if (method != null)
                    {
                        var methodMetadata = method.GetCustomAttributes(typeof(SampleMetadataAttribute), false)
                                                   .FirstOrDefault() as SampleMetadataAttribute;

                        if (methodMetadata != null)
                        {
                            sample.Title = methodMetadata.Title;
                            sample.Description = methodMetadata.Description;
                        }
                        else
                        {
                            // fallbak on the method name
                            sample.Title = method.Name;
                        }

                        // tags


                        var sampleTags = (method.GetCustomAttributes(typeof(SampleMetadataTagAttribute), false)
                                               as SampleMetadataTagAttribute[]).ToList();


                        // addint top-class tags
                        sampleTags.AddRange(instanceType.GetCustomAttributes(typeof(SampleMetadataTagAttribute), false)
                                               as SampleMetadataTagAttribute[]);


                        foreach (var tagNames in sampleTags.GroupBy(tag => tag.Name))
                        {
                            var newTag = new DocSampleTag
                            {
                                Name = tagNames.Key,
                                Values = tagNames.Select(t => t.Value).ToList()
                            };

                            sample.Tags.Add(newTag);
                        }
                    }

                    // use full body?
                    if (SamplesAPI.HasTag(sample, BuiltInTagNames.UseFullMethodBody))
                    {
                        sample.MethodBody = sample.MethodBodyWithFunction;
                    }


                    result.Add(sample);
                }
            }


            return result;
        }
        #endregion
    }
}
