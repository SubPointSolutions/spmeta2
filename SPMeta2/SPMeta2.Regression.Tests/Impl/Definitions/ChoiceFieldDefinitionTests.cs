using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Regression;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Fields;
using SPMeta2.Models;
using SPMeta2.Standard.Definitions.Fields;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Syntax.Default;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class ChoiceFieldDefinitionTests
    {
        #region properties

        [TestMethod]
        [TestCategory("Regression.Definitions.ChoiceFieldDefinition")]
        public void ChoiceFieldDefinition_ShoudHave_NotNollChoicesProperty()
        {
            var def = new ChoiceFieldDefinition();

            Assert.IsNotNull(def.Choices);
        }

        #endregion
    }
}
