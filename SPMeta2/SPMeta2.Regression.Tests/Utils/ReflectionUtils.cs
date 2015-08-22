using System;
using System.Linq;
using System.Reflection;

namespace SPMeta2.Regression.Tests.Utils
{
    internal static class ReflectionUtils
    {
        internal static object CallMethod(this object obj, string methodName)
        {
            return CallMethod(obj, methodName, null);
        }

        internal static object CallMethod(this object obj, string methodName, object[] parameters)
        {
            return CallMethod(obj, parameters, m => m.Name == methodName);
        }

        internal static object CallMethod(this object obj, object[] parameters,
            Func<MethodInfo, bool> filter)
        {
            var type = obj.GetType();
            var method = type.GetMethods().FirstOrDefault(m => filter(m) == true);

            return method.Invoke(obj, parameters);
        }

        internal static object GetPropertyValue(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }
    }
}
