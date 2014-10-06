using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;

namespace SPMeta2.CSOM.Standard.Definitions
{
    public class WebNavigationSettingsDefinition : DefinitionBase
    {
        #region properties

        public string GlobalNavigationSource { get; set; }
        public Guid? GlobalNavigationTermStoreId { get; set; }
        public Guid? GlobalNavigationTermSetId { get; set; }

        public string CurrentNavigationSource { get; set; }
        public Guid? CurrentNavigationTermStoreId { get; set; }
        public Guid? CurrentNavigationTermSetId { get; set; }

        public bool? ResetToDefaults { get; set; }

        #endregion
    }
}
