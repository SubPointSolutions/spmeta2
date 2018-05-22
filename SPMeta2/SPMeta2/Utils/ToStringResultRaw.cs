using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Utils
{
    public class ToStringResultRaw
    {
        #region constructors

        public ToStringResultRaw() : this(String.Empty)
        {

        }

        public ToStringResultRaw(string initialString)
        {
            Values = new Dictionary<string, string>();

            InitialString = initialString;
        }

        #endregion

        #region properties

        public string InitialString { get; set; }
        public Dictionary<string, string> Values { get; set; }

        #endregion

        #region methods

        public ToStringResultRaw AddRawPropertyValue(string key, object value)
        {
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

        #endregion
    }
}
