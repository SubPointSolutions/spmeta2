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

namespace SPMeta2.Regression.Tests.Impl.Definitions
{
    [TestClass]
    public class ImageFieldDefinitionTests : SPMeta2DefinitionRegresionTestBase
    {
        #region properties

        [TestMethod]
        [TestCategory("Regression.Definitions.ImageFieldDefinitions")]
        [TestCategory("CI.Core")]
        public void ImageFieldDefinition_ShouldHave_RichTextMode_And_RichText_Attrs()
        {
            var def = new ImageFieldDefinition();

            // RichTextMode/RichText should be set as follow to make sure field can be edited and displayed correctly
            // Skipping these atts would resul a pure HTML string on the publishing page layout

            // Enhance 'ImageFieldDefinition' - add default AdditionalAttributes #552
            // https://github.com/SubPointSolutions/spmeta2/issues/552

            Assert.IsTrue(def.AdditionalAttributes.Any(
                a => a.Name == "RichTextMode" && a.Value == "FullHtml"));

            Assert.IsTrue(def.AdditionalAttributes.Any(
                a => a.Name == "RichText" && a.Value == "TRUE"));
        }

        #endregion
    }
}
