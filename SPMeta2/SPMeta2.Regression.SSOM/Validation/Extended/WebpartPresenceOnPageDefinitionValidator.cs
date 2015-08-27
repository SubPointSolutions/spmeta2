using System;
using System.Linq;
using System.Net;
using Microsoft.SharePoint;
using SPMeta2.Containers.Assertion;
using SPMeta2.Containers.Utils;
using SPMeta2.Definitions;
using SPMeta2.Regression.Definitions.Extended;
using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;

namespace SPMeta2.Regression.SSOM.Validation.Extended
{
    public class ClientWebpartPresenceOnPageDefinitionValidator : WikiPageModelHandler
    {
        public override Type TargetType
        {
            get { return typeof(WebpartPresenceOnPageDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            SPFolder targetFolder = null;
            SPWeb web = null; 

            if (modelHost is ListModelHost)
            {
                targetFolder = (modelHost as ListModelHost).HostList.RootFolder;
                web = (modelHost as ListModelHost).HostList.ParentWeb;
            }
            if (modelHost is FolderModelHost)
            {
                targetFolder = (modelHost as FolderModelHost).CurrentLibraryFolder;
                web = (modelHost as FolderModelHost).CurrentWeb;
            }

            var definition = model.WithAssertAndCast<WebpartPresenceOnPageDefinition>("model", value => value.RequireNotNull());

            var folder = targetFolder;

            var pageName = GetSafeWikiPageFileName(definition.PageFileName);
            var file = web.GetFile(UrlUtility.CombineUrl(folder.ServerRelativeUrl, pageName));

            var serverUrl = web.Url;

            if (web.ServerRelativeUrl != "/")
                serverUrl = serverUrl.Replace(web.ServerRelativeUrl, string.Empty);

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, file)
                // dont' need to check that, not the pupose of the test
                //.ShouldBeEqual(m => m.PageFileName, o => o.GetName())
                .ShouldNotBeNull(file);

            var pageUrl = UrlUtility.CombineUrl(new[]{
                serverUrl, 
                folder.ServerRelativeUrl,
                pageName});

            if (definition.WebPartDefinitions.Any())
            {
                assert.ShouldBeEqual((p, s, d) =>
                {
                    var client = new WebClient();
                    client.UseDefaultCredentials = true;

                    var pageContent = client.DownloadString(new Uri(pageUrl));
                    var srcProp = s.GetExpressionValue(m => m.WebPartDefinitions);

                    var isValid = true;

                    TraceUtils.WithScope(trace =>
                    {
                        trace.WriteLine(string.Format("Checking web part presence on the page:[{0}]", pageUrl));

                        // so, essentially there should be a span with web part title
                        foreach (var webpart in definition.WebPartDefinitions)
                        {
                            var targetSpan = string.Format("<span>{0}</span>", webpart.Title);
                            var hasWebPart = pageContent.Contains(targetSpan);

                            if (!hasWebPart)
                            {
                                trace.WriteLine(
                                    string.Format("[ERR] Page [{0}] misses web part with title:[{1}] and def:[{2}]",
                                        new object[]
                                        {
                                            pageUrl,
                                            webpart.Title,
                                            webpart
                                        }));

                                isValid = false;
                            }
                            else
                            {
                                trace.WriteLine(string.Format("[True] Page [{0}] has web part with title:[{1}] and def:[{2}]",
                                    new object[]
                                    {
                                        pageUrl,
                                        webpart.Title,
                                        webpart
                                    }));
                            }
                        }
                    });

                    return new PropertyValidationResult
                    {
                        Tag = p.Tag,
                        Src = srcProp,
                        //Dst = dstProp,
                        IsValid = isValid
                    };
                });
            }
            else
            {
                assert.SkipProperty(m => m.WebPartDefinitions, "WebPartDefinitions.Count = 0. Skipping");
            }

        }

       
    }
}
