using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Standard.Definitions
{
    public class SearchResultDefinition
    {
        #region properties

        public string QueryString { get; set; }

        public string ProviderName { get; set; }
        public Guid? ProviderId { get; set; }

        #endregion
    }
}
