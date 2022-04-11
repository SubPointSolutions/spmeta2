﻿using System;

namespace SPMeta2.Utils
{
    public static class ConvertUtils
    {
        #region nullable extensions

        public static bool HasGuidValue(this Guid? value)
        {
            return value.HasValue && (value.Value != default(Guid));
        }

        #endregion

        #region methods

        public static int? ToInt(object value)
        {
            if (value == null) return null;
            if (value is int) return (int)value;

            int tmpInt;

            if (int.TryParse(value.ToString(), out tmpInt))
                return tmpInt;

            return null;
        }

        public static string ToString(object value)
        {
            if (value == null) return string.Empty;

            return value.ToString();
        }

        public static string ToStringAndTrim(object value)
        {
            if (value == null) return string.Empty;

            return value.ToString().Trim();
        }

        public static TEnumType ToEnum<TEnumType>(object value)
        {
            if (value == null) return default(TEnumType);

            if (value is TEnumType)
                return (TEnumType)value;

            var stringValue = value.ToString();

            if (!string.IsNullOrEmpty(stringValue))
                return (TEnumType)Enum.Parse(typeof(TEnumType), stringValue, true);

            return default(TEnumType);
        }

        public static Guid? ToGuid(object value)
        {
            if (value == null) return null;
            if (value is Guid) return (Guid)value;

            try
            {
                var result = new Guid(value.ToString());
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool? ToBool(object value)
        {
            if (value == null) return null;
            if (value is bool) return (bool)value;

            var stringValue = value.ToString();

            if (stringValue == "1")
                return true;

            if (stringValue == "0")
                return false;

            bool tmpBool;

            if (bool.TryParse(value.ToString(), out tmpBool))
                return tmpBool;

            return null;
        }

        public static bool ToBoolWithDefault(object value, bool defaultValue)
        {
            var boolResult = ToBool(value);

            if (boolResult.HasValue)
                return boolResult.Value;

            return defaultValue;
        }

        #endregion
    }
}
