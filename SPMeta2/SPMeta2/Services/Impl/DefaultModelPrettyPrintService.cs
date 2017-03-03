using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Models;

namespace SPMeta2.Services.Impl
{
    public class DefaultModelPrettyPrintService : ModelPrettyPrintServiceBase
    {
        #region constructora
        public DefaultModelPrettyPrintService()
        {
            IndentString = "    ";
            NewLineString = Environment.NewLine;
        }

        #endregion

        #region properties

        public string IndentString { get; set; }
        public string NewLineString { get; set; }

        #endregion

        #region methods
        public override string PrintModel(ModelNode modelNode)
        {
            var result = new StringBuilder();

            result.AppendFormat("{0}{1}{2}", GetCurrentIntent(modelNode, string.Empty), GetDefinitionValue(modelNode), NewLineString);

            WalkModelNodes(modelNode, result, IndentString);

            return result.ToString();
        }

        protected virtual void WalkModelNodes(ModelNode model, StringBuilder result, string indent)
        {
            foreach (var modelNodeGroup in model.ChildModels
                                                .GroupBy(n => n.GetType())
                                                .OrderBy(g => g.Key.Name))
            {
                foreach (var modelNode in modelNodeGroup)
                {
                    result.AppendFormat("{0}{1}{2}", GetCurrentIntent(modelNode, indent), GetDefinitionValue(modelNode), NewLineString);
                    WalkModelNodes(modelNode, result, indent + IndentString);
                }
            }
        }

        protected virtual string GetCurrentIntent(ModelNode modelNode, string currentIndent)
        {
            return currentIndent;
        }

        protected virtual string GetDefinitionValue(ModelNode modelNode)
        {
            return modelNode.Value != null
                        ? string.Format("[{0}] - {1}", modelNode.Value.GetType().Name, modelNode.Value)
                        : base.ToString();
        }

        #endregion
    }
}
