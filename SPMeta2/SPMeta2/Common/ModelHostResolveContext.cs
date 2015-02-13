using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;
using SPMeta2.Models;

namespace SPMeta2.Common
{
    /// <summary>
    /// Content info for model host resolution. 
    /// </summary>
    /// 
    public class ModelHostResolveContext
    {
        #region properties

        /// <summary>
        /// Current model host.
        /// </summary>
        public object ModelHost { get; set; }


        /// <summary>
        /// Current model instance
        /// </summary>
        public DefinitionBase Model { get; set; }

        /// <summary>
        /// Current model node instance.
        /// </summary>
        public ModelNode ModelNode { get; set; }

        /// <summary>
        /// Type of the child models to be processed.
        /// </summary>
        public Type ChildModelType { get; set; }

        /// <summary>
        /// Action to be performed with passing new model host.
        /// </summary>
        public Action<object> Action { get; set; }

        #endregion
    }

}
