using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Misc
{
    [TestClass]
    public class BuiltInWebpartTemplatesTest
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Misc")]
        public void CanCreate_BuiltInWebpartTemplates()
        {
            var tmp = BuiltInWebpartTemplates.XsltListViewWebPart;
        }

        #endregion
    }
}
