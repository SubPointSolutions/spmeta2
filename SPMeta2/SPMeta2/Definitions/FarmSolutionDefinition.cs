﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions.Base;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Utils;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy SharePoint farm solution.
    /// </summary>
    /// 

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
            LCID = 1033;
            Content = new byte[0];
        }

        #endregion

        #region properties

        /// <summary>
        /// Locale for the current solution.
        /// </summary>
        /// 
        [DataMember]
        public int LCID { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<FarmSolutionDefinition>(this)
                          .AddPropertyValue(p => p.FileName)
                          .AddPropertyValue(p => p.SolutionId)
                          .AddPropertyValue(p => p.LCID)
                          .ToString();
        }

        #endregion
    }
}
