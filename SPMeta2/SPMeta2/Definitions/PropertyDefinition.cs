using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using System;
using SPMeta2.Definitions.Base;
using SPMeta2.Utils;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy property value to the SharePoint property bags.
    /// </summary>
    /// 

    [SPObjectTypeAttribute(SPObjectModelType.SSOM, "System.Object", "mscorlib")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "System.Object", "mscorlib")]

    [DefaultRootHostAttribute(typeof(WebDefinition))]
    [DefaultParentHostAttribute(typeof(WebDefinition))]

    [Serializable]
    [ExpectWithExtensionMethod]
    public class PropertyDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Name of the target property.
        /// </summary>
        /// 
        [ExpectValidation]
        public string Key { get; set; }

        /// <summary>
        /// Value of the target property.
        /// </summary>
        /// 
        [ExpectValidation]
        public object Value { get; set; }

        /// <summary>
        /// Should value be overwritten
        /// </summary>
        /// 
        [ExpectValidation]
        public bool Overwrite { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<PropertyDefinition>(this)
                          .AddPropertyValue(p => p.Key)
                          .AddPropertyValue(p => p.Value)
                          .AddPropertyValue(p => p.Overwrite)

                          .ToString();
        }

        #endregion
    }
}
