using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Regression.Reports.Data;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Reports.Extensions
{
    public static class AssertExtensions
    {
        public static TypedTestReportNode<TModel, TObj> ShouldBeEqual<TModel, TObj>(
            this TypedTestReportNode<TModel, TObj> reportItem,
            Expression<Func<TModel, string>> modelPropExp,
            Expression<Func<TObj, string>> objPropLExp)
        {
            var srcProp = reportItem.SourceTypedValue.GetExpressionValue<TModel, string>(modelPropExp);
            var dstProp = reportItem.DestTypedValue.GetExpressionValue<TObj, string>(objPropLExp);

            reportItem.AddProperty(new TestReportNodeProperty
            {
                SourcePropName = srcProp.Name,
                DestPropName = dstProp.Name,

                SourcePropValue = srcProp.Value,
                DestPropValue = dstProp.Value
            });

            // TODO
            //Assert.AreEqual(modelProp.Value, objProp.Value);

            return reportItem;
        }

        public static TypedTestReportNode<TModel, TObj> ShouldBeEqual<TModel, TObj>(
            this TypedTestReportNode<TModel, TObj> reportItem,
            Expression<Func<TModel, Guid>> modelPropExp,
            Expression<Func<TObj, Guid>> objPropLExp)
        {
            var srcProp = reportItem.SourceTypedValue.GetExpressionValue<TModel, Guid>(modelPropExp);
            var dstProp = reportItem.DestTypedValue.GetExpressionValue<TObj, Guid>(objPropLExp);

            reportItem.AddProperty(new TestReportNodeProperty
            {
                SourcePropName = srcProp.Name,
                DestPropName = dstProp.Name,

                SourcePropValue = srcProp.Value,
                DestPropValue = dstProp.Value
            });

            // TODO
            //Assert.AreEqual(modelProp.Value, objProp.Value);

            return reportItem;
        }

        public static TypedTestReportNode<TModel, TObj> ShouldBeEqual<TModel, TObj>(
            this TypedTestReportNode<TModel, TObj> reportItem,
            Expression<Func<TModel, bool>> modelPropExp,
            Expression<Func<TObj, bool>> objPropLExp)
        {
            var srcProp = reportItem.SourceTypedValue.GetExpressionValue<TModel, bool>(modelPropExp);
            var dstProp = reportItem.DestTypedValue.GetExpressionValue<TObj, bool>(objPropLExp);

            reportItem.AddProperty(new TestReportNodeProperty
            {
                SourcePropName = srcProp.Name,
                DestPropName = dstProp.Name,

                SourcePropValue = srcProp.Value,
                DestPropValue = dstProp.Value
            });

            // TODO
            //Assert.AreEqual(modelProp.Value, objProp.Value);

            return reportItem;
        }

        public static TypedTestReportNode<TModel, TObj> ShouldBeEqual<TModel, TObj>(
           this TypedTestReportNode<TModel, TObj> reportItem,
           Expression<Func<TModel, int>> modelPropExp,
           Expression<Func<TObj, int>> objPropLExp)
        {
            var srcProp = reportItem.SourceTypedValue.GetExpressionValue<TModel, int>(modelPropExp);
            var dstProp = reportItem.DestTypedValue.GetExpressionValue<TObj, int>(objPropLExp);

            reportItem.AddProperty(new TestReportNodeProperty
            {
                SourcePropName = srcProp.Name,
                DestPropName = dstProp.Name,

                SourcePropValue = srcProp.Value,
                DestPropValue = dstProp.Value
            });

            // TODO
            //Assert.AreEqual(modelProp.Value, objProp.Value);

            return reportItem;
        }

        public static TypedTestReportNode<TModel, TObj> SkipProperty<TModel, TObj>(
            this TypedTestReportNode<TModel, TObj> reportItem,
            Expression<Func<TModel, int>> modelPropExp,
            string skipMessage)
        {
            var srcProp = reportItem.SourceTypedValue.GetExpressionValue<TModel, int>(modelPropExp);

            reportItem.AddProperty(new TestReportNodeProperty
            {
                SourcePropName = srcProp.Name,

                SourcePropValue = srcProp.Value,

                Comment = skipMessage
            });

            // TODO
            // add skip message for the property


            return reportItem;
        }
    }
}
