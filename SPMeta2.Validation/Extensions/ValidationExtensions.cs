using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Extensions
{
    public static class ValidationExtensions
    {
        public static TSource NotDefaultGuid<TSource, TProperty>(this TSource source,
            Expression<Func<TSource, TProperty>> propertyLambda,
            List<ValidationResult> result)
        {
            return NotEqual(source, propertyLambda, default(Guid), result);
        }

        public static TSource NotEqual<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda,
            object value,
            List<ValidationResult> result)
        {
            var valueResult = source.GetPropertyValue(propertyLambda);

            if (valueResult.Value == value)
            {
                result.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = string.Format("Property [{0}] of type [{1}] must not be equal to [{2}].",
                        valueResult.Name,
                        valueResult.ObjectType.FullName,
                        value)
                });

            }

            return source;
        }

        public static TSource NoMoreThan<TSource, TProperty>(this TSource source,
            Expression<Func<TSource, TProperty>> propertyLambda,
            int lenght,
            List<ValidationResult> result)
        {
            var valueResult = source.GetPropertyValue(propertyLambda);

            CheckIfString<TSource, TProperty>(result, valueResult);
            CheckIfStringIsNotNullOrEmpty<TSource, TProperty>(result, valueResult);
            CheckIfStringIsNoMoreThan<TSource, TProperty>(result, lenght, valueResult);

            return source;
        }

        private static void CheckIfStringIsNoMoreThan<T1, T2>(List<ValidationResult> result, int lenght, PropResult valueResult)
        {
            var value = valueResult.Value as string;

            if (!string.IsNullOrEmpty(value) && value.Length > lenght)
            {
                result.Add(new ValidationResult
               {
                   IsValid = false,
                   Message = string.Format("Property [{0}] of type [{1}] must string with no more than [{2}] chars. Current lenght is: [{3}]",
                       valueResult.Name,
                       valueResult.ObjectType.FullName,
                       lenght,
                       value.Length)
               });
            }
        }

        public static TSource NotEmptyString<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda,
            List<ValidationResult> result)
        {
            var valueResult = source.GetPropertyValue(propertyLambda);

            CheckIfString<TSource, TProperty>(result, valueResult);
            CheckIfStringIsNotNullOrEmpty<TSource, TProperty>(result, valueResult);

            return source;
        }

        public static TSource NotNullString
            <TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda,
            List<ValidationResult> result)
        {
            var valueResult = source.GetPropertyValue(propertyLambda);

            CheckIfString<TSource, TProperty>(result, valueResult);
            CheckIfStringIsNotNull<TSource, TProperty>(result, valueResult);

            return source;
        }

        #region checks

        private static void CheckIfStringIsNotNull<TSource, TProperty>(List<ValidationResult> result, PropResult valueResult)
        {
            if ((valueResult.Value as string) == null)
            {
                result.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = string.Format("Property [{0}] of type [{1}] must not null string.",
                        valueResult.Name,
                        valueResult.ObjectType.FullName)
                });
            }
        }

        private static void CheckIfStringIsNotNullOrEmpty<TSource, TProperty>(List<ValidationResult> result, PropResult valueResult)
        {
            if (string.IsNullOrEmpty(valueResult.Value as string))
            {
                result.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = string.Format("Property [{0}] of type [{1}] must be not null and not empty string.",
                        valueResult.Name,
                        valueResult.ObjectType.FullName)
                });
            }
        }

        private static void CheckIfString<TSource, TProperty>(List<ValidationResult> result, PropResult valueResult)
        {
            if (!(valueResult.Value is string))
            {
                result.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = string.Format("Property [{0}] of type [{1}] must be string type.",
                        valueResult.Name,
                        valueResult.ObjectType.FullName)
                });
            }
        }

        #endregion

        public class PropResult
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public Type ObjectType { get; set; }
        }

        public static PropResult GetPropertyValue<TSource, TProperty>(this TSource source,
            Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            var result = new PropResult();

            result.Name = propInfo.Name;
            result.Value = propInfo.GetValue(source);
            result.ObjectType = source.GetType();

            return result;
        }
    }
}
