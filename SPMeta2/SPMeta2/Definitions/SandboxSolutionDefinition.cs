using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint sandbox solution.
    /// </summary>
    /// 

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPUserSolution", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.File", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectWithExtensionMethod]
    [ExpectArrayExtensionMethod]

    [ParentHostCapability(typeof(SiteDefinition))]

    [ExpectManyInstances]

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
            return new ToStringResultRaw()
                          .AddRawPropertyValue("FileName", FileName)
                          .AddRawPropertyValue("SolutionId", SolutionId)
                          .AddRawPropertyValue("Activate", Activate)
                          .ToString();
        }

        #endregion
    }
}
