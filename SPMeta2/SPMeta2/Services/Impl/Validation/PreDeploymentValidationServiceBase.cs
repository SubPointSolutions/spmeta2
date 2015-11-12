using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Services.Impl.Validation
{
    public abstract class PreDeploymentValidationServiceBase : PreDeploymentServiceBase
    {
        #region properties
        public string Title { get; protected set; }
        public string Description { get; protected set; }
        #endregion

        #region methods

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Title))
                return Title;

            return base.ToString();
        }

        #endregion
    }
}
