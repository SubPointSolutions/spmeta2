using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Extensions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using SPMeta2.Exceptions;

namespace SPMeta2.Services.Impl.Validation
{
    public class DefaultContentTypeLinkValidationService : PreDeploymentValidationServiceBase
    {
        public DefaultContentTypeLinkValidationService()
        {
            this.Title = "Default Content Type link validator";
            this.Description = "Ensures that list scoped content type operations such as adding, removing, hiding and reordering are done via ContentTypeName only.";
        }

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            Exceptions.Clear();

            var exceptionMessage = "List node:[{0}] has [{1}] with ContentTypeId value:[{2}]. ContentTypeId does not work on the list scoped content type operations. Use ContentTypeName for list scoped content type operations.";

            var allNodes = model.Flatten();

            //var addContentTypes = GetParenChildNodes<ListDefinition, ContentTypeLinkDefinition>(allNodes);
            var hideContentTypes = GetParenChildNodes<ListDefinition, HideContentTypeLinksDefinition>(allNodes);
            var removeContentTypes = GetParenChildNodes<ListDefinition, RemoveContentTypeLinksDefinition>(allNodes);
            var uniqueContentTypes = GetParenChildNodes<ListDefinition, UniqueContentTypeOrderDefinition>(allNodes);

            //ValidateChildNodes<ContentTypeLinkDefinition>(addContentTypes, (node, child) =>
            //{
            //    var typedDef = child.Value as ContentTypeLinkDefinition;

            //    if (!string.IsNullOrEmpty(typedDef.ContentTypeId))
            //    {
            //        Exceptions.Add(new SPMeta2ModelValidationException(
            //            string.Format(exceptionMessage,
            //            new object[]{ 
            //                    node,
            //                    typedDef.GetType(),
            //                    typedDef.ContentTypeName
            //                })));
            //    }
            //});

            ValidateChildNodes<HideContentTypeLinksDefinition>(hideContentTypes, (node, child) =>
            {
                var typedDef = child.Value as HideContentTypeLinksDefinition;

                foreach (var ctLink in typedDef.ContentTypes)
                {
                    if (!string.IsNullOrEmpty(ctLink.ContentTypeId))
                    {
                        Exceptions.Add(new SPMeta2ModelValidationException(
                            string.Format(exceptionMessage,
                            new object[]{ 
                                node,
                                typedDef.GetType(),
                                ctLink.ContentTypeId
                            })));
                    }
                }
            });

            ValidateChildNodes<RemoveContentTypeLinksDefinition>(removeContentTypes, (node, child) =>
            {
                var typedDef = child.Value as RemoveContentTypeLinksDefinition;

                foreach (var ctLink in typedDef.ContentTypes)
                {
                    if (!string.IsNullOrEmpty(ctLink.ContentTypeId))
                    {
                        Exceptions.Add(new SPMeta2ModelValidationException(
                            string.Format(exceptionMessage,
                            new object[]{ 
                                node,
                                typedDef.GetType(),
                                ctLink.ContentTypeId
                            })));
                    }
                }
            });

            ValidateChildNodes<UniqueContentTypeOrderDefinition>(uniqueContentTypes, (node, child) =>
            {
                var typedDef = child.Value as UniqueContentTypeOrderDefinition;

                foreach (var ctLink in typedDef.ContentTypes)
                {
                    if (!string.IsNullOrEmpty(ctLink.ContentTypeId))
                    {
                        Exceptions.Add(new SPMeta2ModelValidationException(
                            string.Format(exceptionMessage,
                            new object[]{ 
                                node,
                                typedDef.GetType(),
                                ctLink.ContentTypeId
                            })));
                    }
                }
            });

            if (Exceptions.Count > 0)
            {
                throw new SPMeta2ModelDeploymentException("Errors while validating the model",
                    new SPMeta2AggregateException(Exceptions.OfType<Exception>()));
            }
        }

        protected virtual void ValidateChildNodes<TChildDefinition>(IEnumerable<ModelNode> nodes,
            Action<ModelNode, ModelNode> validator)
            where TChildDefinition : DefinitionBase
        {
            foreach (var node in nodes)
            {
                foreach (var childNode in node.GetChildModels<TChildDefinition>())
                {
                    if (childNode.Value is TChildDefinition)
                        validator(node, childNode);
                }
            }
        }
    }
}
