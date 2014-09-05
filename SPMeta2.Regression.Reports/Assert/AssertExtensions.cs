using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using SPMeta2.Utils;

namespace SPMeta2.Regression.Reports.Assert
{
    public static class AssertExtensions
    {
        public static AssertPair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
           this AssertPair<TModel, TObj> pair,
           Expression<Func<TModel, string>> modelPropExp,
           Expression<Func<TObj, string>> objPropLExp)
        {
            var srcProp = pair.Src.GetExpressionValue<TModel, string>(modelPropExp);
            var objProp = pair.Dst.GetExpressionValue<TObj, string>(objPropLExp);

            pair.InvokeOnValidateProperty(new OnValidatePropertyEventArgs<TModel, TObj>
            {
                Assert = pair,

                SrcPropertyName = srcProp.Name,
                SrcPropertyValue = srcProp.Value,
                SrcPropertyType = srcProp.ObjectType.ToString(),

                DstPropertyName = objProp.Name,
                DstPropertyValue = objProp.Value,
                DstPropertyType = srcProp.ObjectType.ToString(),
            });

            pair.AreEqual(srcProp.Value, objProp.Value);

            return pair;
        }

        public static AssertPair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
            this AssertPair<TModel, TObj> pair,
            Expression<Func<TModel, Guid>> modelPropExp,
            Expression<Func<TObj, Guid>> objPropLExp)
        {
            var srcProp = pair.Src.GetExpressionValue<TModel, Guid>(modelPropExp);
            var objProp = pair.Dst.GetExpressionValue<TObj, Guid>(objPropLExp);

            pair.InvokeOnValidateProperty(new OnValidatePropertyEventArgs<TModel, TObj>
            {
                Assert = pair,

                SrcPropertyName = srcProp.Name,
                SrcPropertyValue = srcProp.Value,
                SrcPropertyType = srcProp.ObjectType.ToString(),

                DstPropertyName = objProp.Name,
                DstPropertyValue = objProp.Value,
                DstPropertyType = srcProp.ObjectType.ToString(),
            });

            pair.AreEqual(srcProp.Value, objProp.Value);

            return pair;
        }

        public static AssertPair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
            this AssertPair<TModel, TObj> pair,
            Expression<Func<TModel, bool>> modelPropExp,
            Expression<Func<TObj, bool>> objPropLExp)
        {
            var srcProp = pair.Src.GetExpressionValue<TModel, bool>(modelPropExp);
            var objProp = pair.Dst.GetExpressionValue<TObj, bool>(objPropLExp);

            pair.InvokeOnValidateProperty(new OnValidatePropertyEventArgs<TModel, TObj>
            {
                Assert = pair,

                SrcPropertyName = srcProp.Name,
                SrcPropertyValue = srcProp.Value,
                SrcPropertyType = srcProp.ObjectType.ToString(),

                DstPropertyName = objProp.Name,
                DstPropertyValue = objProp.Value,
                DstPropertyType = srcProp.ObjectType.ToString(),
            });

            pair.AreEqual(srcProp.Value, objProp.Value);

            return pair;
        }

        public static AssertPair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
           this AssertPair<TModel, TObj> pair,
           Expression<Func<TModel, int>> modelPropExp,
           Expression<Func<TObj, int>> objPropLExp)
        {
            var srcProp = pair.Src.GetExpressionValue<TModel, int>(modelPropExp);
            var objProp = pair.Dst.GetExpressionValue<TObj, int>(objPropLExp);

            pair.InvokeOnValidateProperty(new OnValidatePropertyEventArgs<TModel, TObj>
            {
                Assert = pair,

                SrcPropertyName = srcProp.Name,
                SrcPropertyValue = srcProp.Value,
                SrcPropertyType = srcProp.ObjectType.ToString(),

                DstPropertyName = objProp.Name,
                DstPropertyValue = objProp.Value,
                DstPropertyType = srcProp.ObjectType.ToString(),
            });

            pair.AreEqual(srcProp.Value, objProp.Value);

            return pair;
        }


        public static AssertPair<TModel, TObj> SkipProperty<TModel, TObj>(
           this AssertPair<TModel, TObj> pair,
            Expression<Func<TModel, int>> modelPropExp,
            string skipMessage)
        {
            var srcProp = ((TModel)pair.Src).GetExpressionValue<TModel, int>(modelPropExp);

            pair.InvokeOnValidateProperty(new OnValidatePropertyEventArgs<TModel, TObj>
            {
                Assert = pair,

                SrcPropertyName = srcProp.Name,
                SrcPropertyValue = srcProp.Value,
                SrcPropertyType = srcProp.ObjectType.ToString(),

                //DstPropertyName = objProp.Name,
                //DstPropertyValue = objProp.Value,
                //DstPropertyType = srcProp.ObjectType.ToString(),
            });

            //reportItem.AddProperty(new TestReportNodeProperty
            //{
            //    SourcePropName = srcProp.Name,

            //    SourcePropValue = srcProp.Value,

            //    Comment = skipMessage
            //});

            // TODO
            // add skip message for the property


            return pair;
        }
    }
}
