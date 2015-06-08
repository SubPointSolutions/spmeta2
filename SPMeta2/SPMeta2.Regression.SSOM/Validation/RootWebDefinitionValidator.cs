using System;
using System.Linq.Expressions;
using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Assertion;
using SPMeta2.Definitions;
using SPMeta2.Definitions.Base;

using SPMeta2.SSOM.ModelHandlers;
using SPMeta2.SSOM.ModelHosts;
using SPMeta2.Utils;


namespace SPMeta2.Regression.SSOM.Validation
{
    public class RootWebDefinitionValidator : RootWebModelHandler
    {
        public override void DeployModel(object modelHost, DefinitionBase model)
        {
            var definition = model.WithAssertAndCast<RootWebDefinition>("model", value => value.RequireNotNull());

            var spObject = GetCurrentObject(modelHost, definition);

            var assert = ServiceFactory.AssertService
                                        .NewAssert(definition, spObject)
                                        .ShouldNotBeNull(spObject);

            if (string.IsNullOrEmpty(definition.Title))
                assert.SkipProperty(m => m.Title, "Title is null or empty");
            else
                assert.ShouldBeEqual(m => m.Title, o => o.Title);

            if (string.IsNullOrEmpty(definition.Description))
                assert.SkipProperty(m => m.Description, "Description is null or empty");
            else
                assert.ShouldBeEqual(m => m.Description, o => o.Description);

            assert.ShouldBeEqual((p, s, d) =>
            {
                var isValid = d.IsRootWeb;

                return new PropertyValidationResult
                {
                    Tag = p.Tag,
                    Src = null,
                    Dst = null,
                    IsValid = isValid,
                    Message = "Checking if IsRootWeb == TRUE"
                };
            });
        }
    }

}
