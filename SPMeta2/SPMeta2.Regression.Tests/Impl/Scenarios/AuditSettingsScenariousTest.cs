using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class AuditSettingsScenariousTest : SPMeta2RegresionScenarioTestBase
    {
        #region internal

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            InternalInit();
        }

        [ClassCleanupAttribute]
        public static void Cleanup()
        {
            InternalCleanup();
        }

        #endregion

        #region site and list

        [TestMethod]
        [TestCategory("Regression.Scenarios.AuditSettings")]
        public void CanDeploy_AuditSettings_ToSite()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var model = SPMeta2Model
                    .NewSiteModel(site =>
                    {
                        site.AddRandomAuditSetting();
                    });

                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.AuditSettings")]
        public void CanDeploy_AuditSettings_ToWeb()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var siteModel = SPMeta2Model
                    .NewSiteModel(site =>
                    {
                        site.AddRandomAuditSetting();
                    });

                var webModel = SPMeta2Model
                    .NewWebModel(web =>
                    {
                        web.AddRandomAuditSetting();
                    });

                TestModels(new[] { siteModel, webModel });
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.AuditSettings")]
        public void CanDeploy_AuditSettings_ToList()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var siteModel = SPMeta2Model
                    .NewSiteModel(site =>
                    {
                        site.AddRandomAuditSetting();
                    });

                var listAuditSetting = new AuditSettingsDefinition();

                if (Rnd.Bool())
                {
                    listAuditSetting.AuditFlags = new Collection<string>
                {
                    BuiltInAuditMaskType.CheckIn,
                    BuiltInAuditMaskType.CheckOut,
                    BuiltInAuditMaskType.Copy,
                    BuiltInAuditMaskType.Move
                };
                }
                else
                {
                    listAuditSetting.AuditFlags = new Collection<string>
                {
                    BuiltInAuditMaskType.CheckIn,
                    BuiltInAuditMaskType.Copy,
                };
                }

                var webModel = SPMeta2Model
                    .NewWebModel(web =>
                    {
                        web.AddRandomList(list =>
                        {
                            list.AddAuditSettings(listAuditSetting);
                        });
                    });

                TestModels(new[] { siteModel, webModel });

            });
        }

        #endregion
    }
}
