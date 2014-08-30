using System;

namespace SPMeta2.Utils
{
    /// <summary>
    /// Validation helper.
    /// Internal usage only.
    /// </summary>
    public static class ValidationUtils
    {
        #region static

        public static TResult WithAssertAndCast<TResult>(this object value, string valueName, Action<ValidationValueContext> validation)
        {
            var resultValue = WithAssert(value, valueName, validation);

            // double validation to make sure cast will be ok
            var valueContext = new ValidationValueContext
            {
                Value = resultValue,
                ValueName = valueName
            };

            return (TResult)(RequireType<TResult>(valueContext).Value);
        }

        public static object WithAssert(this object value, string valueName, Action<ValidationValueContext> validation)
        {
            if (string.IsNullOrEmpty(valueName))
                throw new ArgumentException("valueName");

            if (validation == null)
                throw new ArgumentException("validation");

            var valueContext = new ValidationValueContext
            {
                Value = value,
                ValueName = valueName
            };

            validation(valueContext);

            return value;
        }

        public static ValidationValueContext RequireStringNotOrEmpty(this ValidationValueContext valueContext)
        {
            if (!(valueContext.Value is string))
                throw new ArgumentNullException(string.Format("[{0}] has to be string.", valueContext.ValueName));

            if (valueContext.Value == null || string.IsNullOrEmpty(valueContext.Value.ToString()))
                throw new ArgumentNullException(string.Format("[{0}] has to be not NULL or Empty.", valueContext.ValueName));

            return valueContext;
        }

        public static ValidationValueContext RequireNotNull(this ValidationValueContext valueContext)
        {
            if (valueContext.Value == null)
                throw new ArgumentNullException(string.Format("[{0}] has to be not NULL.", valueContext.ValueName));

            return valueContext;
        }

        public static ValidationValueContext RequireType<TValueType>(this ValidationValueContext valueContext)
        {
            if (!(valueContext.Value is TValueType))
                throw new InvalidCastException(string.Format("[{0}] has to be instance of type [{1}]. Current type is: [{2}]",
                        valueContext.ValueName,
                        typeof(TValueType),
                        valueContext.Value == null ? null : valueContext.Value.GetType()));

            return valueContext;
        }

        #endregion
    }
}
