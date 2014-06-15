using System;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.Utils;
using System.Linq.Expressions;

namespace SPMeta2.Regression.Validation.ServerModelHandlers
{
    public class WebDefinitionValidator : WebModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var parentWeb = modelHost.WithAssertAndCast<SPWeb>("modelHost", value => value.RequireNotNull());
            var _model = model.WithAssertAndCast<WebDefinition>("model", value => value.RequireNotNull());

            TraceUtils.WithScope(traceScope =>
            {
                var web = GetWeb(parentWeb, _model);
                var pair = new ComparePair<WebDefinition, SPWeb>(_model, web);

                traceScope.WriteLine(string.Format("Validating model:[{0}] web:[{1}]", _model, web));

                traceScope.WithTraceIndent(trace =>
                {
                    pair
                        .ShouldBeEqual(trace, m => m.Title, w => w.Title)
                        .ShouldBeEqual(trace, m => m.Description, w => w.Description);
                        //.ShouldBeEqual(trace, m => m.Url, w => w.Url);


                    //trace.WriteLine(string.Format("Validating Title: model:[{0}] field:[{1}]", _model.Title, web.Title));
                    //Assert.AreEqual(_model.Title, web.Title);

                    //var webTemplate = string.Format("{0}#{1}", web.WebTemplate, web.Configuration);

                    //trace.WriteLine(string.Format("Validating WebTemplate: model:[{0}] field:[{1}]", _model.WebTemplate, webTemplate));
                    //Assert.AreEqual(_model.WebTemplate, webTemplate);
                });
            });

        }

    }

    public class ComparePair<TModel, TObj>
    {
        public ComparePair(TModel model, TObj obj)
        {
            Model = model;
            Object = obj;
        }

        public TModel Model { get; set; }
        public TObj Object { get; set; }
    }

    public static class AssertExtensions
    {
        //public static ComparePair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
        //    this ComparePair<TModel, TObj> pair,
        //    TraceUtils trace,
        //    Expression<Func<TModel, string>> modelPropExp,
        //    string objPropName,
        //    string objValue)
        //{


        //    return pair;
        //}

        //public static ComparePair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
        //    this ComparePair<TModel, TObj> pair,
        //    TraceUtils trace,
        //    string modelPropName,
        //    string modelValue,
        //    string objPropName,
        //    string objValue)
        //{
        //    trace.WriteLine(string.Format("Validating model prop [{0}] on obj prop [{1}]: model:[{2}] obj:[{3}]",
        //        modelPropName,
        //        objPropName,
        //        modelValue,
        //        objValue));

        //    Assert.AreEqual(modelValue, objValue);
        //}

        public static ComparePair<TModel, TObj> ShouldBeEqual<TModel, TObj>(
            this ComparePair<TModel, TObj> pair,
            TraceUtils trace,
            Expression<Func<TModel, string>> modelPropExp,
            Expression<Func<TObj, string>> objPropLExp)
        {
            var modelProp = pair.Model.GetPropertyValue<TModel, string>(modelPropExp);
            var objProp = pair.Object.GetPropertyValue<TObj, string>(objPropLExp);

            trace.WriteLine(string.Format("Validating model prop [{0}] on obj prop [{1}]: model:[{2}] obj:[{3}]",
               modelProp.Name,
               objProp.Name,
               modelProp.Value,
               objProp.Value));

            Assert.AreEqual(modelProp.Value, objProp.Value);


            return pair;
        }
    }
}
