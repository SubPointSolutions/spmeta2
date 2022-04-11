using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SPMeta2.Utils
{
    public class ToStringResult<TType> : ToStringResultRaw
    {
        #region constructors
        public ToStringResult(TType obj)
            : this(obj, string.Empty)
        {

        }

        public ToStringResult(TType obj, string initialString) : base(initialString)
        {
            SrcObject = obj;
        }

        #endregion

        #region static

        public static ToStringResult<TType> New(TType obj)
        {
            return new ToStringResult<TType>(obj);
        }

        #endregion

        #region properties

        public TType SrcObject { get; set; }

        #endregion

        #region methods

        public ToStringResult<TType> AddPropertyValue(Expression<Func<TType, object>> exp)
        {
            var srcProp = SrcObject.GetExpressionValue<TType, object>(exp);

            var key = srcProp.Name;
            var value = srcProp.Value;

            AddRawPropertyValue(key, value);

            return this;
        }

        #endregion
    }
}
