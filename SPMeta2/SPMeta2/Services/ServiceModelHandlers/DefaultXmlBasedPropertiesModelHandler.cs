using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Definitions;
using SPMeta2.Exceptions;
using SPMeta2.Utils;

namespace SPMeta2.Services.ServiceModelHandlers
{
    public class DefaultXmlBasedPropertiesModelHandler : ServiceModelHandlerBase
    {
        #region methods

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var props = ReflectionUtils.GetPropertiesWithCustomAttribute<XmlPropertyCapabilityAttribute>(model, true);

            ValidateXmlProps(model, props.Where(p => p.GetCustomAttributes(typeof(XmlPropertyCapabilityAttribute), true).FirstOrDefault() is XmlPropertyCapabilityAttribute));
            ValidateCamlProps(model, props.Where(p => p.GetCustomAttributes(typeof(CamlPropertyCapabilityAttribute), true).FirstOrDefault() is CamlPropertyCapabilityAttribute));
        }

        protected virtual string ExtractXmlStringValue(DefinitionBase obj, PropertyInfo prop)
        {
            var value = prop.GetValue(obj, null);

            if (value is string && !string.IsNullOrEmpty(ConvertUtils.ToString(value)))
                return value.ToString();

            return string.Empty;
        }

        protected virtual void ValidateCamlProps(DefinitionBase obj, IEnumerable<PropertyInfo> props)
        {
            // will implement additional CAML validation later
            // so far, same same XML shot
            ValidateXmlPropertyInternal(obj, props);
        }

        protected virtual void ValidateXmlProps(DefinitionBase obj, IEnumerable<PropertyInfo> props)
        {
            ValidateXmlPropertyInternal(obj, props);
        }


        private void ValidateXmlPropertyInternal(DefinitionBase obj, IEnumerable<PropertyInfo> props)
        {
            foreach (var prop in props)
            {
                var propName = prop.Name;
                var propValue = ExtractXmlStringValue(obj, prop);

                if (!string.IsNullOrEmpty(propValue))
                    ValidateXmlString(obj, propValue, propName);
            }
        }

        private static void ValidateXmlString(DefinitionBase obj, string propValue, string propName)
        {
            try
            {
                //  Well formed XML validation throws errors on some XMLs #608
                //  https://github.com/SubPointSolutions/spmeta2/issues/608
                //  
                // handling bits of XML such as list view CAML
                // need to surround it with 'RootNode', a single node 
                if (!string.IsNullOrEmpty(propValue) && !propValue.ToUpper().Contains("<?XML"))
                {
                    propValue = string.Format("<RootNode_{0}>{1}</RootNode_{0}>",
                        Guid.NewGuid().ToString("N"),
                        propValue);
                }

                var doc = XDocument.Parse(propValue);
            }
            catch (Exception ee)
            {
                throw new SPMeta2ModelValidationException(
                    string.Format("There is a not-well formed XML on property: [{0}] Definition:[{1}] XML:[{2}]",
                        propName, obj, propValue), ee)
                {
                    Definition = obj
                };
            }
        }


        #endregion
    }
}
