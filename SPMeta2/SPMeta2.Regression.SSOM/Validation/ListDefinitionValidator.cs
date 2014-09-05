using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Regression.Common.Utils;
using SPMeta2.SSOM.DefaultSyntax;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class ListDefinitionValidator : ListModelHandler
    {
        protected override void DeployModelInternal(object modelHost, DefinitionBase model)
        {
            var webModelHost = modelHost.WithAssertAndCast<WebModelHost>("modelHost", value => value.RequireNotNull());
            var web = webModelHost.HostWeb;

            var definition = model.WithAssertAndCast<ListDefinition>("model", value => value.RequireNotNull());
            var spObject = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, definition.GetListUrl()));

            var assert = ServiceFactory.AssertService.NewAssert(model, definition, spObject);

            assert
                .ShouldBeEqual(m => m.Title, o => o.Title)
                .ShouldBeEqual(m => m.Description, o => o.Description)
                .ShouldBeEndOf(m => m.GetListUrl(), m => m.Url, o => o.GetServerRelativeUrl(), o => o.GetServerRelativeUrl())
                .ShouldBeEqual(m => m.ContentTypesEnabled, o => o.ContentTypesEnabled);

            if (definition.TemplateType > 0)
            {
                assert
                    .ShouldBeEqual(m => m.TemplateType, o => (int)o.BaseTemplate)
                    .SkipProperty(m => m.TemplateName, "Skipping from validation. TemplateType should be == 0");
            }
            else
            {
                assert
                    //.ShouldBeEqual(m => m.TemplateName, o => (int)o.BaseTemplate)
                    .SkipProperty(m => m.TemplateType, "Skipping from validation. TemplateType should be > 0");
            }

            //TraceUtils.WithScope(traceScope =>
            //{


            //    traceScope.WriteLine(string.Format("Validate model:[{0}] field:[{1}]", definition, spObject));

            //    // assert base properties
            //    traceScope.WithTraceIndent(trace =>
            //    {
            //        trace.WriteLine(string.Format("Validate Title: model:[{0}] list:[{1}]", definition.Title, spObject.Title));
            //        Assert.AreEqual(definition.Title, spObject.Title);

            //        trace.WriteLine(string.Format("Validate Description: model:[{0}] list:[{1}]", definition.Description, spObject.Description));
            //        Assert.AreEqual(definition.Description, spObject.Description);

            //        trace.WriteLine(string.Format("Validate ContentTypesEnabled: model:[{0}] list:[{1}]", definition.ContentTypesEnabled, spObject.ContentTypesEnabled));
            //        Assert.AreEqual(definition.ContentTypesEnabled, spObject.ContentTypesEnabled);

            //        // TODO
            //        // template type & template name
            //        if (definition.TemplateType > 0)
            //        {
            //            trace.WriteLine(string.Format("Validate TemplateType: model:[{0}] list:[{1}]", definition.TemplateType, spObject.BaseTemplate));
            //            Assert.AreEqual(definition.TemplateType, (int)spObject.BaseTemplate);
            //        }
            //        else
            //        {
            //            trace.WriteLine(string.Format("Skipping TemplateType check. It is 0"));
            //        }

            //        // TODO
            //        // url checking
            //        trace.WriteLine(string.Format("Validate Url: model:[{0}] list:[{1}]", definition.GetListUrl(), spObject.RootFolder.ServerRelativeUrl));
            //        Assert.IsTrue(spObject.RootFolder.ServerRelativeUrl.Contains(definition.GetListUrl()));
            //    });
            //});
        }
    }

    public static class ListExtensions
    {
        public static string GetServerRelativeUrl(this SPList list)
        {
            return list.RootFolder.ServerRelativeUrl;
        }
    }
}
