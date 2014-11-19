using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Definitions.Fields;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Utils;
using System.Collections.Generic;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.Dependencies
{
    [TestClass]
    public class ArtifactDependenciesTest : SPMeta2RegresionEventsTestBase
    {
        public ArtifactDependenciesTest()
        {
            ProvisionGenerationCount = 2;
        }

        #region common

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        [TestMethod]
        [TestCategory("Regression.Dependencies")]
        public void SiteFields_Before_SiteContentTypes()
        {
            var fieldDefinitionTypes = new List<Type>();

            // foundation defs
            fieldDefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<FieldDefinition>(typeof(FieldDefinition).Assembly));

            // standard defs
            fieldDefinitionTypes.AddRange(ReflectionUtils.GetTypesFromAssembly<FieldDefinition>(typeof(TaxonomyFieldDefinition).Assembly));

            var fieldDefinitions = new List<FieldDefinition>();

            foreach (var fieldDefinitionType in fieldDefinitionTypes)
                fieldDefinitions.Add(ModelGeneratorService.GetRandomDefinition(fieldDefinitionType) as FieldDefinition);

            var contentTypeDefinition = ModelGeneratorService.GetRandomDefinition<ContentTypeDefinition>();

            var siteModel = SPMeta2Model
                .NewSiteModel(site =>
                {
                    foreach (var fieldDefinition in fieldDefinitions)
                        site.AddField(fieldDefinition);

                    site.AddContentType(contentTypeDefinition, contentType =>
                    {
                        foreach (var fieldDefinition in fieldDefinitions)
                            contentType.AddContentTypeFieldLink(fieldDefinition);
                    });
                });

            TestModel(siteModel);
        }

        #endregion
    }
}
