using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Services;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Docs.ProvisionSamples.Base;
using SPMeta2.Docs.ProvisionSamples.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Syntax.Default;
using System;
using System.Linq;
using System.Collections.Generic;
using SubPointSolutions.Docs.Code.Metadata;
using SubPointSolutions.Docs.Code.Enumerations;

namespace SPMeta2.Docs.ProvisionSamples.Provision.Definitions
{
    [TestClass]
    public class CustomSyntax : ProvisionTestBase
    {
        #region methods

        [TestMethod]
        [TestCategory("Docs.Models")]
        public void HideContentTypeFieldsAsOOTB()
        {
            var listContentType = new ContentTypeDefinition
            {
                Name = "Content Type With Hidden Fields",
                Id = new Guid("e71cdcc9-5765-47ea-8879-b9456d57dfa6"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = "SPMeta2.Samples"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                   .AddContentType(listContentType, contentType =>
                   {
                       contentType
                           .AddHideContentTypeFieldLinks(new HideContentTypeFieldLinksDefinition
                           {
                               Fields = new System.Collections.Generic.List<FieldLinkValue>
                               {
                                   new FieldLinkValue { Id = BuiltInFieldId.Title  },
                                   new FieldLinkValue { Id = BuiltInFieldId.Comment  },
                               }
                           });
                   });
            });

            DeployModel(model);
        }

        public void HideContentTypeFieldsAsExtension()
        {
            var listContentType = new ContentTypeDefinition
            {
                Name = "Content Type With Hidden Fields",
                Id = new Guid("e71cdcc9-5765-47ea-8879-b9456d57dfa6"),
                ParentContentTypeId = BuiltInContentTypeId.Item,
                Group = "SPMeta2.Samples"
            };

            var model = SPMeta2Model.NewSiteModel(site =>
            {
                site
                   .AddContentType(listContentType, contentType =>
                   {
                       contentType
                           .HideContentTypeFieldsByIds(new Guid[] { 
                               BuiltInFieldId.Title,
                               BuiltInFieldId.Comment
                            });

                   });
            });

            DeployModel(model);
        }

        #endregion
    }

    public static class HideContentTypeFieldsExtensions
    {
        [SampleMetadataTagAttribute(Name = BuiltInTagNames.UseFullMethodBody)]
        public static ContentTypeModelNode HideContentTypeFieldsByIds(
            this ContentTypeModelNode modelNode,
            IEnumerable<Guid> ids)
        {
            modelNode.AddHideContentTypeFieldLinks(new HideContentTypeFieldLinksDefinition
            {
                Fields = new List<FieldLinkValue>(ids.Select(s =>
                    new FieldLinkValue
                    {
                        Id = s
                    }))
            });

            return modelNode;
        }

        [SampleMetadataTagAttribute(Name = BuiltInTagNames.UseFullMethodBody)]
        public static ModelNode SyntaxExtensionPrototype(this ModelNode modelNode)
        {
            // do stuff


            // !!! always retun the same model which was passed as 'this' object !!!
            return modelNode;
        }
    }
}