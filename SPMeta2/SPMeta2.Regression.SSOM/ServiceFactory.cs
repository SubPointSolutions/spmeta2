using SPMeta2.Containers.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.SSOM
{
    public static class ServiceFactory
    {
        #region static

        static ServiceFactory()
        {

        }

        #endregion

        #region properties

        public static RegressionAssertService AssertService = new RegressionAssertService();

        #endregion
    }
}
