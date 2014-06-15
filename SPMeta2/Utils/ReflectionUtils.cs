using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SPMeta2.Utils
{
    public static class ReflectionUtils
    {
        #region methods

        public static IEnumerable<object> GetStaticFieldValues(Type staticClassType)
        {
            return GetStaticFieldValues<object>(staticClassType);
        }

        public static IEnumerable<TValueType> GetStaticFieldValues<TValueType>(Type staticClassType)
        {
            var result = new List<TValueType>();

            foreach (var p in staticClassType.GetFields())
            {
                var value = p.GetValue(null);

                if (value != null && value.GetType() == typeof(TValueType))
                    result.Add((TValueType)value);
            }

            return result;
        }

        public static IEnumerable<Type> GetTypesFromCurrentDomain<TType>()
        {
            return GetTypesFromAssemblies<TType>(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IEnumerable<Type> GetTypesFromAssembly<TType>(Assembly assembly)
        {
            return GetTypesFromAssemblies<TType>(new[] { assembly });
        }

        public static IEnumerable<Type> GetTypesFromAssemblies<TType>(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.GetTypes())
                       .Where(t => typeof(TType).IsAssignableFrom(t) && !t.IsAbstract);
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

        #endregion
    }

    public class PropResult
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Type ObjectType { get; set; }
    }

}
