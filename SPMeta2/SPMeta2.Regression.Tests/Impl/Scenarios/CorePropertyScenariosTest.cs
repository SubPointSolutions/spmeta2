using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Containers;
using SPMeta2.Containers.Standard;
using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Regression.Tests.Definitions;
using SPMeta2.Regression.Tests.Impl.Scenarios.Base;
using SPMeta2.Syntax.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default.Modern;
using SPMeta2.Standard.Definitions;
using SPMeta2.Standard.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Scenarios
{
    [TestClass]
    public class CorePropertyScenariosTest : SPMeta2RegresionScenarioTestBase
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

        #region properties

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.Properties")]
        public void CanDeploy_CoreProperty_As_IsAlias()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.IsAlias = Rnd.Bool();
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);

                // revert prop. deploy again
                var invertedPropDef = propDef.Inherit(def =>
                {
                    def.IsAlias = !def.IsAlias;
                });

                var invertedModel = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(invertedPropDef);
                });


                TestModel(invertedModel);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.Properties")]
        public void CanDeploy_CoreProperty_As_IsMultivalued()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.StringMultiValue;
                    def.IsMultivalued = Rnd.Bool();
                    def.Length = 1 + Rnd.Int(100);
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });

                TestModel(model);

                // revert prop. deploy again
                // change name cause we need new model as IsMultivalued prop cannot be updated
                var invertedPropDef = propDef.Inherit(def =>
                {
                    def.Name = Rnd.String();
                    def.IsMultivalued = !def.IsMultivalued;
                });

                var invertedModel = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(invertedPropDef);
                });


                TestModel(invertedModel);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.Properties")]
        public void CanDeploy_CoreProperty_As_IsSearchable()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.String;
                    def.IsSearchable = Rnd.Bool();
                    def.Length = 1 + Rnd.Int(10);
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);

                // revert prop. deploy again
                var invertedPropDef = propDef.Inherit(def =>
                {
                    def.IsSearchable = !def.IsSearchable;
                });

                var invertedModel = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(invertedPropDef);
                });


                TestModel(invertedModel);
            });
        }

        #endregion

        #region data types

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_BigInteger()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.BigInteger;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_Binary()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.Binary;
                    def.Length = 1 + Rnd.Int(10);
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_Boolean()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.Boolean;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_Date()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.Date;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_DateNoYear()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.DateNoYear;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_DateTime()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.DateTime;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_Email()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.Email;
                    def.Length = 1 + Rnd.Int(10);
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_Float()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.Float;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_Guid()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.Guid;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_HTML()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.HTML;
                    def.Length = 1 + Rnd.Int(10);
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_Integer()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.Integer;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_Person()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.Person;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_String()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.String;
                    def.Length = 1 + Rnd.Int(10);
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_StringMultiValue()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.StringMultiValue;
                    def.Length = 1 + Rnd.Int(10);
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }


        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_StringSingleValue()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.StringSingleValue;
                    def.Length = 1 + Rnd.Int(10);
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_TimeZone()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.TimeZone;
                    def.Length = null;
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        [TestMethod]
        [TestCategory("Regression.Scenarios.CoreProperty.DataType")]
        public void CanDeploy_CoreProperty_As_URL()
        {
            WithExpectedUnsupportedCSOMnO365RunnerExceptions(() =>
            {
                var propDef = ModelGeneratorService.GetRandomDefinition<CorePropertyDefinition>(def =>
                {
                    def.Type = BuiltInPropertyDataType.URL;
                    def.Length = 1 + Rnd.Int(10);
                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddCoreProperty(propDef);
                });


                TestModel(model);
            });
        }

        #endregion
    }
}
