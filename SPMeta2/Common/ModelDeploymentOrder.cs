using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Common
{
    public class ModelWeigh
    {
        #region contructors

        public ModelWeigh()
        {

        }

        public ModelWeigh(Type model, IEnumerable<Type> childModels)
        {
            Model = model;

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

        public Type Model { get; set; }
        public Dictionary<Type, int> ChildModels { get; set; }

        #endregion
    }
}
