using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Capabilities;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using SPMeta2.Common;
using SPMeta2.Definitions;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;
using SPMeta2.Standard.Definitions;
using SPMeta2.Regression.Tests.Config;
using SPMeta2.Services;
using SPMeta2.Containers.Utils;
using SPMeta2.Standard.Definitions.Webparts;
using SPMeta2.Regression.Utils;
using SPMeta2.Containers.Services.Rnd;
using SPMeta2.Containers.Services;

using SPMeta2.Extensions;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class ModelNodesTests
    {
        #region constructors

        public ModelNodesTests()
        {
            Rnd = new DefaultRandomService();
        }

        #endregion

        #region properties

        public RandomService Rnd { get; set; }

        #endregion

        #region tests

        [TestMethod]
        [TestCategory("CI.Core")]
        [TestCategory("Regression.ModelNodes.ExtensionMethods")]
        public void Can_Use_ModelNode_SetPropertyBagValue()
        {
            var modelNode = new ModelNode();

            var name = Rnd.String();
            var value = Rnd.String();

            modelNode.SetPropertyBagValue(name, value);

            Assert.AreEqual(value, modelNode.GetPropertyBagValue(name));
            Assert.AreEqual(value, modelNode.PropertyBag.FirstOrDefault(p => p.Name == name).Value);
        }

        [TestMethod]
        [TestCategory("CI.Core")]
        [TestCategory("Regression.ModelNodes.ExtensionMethods")]
        public void Can_Use_ModelNode_SetNonPersistentPropertyBagValue()
        {
            var modelNode = new ModelNode();

            var name = Rnd.String();
            var value = Rnd.String();

            modelNode.SetNonPersistentPropertyBagValue(name, value);

            Assert.AreEqual(value, modelNode.GetNonPersistentPropertyBagValue(name));
            Assert.AreEqual(value, modelNode.NonPersistentPropertyBag.FirstOrDefault(p => p.Name == name).Value);
        }

        #endregion

        #region compatibility

        [TestMethod]
        [TestCategory("Regression.ModelNodes.Compatibility")]
        [TestCategory("CI.Core")]
        public void Should_Pass_On_Valid_SSOM_CSOM()
        {
            var validDefinitions = new DefinitionBase[]{ 
                new FieldDefinition(),
                new WebDefinition(),
                new ListDefinition(),
            };

            // both CSOM / SSOM
            foreach (var def in validDefinitions)
            {
                Assert.IsTrue(def.IsCSOMCompatible());
                Assert.IsTrue(def.IsSSOMCompatible());
            }
        }

        [TestMethod]
        [TestCategory("Regression.ModelNodes.Compatibility")]
        [TestCategory("CI.Core")]
        public void Should_Pass_On_Valid_SSOM_Invalid_CSOM()
        {
            var validDefinitions = new DefinitionBase[]{ 
                new FarmDefinition(),
                new WebApplicationDefinition(),
                new SiteDefinition()
            };

            // both CSOM / SSOM
            foreach (var def in validDefinitions)
            {
                Assert.IsFalse(def.IsCSOMCompatible());
                Assert.IsTrue(def.IsSSOMCompatible());
            }
        }

        #endregion
    }
}
