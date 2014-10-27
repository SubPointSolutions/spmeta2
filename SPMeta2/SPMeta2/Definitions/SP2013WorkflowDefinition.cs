using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint 2013 workflow.
    /// </summary>
    /// 
    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.WorkflowServices.WorkflowDefinition", "Microsoft.SharePoint.WorkflowServicesBase")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.WorkflowServices.WorkflowDefinition", "Microsoft.SharePoint.Client.WorkflowServices")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(WebDefinition))]

    [Serializable]

    public class SP2013WorkflowDefinition : DefinitionBase
    {
        #region constructors

        public SP2013WorkflowDefinition()
        {
            Override = false;
        }

        #endregion

        #region properties

        /// <summary>
        /// Display name of the target SharePoint 2013 workflow.
        /// </summary>
        /// 
        [ExpectValidation]
        public string DisplayName { get; set; }

        /// <summary>
        /// XAML content of the target SharePoint 2013 workflow.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Xaml { get; set; }

        /// <summary>
        /// Should target workflow be overwritten.
        /// </summary>
        /// 
        [ExpectValidation]
        public bool Override { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SP2013WorkflowDefinition>(this)
                          .AddPropertyValue(p => p.DisplayName)
                          .AddPropertyValue(p => p.Override)

                          .ToString();
        }

        #endregion
    }
}
