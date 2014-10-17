using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Common
{
    /// <summary>
    /// Indicates default model ordering while provision process.
    /// For instance, fields are to be provisioned before content types, workflow definitions before list definitions, etc.
    /// </summary>
    public class ModelWeigh
    {
        #region constructors

        public ModelWeigh()
        {

        }

        public ModelWeigh(Type model, IEnumerable<Type> childModels)
        {
            Model = model;
            ChildModels = new Dictionary<Type, int>();

            var index = 0;

            foreach (var childModel in childModels)
            {
                if (!ChildModels.ContainsKey(childModel))
                {
                    index += 100;
                    ChildModels.Add(childModel, index);
                }
            }
        }

        #endregion

        #region properties

        /// <summary>
        /// Target model type.
        /// </summary>
        public Type Model { get; set; }

        /// <summary>
        /// Child model types and their weighs.
        /// </summary>
        public Dictionary<Type, int> ChildModels { get; set; }

        #endregion
    }
}
