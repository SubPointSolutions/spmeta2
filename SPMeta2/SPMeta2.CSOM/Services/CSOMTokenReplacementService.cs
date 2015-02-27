using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SharePoint.Client;
using SPMeta2.Exceptions;
using SPMeta2.Services;

namespace SPMeta2.CSOM.Services
{
    public class CSOMTokenReplacementService : TokenReplacementServiceBase
    {
        #region constructors

        public CSOMTokenReplacementService()
        {
            _supportedTokens.Clear();

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

            _supportedTokens.AddRange(TokenProcessInfos.Select(i => new TokenInfo { Name = i.Name }));
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
                    result.Value = tokenInfo.RegEx.Replace(result.Value, ResolveToken(context.Context, tokenInfo.Name));
            }

            return result;
        }

        private string ResolveToken(object contextObject, string token)
        {
            if (string.Equals(token, "~sitecollection", StringComparison.CurrentCultureIgnoreCase))
            {
                var site = ExtractSite(contextObject);

                if (site.ServerRelativeUrl == "/")
                    return string.Empty;

                return site.ServerRelativeUrl;
            }

            if (string.Equals(token, "~site", StringComparison.CurrentCultureIgnoreCase))
            {
                var web = ExtractWeb(contextObject);

                if (web.ServerRelativeUrl == "/")
                    return string.Empty;

                return web.ServerRelativeUrl;
            }

            return token;
        }

        private Web ExtractWeb(object contextObject)
        {
            if (contextObject is ClientContext)
                return (contextObject as ClientContext).Web;

            throw new SPMeta2NotSupportedException(string.Format("contextObject of type: [{0}] is not supported", contextObject.GetType()));
        }

        private Site ExtractSite(object contextObject)
        {
            if (contextObject is ClientContext)
                return (contextObject as ClientContext).Site;

            throw new SPMeta2NotSupportedException(string.Format("contextObject of type: [{0}] is not supported", contextObject.GetType()));
        }

        #endregion
    }
}
