using System;
using System.Collections.Generic;
using System.Linq;
using SPMeta2.Models;

namespace SPMeta2.Extensions
{
    public static class ModelDefinitionExtensions
    {
        #region methods

        public static IEnumerable<ModelNode> GetChildModels<TChildModelType>(this ModelNode model)
        {
            return GetChildModels(model, typeof(TChildModelType));
        }

        public static IEnumerable<ModelNode> GetChildModels(this ModelNode model, Type modelType)
        {
            return model.ChildModels.Where(m => m.Value.GetType() == modelType);
        }

        public static IEnumerable<TChildModelType> GetChildModelsAsType<TChildModelType>(this ModelNode model)
        {
            return GetChildModels(model, typeof(TChildModelType)).Cast<TChildModelType>();
        }

        #endregion
    }
}
