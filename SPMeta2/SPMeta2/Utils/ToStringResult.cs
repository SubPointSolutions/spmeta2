using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Utils
{
    public class ToStringResult<TType>
    {
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

        public static ToStringResult<TType> New(TType obj)
        {
            return new ToStringResult<TType>(obj);
        }

        public override string ToString()
        {
            var result = new List<string>();

            foreach (var key in Values.Keys)
                result.Add(string.Format("{0}:[{1}]", key, Values[key]));

            if (!string.IsNullOrEmpty(InitialString))
                return InitialString + " " + string.Join(" ", result);

            return string.Join(" ", result);
        }

        public string InitialString { get; set; }
        public TType SrcObject { get; set; }
        public Dictionary<string, string> Values { get; set; }

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

                valueString = string.Join("|", enumerableValues);
            }
            else
            {
                valueString = value == null ? string.Empty : value.ToString();
            }

            if (!Values.ContainsKey(key))
                Values.Add(key, valueString);

            return this;
        }
    }
}
