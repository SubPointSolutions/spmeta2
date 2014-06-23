using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Regression.Common;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Utils
{
    public static class AssertExtensions
    {
        public static ComparePair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
            this ComparePair<TModel, TObj> pair,
            TraceUtils trace,
            Expression<Func<TModel, string>> modelPropExp,
            Expression<Func<TObj, string>> objPropLExp)
        {
            var modelProp = pair.Model.GetExpressionValue<TModel, string>(modelPropExp);
            var objProp = pair.Object.GetExpressionValue<TObj, string>(objPropLExp);

            trace.WriteLine(string.Format("Model prop [{0}] on obj prop [{1}]: model:[{2}] obj:[{3}]",
               modelProp.Name,
               objProp.Name,
               modelProp.Value,
               objProp.Value));

            Assert.AreEqual(modelProp.Value, objProp.Value);

            return pair;
        }

        public static ComparePair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
            this ComparePair<TModel, TObj> pair,
            TraceUtils trace,
            Expression<Func<TModel, Guid>> modelPropExp,
            Expression<Func<TObj, Guid>> objPropLExp)
        {
            var modelProp = pair.Model.GetExpressionValue<TModel, Guid>(modelPropExp);
            var objProp = pair.Object.GetExpressionValue<TObj, Guid>(objPropLExp);

            trace.WriteLine(string.Format("Model prop [{0}] on obj prop [{1}]: model:[{2}] obj:[{3}]",
               modelProp.Name,
               objProp.Name,
               modelProp.Value,
               objProp.Value));

            Assert.AreEqual(modelProp.Value, objProp.Value);

            return pair;
        }

        public static ComparePair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
            this ComparePair<TModel, TObj> pair,
            TraceUtils trace,
            Expression<Func<TModel, bool>> modelPropExp,
            Expression<Func<TObj, bool>> objPropLExp)
        {
            var modelProp = pair.Model.GetExpressionValue<TModel, bool>(modelPropExp);
            var objProp = pair.Object.GetExpressionValue<TObj, bool>(objPropLExp);

            trace.WriteLine(string.Format("Model prop [{0}] on obj prop [{1}]: model:[{2}] obj:[{3}]",
               modelProp.Name,
               objProp.Name,
               modelProp.Value,
               objProp.Value));

            Assert.AreEqual(modelProp.Value, objProp.Value);

            return pair;
        }

        public static ComparePair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
           this ComparePair<TModel, TObj> pair,
           TraceUtils trace,
           Expression<Func<TModel, int>> modelPropExp,
           Expression<Func<TObj, int>> objPropLExp)
        {
            var modelProp = pair.Model.GetExpressionValue<TModel, int>(modelPropExp);
            var objProp = pair.Object.GetExpressionValue<TObj, int>(objPropLExp);

            trace.WriteLine(string.Format("Model prop [{0}] on obj prop [{1}]: model:[{2}] obj:[{3}]",
               modelProp.Name,
               objProp.Name,
               modelProp.Value,
               objProp.Value));

            Assert.AreEqual(modelProp.Value, objProp.Value);

            return pair;
        }
    }
}
