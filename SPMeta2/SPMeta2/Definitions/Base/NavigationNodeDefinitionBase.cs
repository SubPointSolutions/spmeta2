using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Definitions.Base
{
    /// <summary>
    /// Base definition for SharePoint navigation nodes.
    /// </summary>
    /// 
     [Serializable]
    public abstract class NavigationNodeDefinitionBase : DefinitionBase
    {
        #region constructors

        public NavigationNodeDefinitionBase()
        {
            IsVisible = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Title of the target navigation node.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL of the target navigation node.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// IsExternal flag of the target navigation node.
        /// </summary>
        public bool IsExternal { get; set; }

        /// <summary>
        /// IsVisible flag of the target navigation node.
        /// </summary>
        public bool IsVisible { get; set; }

        #endregion
    }
}
