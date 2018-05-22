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
    /// Allows to define and deploy SharePoint farm solution
    /// 
    /// Supports Retract, Delete, Add, Deploy/Update workflows with the ShouldXXX properties
    /// By default always ShouldAdd always true.
    /// </summary>

    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.Administration.SPSolution", "Microsoft.SharePoint")]

    [DefaultParentHost(typeof(FarmDefinition))]
    [DefaultRootHost(typeof(FarmDefinition))]

    [Serializable]
    [DataContract]
    [ExpectWithExtensionMethod]

    [ParentHostCapability(typeof(FarmDefinition))]
    [ExpectManyInstances]
    public class FarmSolutionDefinition : SolutionDefinitionBase
    {
        #region constructors

        public FarmSolutionDefinition()
        {
            // changed to int?
            // no locale by default
            //LCID = 1033;

            Content = new byte[0];

            DeploymentGlobalInstallWPPackDlls = true;
            DeploymentForce = true;

            ShouldAdd = true;
        }

        #endregion

        #region properties

        /// <summary>
        /// Locale for the current solution.
        /// </summary>
        /// 
        [DataMember]
        [ExpectValidation]
        public int? LCID { get; set; }

        /// <summary>
        /// Indicates if solution has to be retracted
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public bool? ShouldRetract { get; set; }

        /// <summary>
        /// Indicates if solution has to be added
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public bool? ShouldAdd { get; set; }

        /// <summary>
        /// Indicates if solution has to be deleted
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public bool? ShouldDelete { get; set; }

        /// <summary>
        /// Indicates if solution has to be deployed
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public bool? ShouldDeploy { get; set; }

        /// <summary>
        /// Indicates if solution has to be upgraded
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public bool? ShouldUpgrade { get; set; }

        /// <summary>
        /// Deployment date. NULL for 'right now'
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public DateTime? DeploymentDate { get; set; }

        /// <summary>
        /// Upgrade date. NULL for 'right now'
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public DateTime? UpgradeDate { get; set; }

        /// <summary>
        /// Passed to SPSolution.Deploy() method.
        /// TRUE by default
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public bool DeploymentGlobalInstallWPPackDlls { get; set; }

        /// <summary>
        /// Passed to SPSolution.Deploy() method.
        /// TRUE by default
        /// </summary>
        [DataMember]
        [ExpectValidation]
        public bool DeploymentForce { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw()
                          .AddRawPropertyValue("FileName", FileName)
                          .AddRawPropertyValue("SolutionId", SolutionId)
                          .AddRawPropertyValue("LCID", LCID)
                          .ToString();
        }

        #endregion
    }
}
