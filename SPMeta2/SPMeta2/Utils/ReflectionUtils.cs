using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SPMeta2.Utils
{
    public class PropResult
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Type ObjectType { get; set; }
    }

    /// <summary>
    /// Reflection helpers.
    /// Internal usage only.
    /// </summary>
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

        public static PropResult GetExpressionValue<TSource, TProperty>(this TSource source,
           Expression<Func<TSource, TProperty>> exp)
        {
            Type type = typeof(TSource);

            if (exp.Body is MethodCallExpression)
            {
                var member = exp.Body as MethodCallExpression;
                var methodResult = Expression.Lambda(member, exp.Parameters).Compile().DynamicInvoke(source);

                var result = new PropResult();

                result.Name = member.ToString();
                result.Value = methodResult;
                result.ObjectType = source.GetType();

                return result;
            }

            if (exp.Body is UnaryExpression)
            {
                var member = exp.Body as UnaryExpression;
                var methodResult = Expression.Lambda(member, exp.Parameters).Compile().DynamicInvoke(source);

                var result = new PropResult();

                result.Name = member.ToString();
                result.Value = methodResult;
                result.ObjectType = source.GetType();

                return result;
            }

            if (exp.Body is MemberExpression)
            {
                var member = exp.Body as MemberExpression;
                var propInfo = member.Member as PropertyInfo;

                var result = new PropResult();

                result.Name = propInfo.Name;
                result.Value = propInfo.GetValue(source);
                result.ObjectType = source.GetType();

                return result;
            }

            throw new NotImplementedException("GetExpressionValue");
        }

        #endregion
    }

   
}
