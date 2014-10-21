using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Services.Webparts
{
    /// <summary>
    /// Represent ParameterBinding option for the target web part. 
    /// </summary>
    public class WebPartParameterBindingsOptions
    {
        #region constructors

        public WebPartParameterBindingsOptions()
        {
            ParameterBindings = new Dictionary<string, string>();
        }

        #endregion

        #region properties

        protected Dictionary<string, string> ParameterBindings { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Add new parameter with given name and value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public virtual WebPartParameterBindingsOptions AddParameterBinding(string name, string location)
        {
            if (!ParameterBindings.ContainsKey(name))
                ParameterBindings.Add(name, location);
            else
                ParameterBindings[name] = location;

            return this;
        }

        /// <summary>
        /// Returns rendered ParameterBinding ready to be binded to the target web part property.
        /// </summary>
        public virtual string ParameterBinding
        {
            get { return CreateParameterBindingString(); }
        }

        private string CreateParameterBindingString()
        {
            var result = string.Empty;

            foreach (var name in ParameterBindings.Keys)
                result += string.Format("<ParameterBinding Name=\"{0}\" Location=\"{1}\" />{2}", name, ParameterBindings[name], Environment.NewLine);

            return result;
        }

        #endregion
    }
}
