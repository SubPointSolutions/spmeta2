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
