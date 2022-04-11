using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers;
using SPMeta2.Definitions;
using SPMeta2.Services.Impl;
using SPMeta2.Standard.Definitions.Taxonomy;
using SPMeta2.Standard.Syntax;
using SPMeta2.Syntax.Default;
using SPMeta2.Definitions.ContentTypes;
using SPMeta2.Standard.Definitions;
using SPMeta2.Syntax.Default.Utils;
using SPMeta2.Enumerations;

namespace SPMeta2.Regression.Tests.Impl.Syntax
{
    [TestClass]
    public class ContentTypeDefinitionSyntaxTests
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Syntax.ContentTypeDefinitionSyntax")]
        [TestCategory("CI.Core")]
        public void ContentTypeDefinitionSyntax_IsChildOf()
        {
            // Potential bug with ContentTypeDefinitionSyntax.IsChildOf method #940
            // https://github.com/SubPointSolutions/spmeta2/issues/940 

            var parentContentType = new ContentTypeDefinition
            {
                IdNumberValue = BuiltInContentTypeId.Document
            };

            var childContentType = new ContentTypeDefinition
            {
                Id = Guid.NewGuid(),
                ParentContentTypeId = BuiltInContentTypeId.Document
            };

            PerformIsChildOfTests(childContentType, parentContentType);
        }

        private void PerformIsChildOfTests(ContentTypeDefinition childContentType, ContentTypeDefinition parentContentType)
        {
            // by ct
            Assert.IsTrue(ContentTypeDefinitionSyntax.IsChildOf(childContentType, parentContentType));
            Assert.IsFalse(ContentTypeDefinitionSyntax.IsChildOf(parentContentType, childContentType));

            var parentId = parentContentType.GetContentTypeId();
            var childId = childContentType.GetContentTypeId();

            // by strings
            Assert.IsTrue(ContentTypeDefinitionSyntax.IsChildOf(childId, parentId));
            Assert.IsFalse(ContentTypeDefinitionSyntax.IsChildOf(parentId, childId));

            //  upper-lower case strings
            var parentIdAsLower = string.Join(string.Empty, parentId.Select(s => char.ToLower(s)));
            var parentIdAsUpper = string.Join(string.Empty, parentId.Select(s => char.ToUpper(s)));

            var childIdAsLower = string.Join(string.Empty, childId.Select(s => char.ToLower(s)));
            var childIdAsUpper = string.Join(string.Empty, childId.Select(s => char.ToUpper(s)));

            Assert.IsTrue(ContentTypeDefinitionSyntax.IsChildOf(childIdAsLower, parentIdAsLower));
            Assert.IsFalse(ContentTypeDefinitionSyntax.IsChildOf(parentIdAsUpper, childIdAsLower));

            Assert.IsTrue(ContentTypeDefinitionSyntax.IsChildOf(childIdAsUpper, parentIdAsUpper));
            Assert.IsFalse(ContentTypeDefinitionSyntax.IsChildOf(parentIdAsUpper, childIdAsUpper));
        }

        #endregion
    }
}
