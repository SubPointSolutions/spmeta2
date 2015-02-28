using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Services
{
    public class TokenReplacementContext
    {
        #region properties

        public object Context { get; set; }
        public string Value { get; set; }

        #endregion
    }

    public class TokenReplacementResult
    {
        #region properties

        public string Value { get; set; }

        #endregion
    }

    public class TokenInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public abstract class TokenReplacementServiceBase
    {
        #region constructors

        public TokenReplacementServiceBase()
        {

        }

        #endregion

        #region properties

        protected List<TokenInfo> _supportedTokens = new List<TokenInfo>();
        public IEnumerable<TokenInfo> SupportedTokens { get { return _supportedTokens; } }


        #endregion

        #region methods

        public abstract TokenReplacementResult ReplaceTokens(TokenReplacementContext context);

        #endregion
    }
}
