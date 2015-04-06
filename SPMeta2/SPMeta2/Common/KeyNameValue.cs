using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace SPMeta2.Common
{
    [DataContract]
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
