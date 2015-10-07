using System;
using System.Runtime.Serialization;

namespace SPMeta2.Common
{
    [DataContract]
    [Serializable]
    public class KeyNameValue
    {
        #region properties

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }

        #endregion
    }
}
