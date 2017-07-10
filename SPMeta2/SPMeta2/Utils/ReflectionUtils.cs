using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SPMeta2.Exceptions;

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

        #region set helpers

        public static void SetNonPublicPropertyValue(object obj, string name, object value)
        {
            var prop = obj.GetType().GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance);

            if (prop != null)
                prop.SetValue(obj, value, null);
        }

        public static void SetNonPublicFieldValue(object obj, string name, object value)
        {
            var prop = obj.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);

            if (prop != null)
                prop.SetValue(obj, value);
        }

        #endregion

        #region get prop methods

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<TType>(object obj)
        {
            return GetPropertiesWithCustomAttribute(obj.GetType(), typeof(TType), false);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<TType>(object obj, bool inherit)
        {
            return GetPropertiesWithCustomAttribute(obj.GetType(), typeof(TType), inherit);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<TType>(Type type)
        {
            return GetPropertiesWithCustomAttribute(type, typeof(TType), false);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute<TType>(Type type, bool inherit)
        {
            return GetPropertiesWithCustomAttribute(type, typeof(TType), inherit);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute(object obj, Type attributeType)
        {
            return GetPropertiesWithCustomAttribute(obj, attributeType, false);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute(object obj, Type attributeType, bool inherit)
        {
            return GetPropertiesWithCustomAttribute(obj.GetType(), attributeType, inherit);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute(Type type, Type attributeType)
        {
            return GetPropertiesWithCustomAttribute(type, attributeType, false);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithCustomAttribute(Type type, Type attributeType, bool inherit)
        {
            return type
                           .GetProperties()
                           .Where(p => p.GetCustomAttributes(attributeType, inherit).Any());
        }
        #endregion

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

        private static Dictionary<object, Delegate> _cache4GetExpressionValueAsMethodCallExpression = new Dictionary<object, Delegate>();
        private static Dictionary<object, Delegate> _cache4GetExpressionValueAsUnaryExpression = new Dictionary<object, Delegate>();
        private static Dictionary<object, Delegate> _cache4GetExpressionValueAsMemberExpression = new Dictionary<object, Delegate>();

        private static Dictionary<Type, Dictionary<PropertyInfo, Delegate>> _cache4FastGetters = new Dictionary<Type, Dictionary<PropertyInfo, Delegate>>();



        public static PropResult GetExpressionValue<TSource, TProperty>(this TSource source,
           Expression<Func<TSource, TProperty>> exp)
        {
            Type type = typeof(TSource);

            if (exp.Body is MethodCallExpression)
            {
                var member = exp.Body as MethodCallExpression;

                if (!_cache4GetExpressionValueAsMethodCallExpression.ContainsKey(exp))
                {
                    var compiledLambda = Expression.Lambda(member, exp.Parameters.ToArray()).Compile();
                    _cache4GetExpressionValueAsMethodCallExpression.Add(exp, compiledLambda);
                }

                //var methodResult = compiledLambda.DynamicInvoke(source);
                var methodResult = _cache4GetExpressionValueAsMethodCallExpression[exp].DynamicInvoke(source);

                var result = new PropResult();

                result.Name = member.ToString();
                result.Value = methodResult;
                result.ObjectType = source.GetType();

                return result;
            }

            if (exp.Body is UnaryExpression)
            {
                var member = exp.Body as UnaryExpression;

                if (!_cache4GetExpressionValueAsUnaryExpression.ContainsKey(exp))
                {
                    var compiledLambda = Expression.Lambda(member, exp.Parameters.ToArray()).Compile();
                    _cache4GetExpressionValueAsUnaryExpression.Add(exp, compiledLambda);
                }

                //var methodResult = compiledLambda.DynamicInvoke(source);
                var methodResult = _cache4GetExpressionValueAsUnaryExpression[exp].DynamicInvoke(source);

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

                if (!_cache4FastGetters.ContainsKey(type))
                    _cache4FastGetters.Add(type, new Dictionary<PropertyInfo, Delegate>());

                var _cache4FastGettersFunctions = _cache4FastGetters[type];

                if (!_cache4FastGettersFunctions.ContainsKey(propInfo))
                {
                    var fastGetter = BuildUntypedGetter<TSource>(propInfo);
                    _cache4FastGettersFunctions.Add(propInfo, fastGetter);
                }
                else
                {

                }

                var value = _cache4FastGettersFunctions[propInfo].DynamicInvoke(source);
                //result.Value = propInfo.GetValue(source, null);
                result.Value = value;

                result.ObjectType = source.GetType();

                return result;
            }

            throw new NotImplementedException("GetExpressionValue");
        }

        public static Func<T, object> BuildUntypedGetter<T>(PropertyInfo propertyInfo)
        {
            var targetType = propertyInfo.DeclaringType;
            var methodInfo = propertyInfo.GetGetMethod();
            var returnType = methodInfo.ReturnType;

            var exTarget = Expression.Parameter(targetType, "t");
            var exBody = Expression.Call(exTarget, methodInfo);
            var exBody2 = Expression.Convert(exBody, typeof(object));

            var lambda = Expression.Lambda<Func<T, object>>(exBody2, exTarget);

            var action = lambda.Compile();
            return action;
        }
        public static object GetPropertyValue(object obj, string propName)
        {
            var prop = obj.GetType().GetProperties(BindingFlags.Instance |
                                                   BindingFlags.NonPublic |
                                                   BindingFlags.Public)
                                    .FirstOrDefault(p => p.Name.ToUpper() == propName.ToUpper());

            if (prop == null)
                throw new SPMeta2Exception(string.Format("Can't find prop: [{0}] in obj:[{1}]", propName, obj));

            return prop.GetValue(obj, null);
        }

        // AddSupportedUILanguage

        public static MethodInfo GetMethod(object obj, string methodName)
        {
            var methods = obj.GetType().GetMethods(BindingFlags.Instance |
                                                      BindingFlags.NonPublic |
                                                      BindingFlags.Public);

            return methods.FirstOrDefault(p => p.Name.ToUpper() == methodName.ToUpper());
        }

        public static bool HasMethod(object obj, string methodName)
        {
            return HasMethods(obj, new[] { methodName });
        }

        public static bool HasMethods(object obj, IEnumerable<string> methodNames)
        {
            var methods = obj.GetType().GetMethods(BindingFlags.Instance |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Public);

            foreach (var methodName in methodNames)
            {
                var method = methods.FirstOrDefault(p => p.Name.ToUpper() == methodName.ToUpper());

                if (method == null)
                    return false;
            }

            return true;
        }

        public static bool HasProperty(object obj, string propName)
        {
            return HasProperties(obj, new[] { propName });
        }

        public static bool HasProperties(object obj, IEnumerable<string> propNames)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Instance |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Public);

            foreach (var propName in propNames)
            {
                var prop = props.FirstOrDefault(p => p.Name.ToUpper() == propName.ToUpper());

                if (prop == null)
                    return false;
            }

            return true;
        }

        public static bool HasPropertyPublicSetter(object obj, string propName)
        {
            var prop = obj.GetType().GetProperty(propName);
            if (prop != null)
            {
                return prop.GetSetMethod(false) != null;
            }

            return false;
        }

        public static int? GetHResultValue(Exception exception)
        {
            // .net 4 hack to get HResult
            var hResultProp = exception.GetType()
                                .GetProperty("HResult",
                                            BindingFlags.NonPublic
                                            | BindingFlags.Public
                                            | BindingFlags.Instance
                                            | BindingFlags.Static);


            if (hResultProp != null)
            {
                var hResultValue = hResultProp.GetValue(exception, null);
                if (hResultValue is int)
                {
                    return (int)hResultValue;
                }
            }

            return null;
        }

        #endregion
    }
}
