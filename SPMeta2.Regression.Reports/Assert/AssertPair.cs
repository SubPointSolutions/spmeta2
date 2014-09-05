using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Reports.Assert
{
    public class AssertPairBase
    {

    }

    public class OnValidatePropertyEventArgs<TSrc, TDst> : EventArgs
    {
        public AssertPair<TSrc, TDst> Assert { get; set; }

        public string SrcPropertyName { get; set; }
        public object SrcPropertyValue { get; set; }
        public string SrcPropertyType { get; set; }

        public string DstPropertyName { get; set; }
        public object DstPropertyValue { get; set; }
        public string DstPropertyType { get; set; }
    }

    public class AssertPair<TSrc, TDst> : AssertPairBase
    {

        public EventHandler<OnValidatePropertyEventArgs<TSrc, TDst>> OnValidateProperty;


        public void InvokeOnValidateProperty(OnValidatePropertyEventArgs<TSrc, TDst> args)
        {
            if (OnValidateProperty != null)
                OnValidateProperty(this, args);
        }

        public AssertPair(TSrc model, TDst obj)
        {
            Src = model;
            Dst = obj;
        }

        public TSrc Src { get; set; }
        public TDst Dst { get; set; }

        internal void AreEqual(object p1, object p2)
        {
            if (!object.Equals(p1, p2))
                throw new ArgumentException(string.Format("object aren't equals."));
        }
    }
}
