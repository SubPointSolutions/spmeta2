using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers;
using SPMeta2.Containers.DefinitionGenerators.Fields;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Enumerations;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Services;
using SPMeta2.Exceptions;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class BooleanFieldDefinitionTests : SPMeta2ProvisionRegresionTestBase
    {
        #region common

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
        [TestCategory("Regression.Definitions.BooleanFieldDefinition")]
        public void BooleanFieldDefinition_ShouldCheck_DefaultValue()
        {
            // Enhance BooleanFieldDefinition - add DefaultValue property validation #792
            // When creating a BooleanFieldDefinition you have to use "0" and "1" for DefaultValue 
            // else it won't work. Neither "false" nor false.ToString() work, 
            // so there should be an BuiltIn option for this.
            var checksTrueResult = false;
            var checksFalseResult = false;

            var checks1Result = false;
            var checks0Result = false;

            var invalidData = new Dictionary<string, bool>();

            invalidData.Add("true", false);
            invalidData.Add("false", false);
            invalidData.Add("-1", false);
            invalidData.Add("-2", false);

            var validData = new Dictionary<string, bool>();

            validData.Add("1", false);
            validData.Add("0", false);

            // expected exception
            foreach (var data in invalidData.Keys.ToArray())
            {
                var booleanFieldDef = ModelGeneratorService.GetRandomDefinition<BooleanFieldDefinition>(def =>
                {

                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddField(booleanFieldDef);
                });

                var hasException = false;

                try
                {
                    booleanFieldDef.DefaultValue = data;
                    TestModel(model);
                }
                catch (Exception ex)
                {
                    hasException = true;
                    invalidData[data] = IsValidException(ex);
                }

                if (!hasException)
                    Assert.Fail(string.Format("Fail validating value:[{0}]", data));
            }

            // no exception
            foreach (var data in validData.Keys.ToArray())
            {
                var booleanFieldDef = ModelGeneratorService.GetRandomDefinition<BooleanFieldDefinition>(def =>
                {

                });

                var model = SPMeta2Model.NewSiteModel(site =>
                {
                    site.AddField(booleanFieldDef);
                });

                var hasException = false;

                try
                {
                    booleanFieldDef.DefaultValue = data;
                    TestModel(model);
                }
                catch (Exception ex)
                {
                    hasException = true;
                }

                if (hasException)
                    Assert.Fail(string.Format("Fail validating value:[{0}]", data));

                validData[data] = true;
            }

            // sum up
            Assert.IsTrue(validData.Values.All(v => v == true)
                          && invalidData.Values.All(v => v == true));
        }

        protected bool IsValidException(Exception ex)
        {
            return ex is SPMeta2ModelDeploymentException
               && (ex.InnerException as AggregateException).InnerException is SPMeta2ModelValidationException;
        }
        #endregion
    }


}
