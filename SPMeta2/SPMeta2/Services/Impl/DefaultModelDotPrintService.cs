using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;

namespace SPMeta2.Services.Impl
{
    public class DefaultModelDotGraphPrintService : ModelDotGraphPrintServiceBase
    {
        #region constructora
        public DefaultModelDotGraphPrintService()
        {
            IndentString = "    ";
            NewLineString = Environment.NewLine;

            ModelNodeRelatonshipTemplate = "\"[root] {0}\" -> \"[root] {1}\"";
            ModelNodeTemplate = "\"[root] {0}\" [label = \"{1}\", shape = \"box\"]";
        }

        #endregion

        #region properties

        public string NewLineString { get; set; }
        public string IndentString { get; set; }

        public string ModelNodeRelatonshipTemplate { get; set; }
        public string ModelNodeTemplate { get; set; }

        private HashCodeServiceBase _hashCodeService;

        protected HashCodeServiceBase HashCodeService
        {
            get
            {
                if (_hashCodeService == null)
                    _hashCodeService = ServiceContainer.Instance.GetService<HashCodeServiceBase>();

                return _hashCodeService;
            }
            set
            {
                _hashCodeService = value;
            }
        }

        #endregion

        #region methods

        public override string PrintModel(ModelNode modelNode)
        {
            var result = new StringBuilder();

            BuildGraphStart(result, modelNode);

            BuildShapes(result, modelNode);
            BuildRelationships(result, modelNode);

            BuildGraphEnd(result, modelNode);

            return result.ToString();
        }


        private void BuildGraphStart(StringBuilder result, ModelNode modelNode)
        {
            result.AppendFormat("digraph {{{0}", NewLineString);

            result.AppendFormat("{0}compound = \"true\"{1}", IndentString, NewLineString);
            result.AppendFormat("{0}newrank = \"true\"{1}", IndentString, NewLineString);
            result.AppendFormat("{0}rankdir = \"BT\"{1}", IndentString, NewLineString);
            result.AppendFormat("{0}subgraph \"root\" {{{1}", IndentString, NewLineString);
        }

        private void BuildGraphEnd(StringBuilder result, ModelNode modelNode)
        {
            result.AppendFormat("{0}}}{1}", IndentString, NewLineString);
            result.AppendFormat("}}{0}", NewLineString);
        }

        protected virtual void BuildRelationships(StringBuilder result, ModelNode modelNode)
        {
            WalkModelNodes(modelNode, result, false);
        }

        protected virtual void BuildShapes(StringBuilder result, ModelNode modelNode)
        {
            result.AppendFormat("{0}{0}{1}{2}", IndentString, GetDefinitionShape(modelNode), NewLineString);
            WalkModelNodes(modelNode, result, true);
        }

        protected virtual void WalkModelNodes(ModelNode model, StringBuilder result, bool isShape)
        {
            foreach (var modelNodeGroup in model.ChildModels
                                                .GroupBy(n => n.GetType())
                                                .OrderBy(g => g.Key.Name))
            {
                foreach (var modelNode in modelNodeGroup)
                {
                    if (isShape)
                    {
                        result.AppendFormat("{0}{0}{1}{2}", IndentString, GetDefinitionShape(modelNode), NewLineString);
                    }
                    else
                    {
                        result.AppendFormat("{0}{0}{1}{2}", IndentString, GetDefinitionRelationShip(model, modelNode), NewLineString);
                    }

                    WalkModelNodes(modelNode, result, isShape);
                }
            }
        }

        private string GetDefinitionRelationShip(ModelNode parentNode, ModelNode childNode)
        {
            var result = string.Format(ModelNodeRelatonshipTemplate,
                HashCodeService.GetHashCode(childNode.Value).Replace("=", string.Empty),
                HashCodeService.GetHashCode(parentNode.Value).Replace("=", string.Empty));

            return result;
        }

        protected virtual string GetDefinitionShape(ModelNode modelNode)
        {
            var result = modelNode.Value != null
                        ? string.Format(ModelNodeTemplate,
                                        HashCodeService.GetHashCode(modelNode.Value).Replace("=", string.Empty),
                                        string.Format("[{0}] - {1}", modelNode.Value.GetType().Name, modelNode.Value))
                       : string.Empty;

            return result;
        }

        #endregion
    }
}
