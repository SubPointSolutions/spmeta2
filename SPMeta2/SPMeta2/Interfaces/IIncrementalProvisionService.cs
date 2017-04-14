using SPMeta2.Common;
using SPMeta2.ModelHosts;
using SPMeta2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Interfaces
{
    public interface IIncrementalProvisionService : IProvisionService
    {
        #region properties

        ModelHash PreviousModelHash { get; set; }
        ModelHash CurrentModelHash { get; set; }

        #endregion
    }
}
