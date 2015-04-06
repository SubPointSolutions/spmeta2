using System.Net.Mail;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions.Base;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint sandbox solution.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPUserSolution", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultParentHostAttribute(typeof(SiteDefinition))]
    [DefaultRootHostAttribute(typeof(SiteDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    public class SandboxSolutionDefinition : SolutionDefinitionBase
    {
        #region constructors

        public SandboxSolutionDefinition()
        {
            Content = new byte[0];
        }

        #endregion

        #region properties

        /// <summary>
        /// Should the solution be activated.
        /// </summary>
        [ExpectValidation]
        [DataMember]
        public bool Activate { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<SandboxSolutionDefinition>(this)
                          .AddPropertyValue(p => p.FileName)
                          .AddPropertyValue(p => p.SolutionId)
                          .AddPropertyValue(p => p.Activate)
                          .ToString();
        }

        #endregion
    }
}
