using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Utils;

namespace SPMeta2.Definitions.Base
{
    [Serializable]
    [DataContract]
    public class NavigationNodeMatch
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Url { get; set; }
    }

    /// <summary>
    /// Base definition for deleting navigation nodes.
    /// </summary>
    /// 
    [Serializable]
    [DataContract]
    public abstract class DeleteNavigationNodesDefinitionBase : DefinitionBase
    {
        #region constructors

        public DeleteNavigationNodesDefinitionBase()
        {
            NavigationNodes = new List<NavigationNodeMatch>();
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        public List<NavigationNodeMatch> NavigationNodes { get; set; }

        #endregion

        #region methods

        // ReSharper disable once RedundantOverridenMember
        public override string ToString()
        {
            return new ToStringResultRaw()
                         .ToString();
        }

        #endregion
    }
}
