using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace SPMeta2.CSOM.ModelHandlers
{
    public class AppPrincipalModelHandler : CSOMModelHandlerBase
    {
        #region properties

        public override Type TargetType
        {
            get { return typeof(AppPrincipal); }
        }

        #endregion

        #region methods

        #endregion
    }
}
