using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using SPMeta2.Containers.Assertion;
using SPMeta2.CSOM.Extensions;
using SPMeta2.CSOM.ModelHandlers;
using SPMeta2.CSOM.ModelHosts;
using SPMeta2.Definitions;
using SPMeta2.Regression.Definitions;
using SPMeta2.Utils;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Containers.Utils;

namespace SPMeta2.Regression.CSOM.Validation
{
    public class ClientWebpartPresenceOnPageDefinitionValidator : WikiPageModelHandler
    {
        public override Type TargetType
        {
            get { return typeof(WebpartPresenceOnPageDefinition); }
        }

        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var folderModelHost = modelHost.WithAssertAndCast<ListModelHost>("modelHost", value => value.RequireNotNull());
            var definition = model.WithAssertAndCast<WebpartPresenceOnPageDefinition>("model", value => value.RequireNotNull());

            var folder = folderModelHost.HostList.RootFolder;
            var context = folder.Context;

            var pageName = GetSafeWikiPageFileName(definition.PageFileName);
            var file = GetPageFile(folderModelHost.HostWeb, folder, pageName);
            var spObject = file.ListItemAllFields;

            context.Load(spObject, item => item["EncodedAbsUrl"], item => item["FileLeafRef"]);
            context.ExecuteQueryWithTrace();

            var pageUrl = spObject["EncodedAbsUrl"].ToString();

            var assert = ServiceFactory.AssertService
                .NewAssert(definition, spObject)
                .ShouldBeEqual(m => m.PageFileName, o => o.GetName())
                .ShouldNotBeNull(spObject);

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

        protected File GetPageFile(Web web, Folder folder, string pageName)
        {
            var context = folder.Context;

            context.Load(folder, l => l.ServerRelativeUrl);
            context.ExecuteQueryWithTrace();

            var newWikiPageUrl = folder.ServerRelativeUrl + "/" + pageName;

            var file = web.GetFileByServerRelativeUrl(newWikiPageUrl);

            context.Load(file, f => f.Exists);
            context.ExecuteQueryWithTrace();

            if (file.Exists)
            {
                return file;
            }

            return null;
        }
    }
}
