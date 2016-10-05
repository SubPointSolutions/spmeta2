using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Diagnostic
{

    public class ModelStatInfo
    {
        public ModelStatInfo()
        {
            NodeStat = new List<ModelNodeStatInfo>();
        }

        #region properties

        public int TotalModelNodeCount { get; set; }

        public List<ModelNodeStatInfo> NodeStat { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("[{0}] definition(s)", TotalModelNodeCount);
        }

        #endregion
    }

    public class ModelNodeStatInfo
    {
        public ModelNodeStatInfo()
        {
            ModelNodes = new List<ModelNode>();
        }

        #region properties

        public string DefinitionAssemblyQualifiedName { get; set; }
        public string DefinitionName { get; set; }

        public int Count { get; set; }

        public List<ModelNode> ModelNodes { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("[{0}] - [{1}]", DefinitionName, Count);
        }

        #endregion
    }
}
