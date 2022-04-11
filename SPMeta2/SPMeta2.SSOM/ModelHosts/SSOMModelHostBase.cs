﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.ModelHosts;

namespace SPMeta2.SSOM.ModelHosts
{
    public class SSOMModelHostBase : ModelHostBase
    {
         #region constructors

        public SSOMModelHostBase()
        {
            IsCSOM = false;
            IsSSOM = true;
        }

        #endregion
    }
}
