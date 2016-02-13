using System;
using System.Collections.Generic;

namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Used by regression testing infrastructure to build up a 'sandbox' model tree for the given definition.
    /// This attribute indicates parent definition to the given definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DefaultParentHostAttribute : Attribute
    {
        #region constructors

        public DefaultParentHostAttribute(Type hostType)
            : this(hostType, null)
        {
        }

        public DefaultParentHostAttribute(Type hostType, params Type[] additionalHostTypes)
        {
            HostType = hostType;

            AdditionalHostTypes = new List<Type>();

            if (additionalHostTypes != null)
                AdditionalHostTypes.AddRange(additionalHostTypes);
        } 

        #endregion

        #region properties

        public Type HostType { get; set; }
        public List<Type> AdditionalHostTypes { get; set; } 

        #endregion
    }

    /// <summary>
    /// Used by regression testing infrastructure to build up a 'sandbox' model tree for the given definition.
    /// This attribute indicates root hos (site, web, farm, etc) definition to the given definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DefaultRootHostAttribute : Attribute
    {
        #region constructors

        public DefaultRootHostAttribute(Type hostType)
        {
            HostType = hostType;
        } 

        #endregion

        #region properties

        public Type HostType { get; set; } 
        
        #endregion
    }

    /// <summary>
    /// Used by regression testing infrastructure to build up a 'sandbox' model tree for the given definition.
    /// This attribute indicates CSOM related parent for the given definition.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CSOMParentHostAttribute : DefaultParentHostAttribute
    {
        public CSOMParentHostAttribute(Type hostType)
            : base(hostType)
        {

        }
    }

    /// Used by regression testing infrastructure to build up a 'sandbox' model tree for the given definition.
    /// This attribute indicates CSOM related root (web, site, etc) for the given definition.
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CSOMRootHostAttribute : DefaultRootHostAttribute
    {
        public CSOMRootHostAttribute(Type hostType)
            : base(hostType)
        {

        }
    }
}
