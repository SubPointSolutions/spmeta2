using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Misc
{
    [TestClass]
    public class BuiltInWebpartTemplatesTest
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Misc")]
        [TestCategory("CI.Core")]
        public void CanCreate_BuiltInWebpartTemplates()
        {
            var tmp = BuiltInWebPartTemplates.XsltListViewWebPart;
        }

        #endregion
    }
}
