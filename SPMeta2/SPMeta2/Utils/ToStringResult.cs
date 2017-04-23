using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SPMeta2.Utils
{
    public class ToStringResult<TType>
    {
        #region constructors
        public ToStringResult(TType obj)
            : this(obj, string.Empty)
        {

        }

        public ToStringResult(TType obj, string initialString)
        {
            SrcObject = obj;
            Values = new Dictionary<string, string>();

            InitialString = initialString;
        }

        #endregion

        #region static

        public static ToStringResult<TType> New(TType obj)
        {
            return new ToStringResult<TType>(obj);
        }

        #endregion

        #region properties

        public string InitialString { get; set; }
        public TType SrcObject { get; set; }
        public Dictionary<string, string> Values { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return ToString(" ");
        }

        public string ToString(string joinSeparator)
        {
            var result = new List<string>();

            foreach (var key in Values.Keys)
                result.Add(string.Format("{0}:[{1}]", key, Values[key]));

            if (!string.IsNullOrEmpty(InitialString))
                return InitialString + joinSeparator + string.Join(joinSeparator, result.ToArray());

            return string.Join(joinSeparator, result.ToArray());
        }

        public ToStringResult<TType> AddPropertyValue(Expression<Func<TType, object>> exp)
        {
            var srcProp = SrcObject.GetExpressionValue<TType, object>(exp);

            var key = srcProp.Name;
            var value = srcProp.Value;

            var valueString = string.Empty;

            if (value is IEnumerable && !(value is string))
            {
                var enumerableValues = new List<string>();

                var enumerator = (value as IEnumerable).GetEnumerator();

                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != null)
                        enumerableValues.Add(enumerator.Current.ToString());
                    else
                        enumerableValues.Add(string.Empty);
                }

                valueString = string.Join("|", enumerableValues.ToArray());
            }
            else
            {
                valueString = value == null ? string.Empty : value.ToString();
            }

            if (!Values.ContainsKey(key))
                Values.Add(key, valueString);

            return this;
        }

        #endregion
    }
}
