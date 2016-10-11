using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Tests.Impl.Utils
{
    [TestClass]
    public class ConvertUtilsTests
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Utils.ConvertUtils")]
        [TestCategory("CI.Core")]
        public void ConvertUtils_CanParseBool_AsTrue()
        {
            Assert.IsTrue(ConvertUtils.ToBool(1).Value);
            Assert.IsTrue(ConvertUtils.ToBool((object)1).Value);

            Assert.IsTrue(ConvertUtils.ToBool("1").Value);
            Assert.IsTrue(ConvertUtils.ToBool((object)"1").Value);

            Assert.IsTrue(ConvertUtils.ToBool("true").Value);
            Assert.IsTrue(ConvertUtils.ToBool("True").Value);
            Assert.IsTrue(ConvertUtils.ToBool("TRUE").Value);
            Assert.IsTrue(ConvertUtils.ToBool((object)"true").Value);
            Assert.IsTrue(ConvertUtils.ToBool((object)"True").Value);
            Assert.IsTrue(ConvertUtils.ToBool((object)"TRUE").Value);


            Assert.IsTrue(ConvertUtils.ToBool(true).Value);
            Assert.IsTrue(ConvertUtils.ToBool((object)true).Value);
        }

        [TestMethod]
        [TestCategory("Regression.Utils.ConvertUtils")]
        [TestCategory("CI.Core")]
        public void ConvertUtils_CanParseBool_AsFalse()
        {
            Assert.IsFalse(ConvertUtils.ToBool(0).Value);
            Assert.IsFalse(ConvertUtils.ToBool((object)0).Value);

            Assert.IsFalse(ConvertUtils.ToBool("0").Value);
            Assert.IsFalse(ConvertUtils.ToBool((object)"0").Value);

            Assert.IsFalse(ConvertUtils.ToBool("false").Value);
            Assert.IsFalse(ConvertUtils.ToBool("False").Value);
            Assert.IsFalse(ConvertUtils.ToBool("FALSE").Value);
            Assert.IsFalse(ConvertUtils.ToBool((object)"false").Value);
            Assert.IsFalse(ConvertUtils.ToBool((object)"False").Value);
            Assert.IsFalse(ConvertUtils.ToBool((object)"FALSE").Value);

            Assert.IsFalse(ConvertUtils.ToBool(false).Value);
            Assert.IsFalse(ConvertUtils.ToBool((object)false).Value);
        }

        [TestMethod]
        [TestCategory("Regression.Utils.ConvertUtils")]
        [TestCategory("CI.Core")]
        public void ConvertUtils_CanParseBool_AsNull()
        {
            Assert.IsFalse(ConvertUtils.ToBool(2).HasValue);
            Assert.IsFalse(ConvertUtils.ToBool((object)2).HasValue);

            Assert.IsFalse(ConvertUtils.ToBool("2").HasValue);
            Assert.IsFalse(ConvertUtils.ToBool((object)"2").HasValue);

            Assert.IsFalse(ConvertUtils.ToBool(-1).HasValue);
            Assert.IsFalse(ConvertUtils.ToBool((object)-1).HasValue);

            Assert.IsFalse(ConvertUtils.ToBool("-1").HasValue);
            Assert.IsFalse(ConvertUtils.ToBool((object)"-1").HasValue);

            Assert.IsFalse(ConvertUtils.ToBool("truee").HasValue);
            Assert.IsFalse(ConvertUtils.ToBool("Truee").HasValue);
            Assert.IsFalse(ConvertUtils.ToBool("TRUEE").HasValue);
            Assert.IsFalse(ConvertUtils.ToBool((object)"trueE").HasValue);
            Assert.IsFalse(ConvertUtils.ToBool((object)"TrueE").HasValue);
            Assert.IsFalse(ConvertUtils.ToBool((object)"TRUEE").HasValue);
        }

        #endregion
    }
}
