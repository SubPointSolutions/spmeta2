using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Utils;

namespace SPMeta2.Services.ServiceModelHandlers
{
    public class DefaultVersionBasedPropertiesModelHandler : ServiceModelHandlerBase
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var props = ReflectionUtils.GetPropertiesWithCustomAttribute<VersionPropertyCapabilityAttribute>(model, true);

            ValidateProps(model, props.Where(p => p.GetCustomAttributes(typeof(VersionPropertyCapabilityAttribute), true).FirstOrDefault() is VersionPropertyCapabilityAttribute));
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
                    ValidateVersionString(obj, propValue, propName);
            }
        }

        private static void ValidateVersionString(DefinitionBase obj, string propValue, string propName)
        {
            try
            {
                // Correct version string
                // https://github.com/SubPointSolutions/spmeta2/issues/640

                if (!string.IsNullOrEmpty(propValue))
                {
                    // ReSharper disable once ObjectCreationAsStatement
                    new Version(propValue);
                }
            }
            catch (Exception ee)
            {
                throw new SPMeta2ModelValidationException(
                    string.Format("There is a not-well formed version string property: [{0}] Definition:[{1}] Value:[{2}]",
                        propName, obj, propValue), ee)
                {
                    Definition = obj
                };
            }
        }


        #endregion
    }
}
