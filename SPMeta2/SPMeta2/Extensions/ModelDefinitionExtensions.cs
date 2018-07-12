using System;
using System.Collections.Generic;
using System.Linq;

using SPMeta2.Models;

namespace SPMeta2.Extensions
{
    /// <summary>
    /// Internal usage only.
    /// </summary>
    public static class ModelDefinitionExtensions
    {
        #region methods

        /// <summary>
        /// Returns child model of the particular type.
        /// </summary>
        /// <typeparam name="TChildModelType"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<ModelNode> GetChildModels<TChildModelType>(this ModelNode model)
        {
            return GetChildModels(model, typeof(TChildModelType));
        }

        /// <summary>
        /// Returns child model of the particular type.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelType"></param>
        /// <returns></returns>
        public static List<ModelNode> GetChildModels(this ModelNode model, Type modelType)
        {
            return model.ChildModels.Where(m => m.Value.GetType() == modelType).ToList();
        }

        /// <summary>
        /// Returns child model of the particular type performing casting to the target type.
        /// </summary>
        /// <typeparam name="TChildModelType"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<TChildModelType> GetChildModelsAsType<TChildModelType>(this ModelNode model)
        {
            return GetChildModels(model, typeof(TChildModelType)).Cast<TChildModelType>().ToList();
        }

        #endregion
    }
}
