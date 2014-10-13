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
        #region constructors



        #endregion

        #region properties

        public string GlobalNavigationSource { get; set; }
        public Guid? GlobalNavigationTermStoreId { get; set; }
        public Guid? GlobalNavigationTermSetId { get; set; }

        public string CurrentNavigationSource { get; set; }
        public Guid? CurrentNavigationTermStoreId { get; set; }
        public Guid? CurrentNavigationTermSetId { get; set; }

        public bool? ResetToDefaults { get; set; }

        public bool? GlobalNavigationShowPages { get; set; }
        public bool? GlobalNavigationShowSubsites { get; set; }

        public int? GlobalNavigationMaximumNumberOfDynamicItems { get; set; }

        public bool? CurrentNavigationShowPages { get; set; }
        public bool? CurrentNavigationShowSubsites { get; set; }

        public int? CurrentNavigationMaximumNumberOfDynamicItems { get; set; }


        #endregion
    }
}
