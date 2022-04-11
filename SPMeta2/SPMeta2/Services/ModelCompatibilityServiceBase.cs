using SPMeta2.Definitions;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SPMeta2.Attributes;
using SPMeta2.Extensions;

namespace SPMeta2.Services
{
    public abstract class ModelCompatibilityServiceBase
    {
        #region methods

        public abstract ModelProvisionCompatibilityResult CheckProvisionCompatibility(ModelNode model);
        public abstract ModelProvisionCompatibilityResult CheckProvisionCompatibility(DefinitionBase definition);


        #endregion
    }

    public class ModelProvisionCompatibilityResult
    {
        public ModelProvisionCompatibilityResult()
        {
            Result = new List<ModelProvisionCompatibilityResultValue>();
        }

        public ModelNode Model { get; set; }
        public List<ModelProvisionCompatibilityResultValue> Result { get; set; }
    }

    public class ModelProvisionCompatibilityResultValue
    {
        #region properties

        public DefinitionBase Definition { get; set; }

        public ModelNode ModelNode { get; set; }

        /// <summary>
        /// Indicates if definition can be deployed with CSOM.
        /// If null, then M2 can't understand if definition can be deployed with CSOM/SSOM, please use SPObjectModelType attribute to flag provision support.
        /// </summary>
        public bool? IsCSOMCompatible { get; set; }

        /// <summary>
        /// Indicates if definition can be deployed with SSOM.
        /// If null, then M2 can't understand if definition can be deployed with CSOM/SSOM, please use SPObjectModelType attribute to flag provision support.
        /// </summary>
        public bool? IsSSOMCompatible { get; set; }

        #endregion
    }
}
