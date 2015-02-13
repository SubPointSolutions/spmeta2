using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.SSOM.ModelHosts
{
    public class FarmModelHost : SSOMModelHostBase
    {
        #region constructors

        public FarmModelHost()
        {

        }

        public FarmModelHost(SPFarm farm)
        {
            HostFarm = farm;
        }

        #endregion

        #region properties

        public SPFarm HostFarm { get; set; }

        #endregion

        #region static

        public static FarmModelHost FromFarm(SPFarm farm)
        {
            return new FarmModelHost(farm);
        }

        #endregion
    }
}
