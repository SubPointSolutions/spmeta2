using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SPMeta2.Models;


namespace SPMeta2.Common
{

    [DataContract]
    [Serializable]
    public class ModelHash
    {
        #region constructors

        public ModelHash()
        {
            ModelNodes = new List<ModelNodeHash>();
        }

        #endregion

        #region properties

        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public List<ModelNodeHash> ModelNodes { get; set; }

        #endregion

        #region override

        public override string ToString()
        {
            return string.Format("Model hash:[{0}] nodes count:[{1}]", Hash, ModelNodes.Count);
        }

        #endregion
    }


    [DataContract]
    [Serializable]
    public class ModelNodeHash
    {
        #region properties

        [DataMember]
        public string DefinitionHash { get; set; }

        [DataMember]
        public string DefinitionFullPathHash { get; set; }

        [DataMember]
        public string DefinitionFullPath { get; set; }

        [DataMember]
        public string DefinitionIdentityKey { get; set; }

        [DataMember]
        public string DefinitionIdentityKeyHash { get; set; }

        #endregion

        #region override

        public override string ToString()
        {
            return string.Format("Definition hash:[{0}] Full path hash:[{1}] Identity key hash:[{2}]",
                        DefinitionHash, DefinitionFullPathHash, DefinitionIdentityKeyHash);
        }

        #endregion
    }

    //[DataContract]
    //[Serializable]
    //public class DefinitionHash
    //{
    //    #region properties

    //    [DataMember]
    //    public string Hash { get; set; }


    //    [DataMember]
    //    public string Path { get; set; }

    //    #endregion
    //}
}
