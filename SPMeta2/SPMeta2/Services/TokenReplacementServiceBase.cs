using System;
using System.Collections.Generic;

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

        #region classes

        public class TokenReplacementResultEventArgs : EventArgs
        {
            public TokenReplacementResult Result { get; set; }
        }

        #endregion

        #region events

        public EventHandler<TokenReplacementResultEventArgs> OnTokenReplaced;

        #endregion

        #region properties

        protected readonly List<TokenInfo> SupportedTokensInternal = new List<TokenInfo>();
        public IEnumerable<TokenInfo> SupportedTokens { get { return SupportedTokensInternal; } }


        #endregion

        #region methods

        public abstract TokenReplacementResult ReplaceTokens(TokenReplacementContext context);

        #endregion
    }
}
