using System;
using System.Collections.Generic;
using System.Linq;
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

        #endregion
    }
}
