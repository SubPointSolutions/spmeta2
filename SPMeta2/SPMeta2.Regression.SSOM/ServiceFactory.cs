using SPMeta2.Validation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
