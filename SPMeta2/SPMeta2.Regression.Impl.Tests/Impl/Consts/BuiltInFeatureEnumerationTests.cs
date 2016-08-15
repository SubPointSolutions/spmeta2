using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Impl.Tests.Impl.Consts
{
    [TestClass]
    public class BuiltInFeatureEnumerationTests
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Impl.BuiltInFeatureEnumeration")]
        [TestCategory("CI.Core")]
        public void Can_Create_BuiltInFeatureEnumerations()
        {
            // checks really silly typos in GUIDs
            // static classes, so they raise an exception while acessing

            var farmFeature = BuiltInFarmFeatures.ExcelServicesApplicationViewFarmFeature;
            var webAppFeature = BuiltInWebApplicationFeatures.AppsThatRequireAccessibleInternetFacingEndpoints;
            var siteFeature = BuiltInSiteFeatures.BasicWebParts;
            var webFeature = BuiltInWebFeatures.AccessApp;
        }

        #endregion
    }
}
