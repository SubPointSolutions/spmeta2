using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Attributes
{
    /// <summary>
    /// Used internally by regression testing infrastructure.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SPObjectTypeAttribute : Attribute
    {
        #region constructors

        public SPObjectTypeAttribute(SPObjectModelType objectModelType, string className, string assemblyName)
        {
            ObjectModelType = objectModelType;

            ClassName = className;
            AssemblyName = assemblyName;
        }

        #endregion

        #region properties

        public SPObjectModelType ObjectModelType { get; set; }

        public string ClassName { get; set; }
        public string AssemblyName { get; set; }

        #endregion
    }
}
