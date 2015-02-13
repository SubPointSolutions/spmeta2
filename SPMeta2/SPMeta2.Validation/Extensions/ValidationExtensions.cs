using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using SPMeta2.Utils;
using SPMeta2.Validation.Common;

namespace SPMeta2.Validation.Extensions
{
    public static class ValidationExtensions
    {
        public static TSource NotDefaultGuid<TSource, TProperty>(this TSource source,
            Expression<Func<TSource, TProperty>> propertyLambda,
            List<ValidationResult> result)
        {
            return NotEqual(source, propertyLambda, default(Guid), result, ValidationResultType.NotDefaultGuid);
        }

        public static TSource NotEqual<TSource, TProperty>(this TSource source,
            Expression<Func<TSource, TProperty>> propertyLambda,
            object value,
            List<ValidationResult> result)
        {
            return NotEqual(source, propertyLambda, value, result, ValidationResultType.NotEqual);
        }

        public static TSource NotEqual<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda,
            object value,
            List<ValidationResult> result,
            ValidationResultType resultType)
        {
            var valueResult = source.GetExpressionValue(propertyLambda);

            if (object.Equals(valueResult.Value, value))
            // AH!
            //(value is Guid && ((Guid)value == (Guid)valueResult.Value)))
            {
                result.Add(new ValidationResult
                {
                    PropertyName = valueResult.Name,
                    ResultType = resultType,
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
            var valueResult = source.GetExpressionValue(propertyLambda);

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
                    PropertyName = valueResult.Name,
                    ResultType = ValidationResultType.NoMoreThan,
                    IsValid = false,
                    Message = string.Format("Property [{0}] of type [{1}] must string with no more than [{2}] chars. Current lenght is: [{3}]",
                        valueResult.Name,
                        valueResult.ObjectType.FullName,
                        lenght,
                        value.Length)
                });
            }
        }

        public static TSource NoSlashesBefore<TSource, TProperty>(this TSource source,
                                                                        Expression<Func<TSource, TProperty>>
                                                                            propertyLambda,
                                                                        List<ValidationResult> result)
        {
            var valueResult = source.GetExpressionValue(propertyLambda);

            CheckIfString<TSource, TProperty>(result, valueResult);
            CheckIfStringHasStringsBefore<TSource, TProperty>(result, valueResult,
                new string[] { "/", "\\" });

            return source;
        }

        private static void CheckIfStringHasStringsBefore<T1, T2>(List<ValidationResult> result, PropResult valueResult,
            IEnumerable<string> symbols)
        {
            var stringValue = valueResult.Value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                foreach (var ch in symbols)
                {
                    if (stringValue.StartsWith(ch))
                    {
                        result.Add(new ValidationResult
                        {
                            IsValid = false,
                            Message = string.Format("Property [{0}] of type [{1}] must not start with char:[{2}].",
                                valueResult.Name,
                                valueResult.ObjectType.FullName,
                                ch)
                        });
                    }
                }
            }
        }

        public static TSource NoSpacesBeforeOrAfter<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda,
          List<ValidationResult> result)
        {
            var valueResult = source.GetExpressionValue(propertyLambda);

            CheckIfString<TSource, TProperty>(result, valueResult);
            CheckIfStringHasSpacesBeforeOrAfter<TSource, TProperty>(result, valueResult);

            return source;
        }

        private static void CheckIfStringHasSpacesBeforeOrAfter<T1, T2>(List<ValidationResult> result, PropResult valueResult)
        {
            var stringValue = valueResult.Value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                if (stringValue.StartsWith(" "))
                {
                    result.Add(new ValidationResult
                    {
                        PropertyName = valueResult.Name,
                        ResultType = ValidationResultType.NoSpacesBeforeOrAfter,
                        IsValid = false,
                        Message = string.Format("Property [{0}] of type [{1}] must not start with space.",
                            valueResult.Name,
                            valueResult.ObjectType.FullName)
                    });
                }

                if (stringValue.EndsWith(" "))
                {
                    result.Add(new ValidationResult
                    {
                        PropertyName = valueResult.Name,
                        ResultType = ValidationResultType.NoSpacesBeforeOrAfter,
                        IsValid = false,
                        Message = string.Format("Property [{0}] of type [{1}] must not end with space.",
                            valueResult.Name,
                            valueResult.ObjectType.FullName)
                    });
                }


            }
        }

        public static TSource NotEmptyString<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda,
            List<ValidationResult> result)
        {
            var valueResult = source.GetExpressionValue(propertyLambda);

            CheckIfString<TSource, TProperty>(result, valueResult);
            CheckIfStringIsNotNullOrEmpty<TSource, TProperty>(result, valueResult);

            return source;
        }

        public static TSource NotNullString
            <TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda,
            List<ValidationResult> result)
        {
            var valueResult = source.GetExpressionValue(propertyLambda);

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
                    PropertyName = valueResult.Name,
                    ResultType = ValidationResultType.NotNullString,
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
                    PropertyName = valueResult.Name,
                    ResultType = ValidationResultType.NotEmptyString,
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
                    PropertyName = valueResult.Name,
                    ResultType = ValidationResultType.MustBeString,
                    IsValid = false,
                    Message = string.Format("Property [{0}] of type [{1}] must be string type.",
                        valueResult.Name,
                        valueResult.ObjectType.FullName)
                });
            }
        }

        #endregion

    }
}
