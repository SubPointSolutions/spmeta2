using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPMeta2.Models
{
    public abstract class WebModel : ModelBase
    {
        #region constructors

        #endregion

        #region properties

        #endregion


        #region methods

        public abstract ModelNode GetSiteModel();
        public abstract ModelNode GetRootWebModel();
        public abstract ModelNode GetWebModel();

        #endregion
    }
}
