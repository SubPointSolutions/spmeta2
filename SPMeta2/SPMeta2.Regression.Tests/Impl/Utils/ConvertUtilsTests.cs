using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Tests.Impl.Utils
{
    [TestClass]
    public class ConvertUtilsTests
    {
        #region ToBool tests

        [TestMethod]
        [TestCategory("Regression.Utils.ConvertUtils")]
        [TestCategory("CI.Core")]
        public void ConvertUtils_ToBool_AsTrue()
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
        public void ConvertUtils_ToBool_AsFalse()
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
        public void ConvertUtils_ToBool_AsNull()
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

        #region ToInt tests

        [TestMethod]
        [TestCategory("Regression.Utils.ConvertUtils")]
        [TestCategory("CI.Core")]
        public void ConvertUtils_ToInt_AsInt()
        {
            Assert.AreEqual(1, ConvertUtils.ToInt(1));
            Assert.AreEqual(0, ConvertUtils.ToInt(0));
            Assert.AreEqual(-1, ConvertUtils.ToInt(-1));

            Assert.AreEqual(1, ConvertUtils.ToInt((object)1));
            Assert.AreEqual(0, ConvertUtils.ToInt((object)0));
            Assert.AreEqual(-1, ConvertUtils.ToInt((object)-1));

            Assert.AreEqual(1, ConvertUtils.ToInt("1"));
            Assert.AreEqual(0, ConvertUtils.ToInt("0"));
            Assert.AreEqual(-1, ConvertUtils.ToInt("-1"));

            Assert.AreEqual(1, ConvertUtils.ToInt((object)"1"));
            Assert.AreEqual(0, ConvertUtils.ToInt((object)"0"));
            Assert.AreEqual(-1, ConvertUtils.ToInt((object)"-1"));
        }

        [TestMethod]
        [TestCategory("Regression.Utils.ConvertUtils")]
        [TestCategory("CI.Core")]
        public void ConvertUtils_ToInt_AsNull()
        {
            Assert.IsFalse(ConvertUtils.ToInt(null).HasValue);

            Assert.IsFalse(ConvertUtils.ToInt("tmp").HasValue);
            Assert.IsFalse(ConvertUtils.ToInt((object)"tmp").HasValue);
        }

        #endregion

        #region ToGuid tests

        [TestMethod]
        [TestCategory("Regression.Utils.ConvertUtils")]
        [TestCategory("CI.Core")]
        public void ConvertUtils_ToGuid_AsGuid()
        {
            var g1 = Guid.NewGuid();
            var g2 = Guid.NewGuid();
            var g3 = Guid.NewGuid();

            Assert.AreEqual(g1, ConvertUtils.ToGuid(g1));
            Assert.AreEqual(g2, ConvertUtils.ToGuid(g2));
            Assert.AreEqual(g3, ConvertUtils.ToGuid(g3));

            Assert.AreEqual(g1, ConvertUtils.ToGuid((object)g1));
            Assert.AreEqual(g2, ConvertUtils.ToGuid((object)g2));
            Assert.AreEqual(g3, ConvertUtils.ToGuid((object)g3));

            Assert.AreEqual(g1, ConvertUtils.ToGuid(g1.ToString()));
            Assert.AreEqual(g2, ConvertUtils.ToGuid(g2.ToString()));
            Assert.AreEqual(g3, ConvertUtils.ToGuid(g3.ToString()));

            Assert.AreEqual(g1, ConvertUtils.ToGuid((object)g1.ToString()));
            Assert.AreEqual(g2, ConvertUtils.ToGuid((object)g2.ToString()));
            Assert.AreEqual(g3, ConvertUtils.ToGuid((object)g3.ToString()));
        }

        [TestMethod]
        [TestCategory("Regression.Utils.ConvertUtils")]
        [TestCategory("CI.Core")]
        public void ConvertUtils_ToGuid_AsNull()
        {
            Assert.IsFalse(ConvertUtils.ToGuid(null).HasValue);

            Assert.IsFalse(ConvertUtils.ToGuid("tmp").HasValue);
            Assert.IsFalse(ConvertUtils.ToGuid((object)"tmp").HasValue);
        }

        #endregion
    }
}
