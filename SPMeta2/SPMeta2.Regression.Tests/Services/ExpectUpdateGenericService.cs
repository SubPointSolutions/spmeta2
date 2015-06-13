using System;
using SPMeta2.Attributes.Regression;

namespace SPMeta2.Regression.Tests.Services
{
    public abstract class ExpectUpdateGenericService<TTargetType> : ExpectUpdateValueServiceBase
        where TTargetType : ExpectUpdate
    {
        public override Type TargetType
        {
            get { return typeof(TTargetType); }
            set { }
        }
    }
}
