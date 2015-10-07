using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Utils;

namespace SPMeta2.Services.ServiceModelHandlers
{
    public class DefaultNotAbsoluteUrlPropertiesModelHandler : ServiceModelHandlerBase
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var props = ReflectionUtils.GetPropertiesWithCustomAttribute<NotAbsoluteUrlCapabilityAttribute>(model, true);

            ValidateProps(model, props.Where(p => p.GetCustomAttributes(typeof(NotAbsoluteUrlCapabilityAttribute), true).FirstOrDefault() is NotAbsoluteUrlCapabilityAttribute));
        }

        protected virtual string ExtractStringValue(DefinitionBase obj, PropertyInfo prop)
        {
            var value = prop.GetValue(obj, null);

            if (value is string && !string.IsNullOrEmpty(ConvertUtils.ToString(value)))
                return value.ToString();

            return string.Empty;
        }

        protected virtual void ValidateProps(DefinitionBase obj, IEnumerable<PropertyInfo> props)
        {
            ValidatePropertyInternal(obj, props);
        }


        private void ValidatePropertyInternal(DefinitionBase obj, IEnumerable<PropertyInfo> props)
        {
            foreach (var prop in props)
            {
                var propName = prop.Name;
                var propValue = ExtractStringValue(obj, prop);

                if (!string.IsNullOrEmpty(propValue))
                    ValidateUrlString(obj, propValue, propName);
            }
        }

        private static void ValidateUrlString(DefinitionBase obj, string propValue, string propName)
        {
            if (!string.IsNullOrEmpty(propValue))
            {
                var urlValue = propValue.ToUpper();

                if (urlValue.StartsWith("//")
                    || urlValue.StartsWith("HTTP"))
                {

                    throw new SPMeta2ModelValidationException(
                        string.Format("There is a not-well formed URL string property. Should not be absolute. Use tokens: [{0}] Definition:[{1}] Value:[{2}]",
                            propName, obj, propValue))
                    {
                        Definition = obj
                    };
                }
            }
        }

        #endregion
    }
}
