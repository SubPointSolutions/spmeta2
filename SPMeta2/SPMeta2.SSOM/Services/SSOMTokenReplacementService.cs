using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.SharePoint;
using SPMeta2.Exceptions;
using SPMeta2.Services;

namespace SPMeta2.SSOM.Services
{
    public class SSOMTokenReplacementService : TokenReplacementServiceBase
    {
        #region constructors

        public SSOMTokenReplacementService()
        {
            SupportedTokensInternal.Clear();

            TokenProcessInfos.Add(new TokenProcessInfo
            {
                Name = "~sitecollection",
                RegEx = new Regex("~sitecollection", RegexOptions.IgnoreCase)
            });

            TokenProcessInfos.Add(new TokenProcessInfo
            {
                Name = "~site",
                RegEx = new Regex("~site", RegexOptions.IgnoreCase)
            });

            SupportedTokensInternal.AddRange(TokenProcessInfos.Select(i => new TokenInfo { Name = i.Name }));
        }

        #endregion

        #region classes

        private class TokenProcessInfo
        {
            public string Name { get; set; }
            public Regex RegEx { get; set; }
        }

        #endregion

        #region properties

        private List<TokenProcessInfo> TokenProcessInfos = new List<TokenProcessInfo>();

        #endregion

        #region methods

        public override TokenReplacementResult ReplaceTokens(TokenReplacementContext context)
        {
            var result = new TokenReplacementResult
            {
                Value = context.Value
            };

            if (string.IsNullOrEmpty(result.Value))
                return result;

            foreach (var tokenInfo in TokenProcessInfos)
            {
                if (!string.IsNullOrEmpty(result.Value))
                {
                    var replacedValue = tokenInfo.RegEx.Replace(result.Value, ResolveToken(context, context.Context, tokenInfo.Name));

                    if (!string.IsNullOrEmpty(replacedValue))
                    {
                        // everything to '/'
                        replacedValue = replacedValue.Replace(@"\", @"/");

                        // replace doubles after '://'
                        var urlParts = replacedValue.Split(new string[] { "://" }, StringSplitOptions.RemoveEmptyEntries);

                        // return non 'protocol://' values
                        if (urlParts.Count() == 1)
                        {
                            result.Value = urlParts[0].Replace(@"//", @"/");
                        }
                        else
                        {
                            var resultValues = new List<string>();

                            resultValues.Add(urlParts[0]);

                            foreach (var value in urlParts.Skip(1))
                            {
                                resultValues.Add(value.Replace(@"//", @"/"));
                            }

                            result.Value = string.Join("://", resultValues.ToArray());
                        }
                    }
                    else
                    {
                        result.Value = replacedValue;
                    }
                }
            }

            // remove ending slash, SharePoint removes it everywhere
            if (result.Value.Length > 1)
                result.Value = result.Value.TrimEnd('/');

            if (OnTokenReplaced != null)
            {
                OnTokenReplaced(this, new TokenReplacementResultEventArgs
                {
                    Result = result
                });
            }

            return result;
        }

        private string ResolveToken(TokenReplacementContext tokenContext, object contextObject, string token)
        {
            if (string.Equals(token, "~sitecollection", StringComparison.CurrentCultureIgnoreCase))
            {
                if (tokenContext.IsSiteRelativeUrl)
                    return "/";

                var site = ExtractSite(contextObject);

                //if (site.ServerRelativeUrl == "/")
                //    return string.Empty;

                // Incorrect ~site/~sitecollection tokens resolve in NavigationNodes #1025
                // https://github.com/SubPointSolutions/spmeta2/issues/1025
                // always return '/' instead of empty string, further replacements would fix up double-'/'

                return site.ServerRelativeUrl;
            }

            if (string.Equals(token, "~site", StringComparison.CurrentCultureIgnoreCase))
            {
                var web = ExtractWeb(contextObject);

                if (tokenContext.IsSiteRelativeUrl)
                {
                    return "/" + web.ServerRelativeUrl.Replace(web.Site.ServerRelativeUrl, string.Empty);
                }

                //if (web.ServerRelativeUrl == "/")
                //    return string.Empty;

                // Incorrect ~site/~sitecollection tokens resolve in NavigationNodes #1025
                // https://github.com/SubPointSolutions/spmeta2/issues/1025
                // always return '/' instead of empty string, further replacements would fix up double-'/'

                return web.ServerRelativeUrl;
            }

            return token;
        }

        private SPWeb ExtractWeb(object contextObject)
        {
            if (contextObject is SPSite)
                return (contextObject as SPSite).RootWeb;

            if (contextObject is SPWeb)
                return (contextObject as SPWeb);

            if (contextObject is SPList)
                return (contextObject as SPList).ParentWeb;

            throw new SPMeta2NotSupportedException(string.Format("contextObject of type: [{0}] is not supported", contextObject.GetType()));
        }

        private SPSite ExtractSite(object contextObject)
        {
            if (contextObject is SPSite)
                return contextObject as SPSite;

            if (contextObject is SPWeb)
                return (contextObject as SPWeb).Site;

            if (contextObject is SPList)
                return (contextObject as SPList).ParentWeb.Site;

            throw new SPMeta2NotSupportedException(string.Format("contextObject of type: [{0}] is not supported", contextObject.GetType()));
        }

        #endregion
    }
}
