using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Utils;

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
        /// 
        [ExpectValidation]
        public string Title { get; set; }

        /// <summary>
        /// URL of the target navigation node.
        /// </summary>
        [ExpectValidation]
        public string Url { get; set; }

        /// <summary>
        /// IsExternal flag of the target navigation node.
        /// </summary>
        /// 
        [ExpectValidation]
        public bool IsExternal { get; set; }

        /// <summary>
        /// IsVisible flag of the target navigation node.
        /// </summary>
        /// 
        [ExpectValidation]
        public bool IsVisible { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<NavigationNodeDefinitionBase>(this)
                          .AddPropertyValue(p => p.Title)
                          .AddPropertyValue(p => p.Url)
                          .AddPropertyValue(p => p.IsExternal)
                          .AddPropertyValue(p => p.IsVisible)
                          .ToString();
        }

        #endregion
    }
}
