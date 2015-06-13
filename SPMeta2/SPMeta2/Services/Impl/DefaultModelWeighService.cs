using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Common;
using SPMeta2.Models;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Services.Impl
{
    public class DefaultModelWeighService : ModelWeighServiceBase
    {
        #region consts

        private const string ContentTypeGuidFormatString = "N";

        #endregion

        #region methods

        public override IEnumerable<Common.ModelWeigh> GetModelWeighs()
        {
            return DefaultModelWeigh.Weighs;
        }

        public override void SortChildModelNodes(ModelNode modelNode, List<ModelNode> childNodes)
        {
            base.SortChildModelNodes(modelNode, childNodes);

            HandleContentTypes(modelNode, childNodes);
        }

        private void HandleContentTypes(ModelNode modelNode, List<ModelNode> childNodes)
        {
            // https://github.com/SubPointSolutions/spmeta2/issues/385

            if (modelNode.Value is SiteDefinition
                || modelNode.Value is WebDefinition)
            {
                var isContentTypeNodes = childNodes.All(n => n.Value is ContentTypeDefinition);

                if (isContentTypeNodes)
                {
                    childNodes.Sort(delegate(ModelNode n1, ModelNode n2)
                    {
                        var firstCtId = GetContentTypeId(n1.Value as ContentTypeDefinition);
                        var secondCtId = GetContentTypeId(n2.Value as ContentTypeDefinition);

                        return firstCtId.CompareTo(secondCtId);
                    });
                }
            }
        }

        #endregion

        #region utils

        private static bool IsChildOf(string childId, string parentId)
        {
            if (parentId.Length < childId.Length)
                return false;

            for (int i = 0; i < childId.Length; i++)
                if (childId[i] != parentId[i])
                    return false;

            return true;
        }

        private static string GetContentTypeId(ContentTypeDefinition contentType)
        {
            if (contentType.Id != default(Guid))
            {
                // for content types like
                // 0x010100339210063E00144CBB5EFF79F55FE57400D05BFCF30398485FBBD6D5BB034AFF74
                // 0x010100339210063E00144CBB5EFF79F55FE57400D05BFCF30398485FBBD6D5BB034AFF74008FA22F0260524AF78AF78C349553F22E

                return contentType.ParentContentTypeId + "00" + contentType.Id.ToString(ContentTypeGuidFormatString).ToUpper();
            }

            if (!string.IsNullOrEmpty(contentType.IdNumberValue))
            {
                // for content types like
                // 0x010100339210063E00144CBB5EFF79F55FE574 
                // 0x010100339210063E00144CBB5EFF79F55FE57401 
                // 0x010100339210063E00144CBB5EFF79F55FE5740101

                return contentType.ParentContentTypeId + contentType.IdNumberValue;
            }

            // TODO
            // however, validation system for model definition are going to be implemented
            throw new ArgumentException("Either Id or IdNumberValue have to be specified for content type model");
        }

        #endregion
    }
}
