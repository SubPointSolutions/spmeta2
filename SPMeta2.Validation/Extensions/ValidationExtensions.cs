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
        public static TSource NotEmptyString
            <TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda,
            List<ValidationResult> result)
        {
            var valueResult = source.GetPropertyValue(propertyLambda);

            CheckIfString<TSource, TProperty>(result, valueResult);

            CheckIfStringIsNotNullOrEmpty<TSource, TProperty>(result, valueResult);

            return source;
        }

        private static void CheckIfStringIsNotNullOrEmpty<TSource, TProperty>(List<ValidationResult> result, PropResult valueResult)
        {
            if (string.IsNullOrEmpty(valueResult.Value as string))
            {
                result.Add(new ValidationResult
                {
                    IsValid = false,
                    Message = string.Format("Property [{0}] of type [{1}] must be non-empty string.",
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

        class PropResult
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public Type ObjectType { get; set; }
        }

        private static PropResult GetPropertyValue<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
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
