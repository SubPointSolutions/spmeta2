using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Extensions;
using SPMeta2.ModelHosts;
using SPMeta2.Models;

namespace SPMeta2.Services.Impl.Validation
{
    public class ContentTypeLinkDeploymentValidationService : PreDeploymentValidationServiceBase
    {
        protected class ValidationPair
        {
            public ValidationPair()
            {
                ModelNodes = new List<ModelNode>();
            }

            public ModelNode ListModelNode { get; set; }
            public List<ModelNode> ModelNodes { get; set; }
        }

        public override void DeployModel(ModelHostBase modelHost, ModelNode model)
        {
            var allNodes = model.Flatten();

            var addContentTypes = allNodes.Where(n => n.Value is ListDefinition)
                                          .Select(listNode => new ValidationPair
                                          {
                                              ListModelNode = listNode,
                                              ModelNodes = listNode.Flatten().Where(n => n.Value is ContentTypeLinkDefinition).ToList()
                                          }).ToList();

            var hideContentTypes = allNodes.Where(n => n.Value is ListDefinition)
                                          .SelectMany(listNode => listNode.Flatten().Where(n => n.Value is HideContentTypeLinksDefinition))
                                          .ToList();

            var removeContentTypes = allNodes.Where(n => n.Value is ListDefinition)
                                          .SelectMany(listNode => listNode.Flatten().Where(n => n.Value is RemoveContentTypeFieldLinksDefinition))
                                          .ToList();

            var reorderContentTypes = allNodes.Where(n => n.Value is ListDefinition)
                                          .SelectMany(listNode => listNode.Flatten().Where(n => n.Value is UniqueContentTypeOrderDefinition))
                                          .ToList();


            // TODO
        }
    }
}
