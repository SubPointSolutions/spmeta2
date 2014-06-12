using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Services;

namespace SPMeta2.Validation.Tests
{
    //[TestClass]
    public class ModelValidationServiceTests
    {
        [TestMethod]
        public void CanFindDuplicateFolders()
        {
            var validationService = new ModelValidationService();

            var model = SPMeta2Model
                .NewModel()
                .DummyWeb()
                .AddFolder(new FolderDefinition { Name = "test folder 1" })
                .AddFolder(new FolderDefinition { Name = "test folder 1" })
                .AddFolder(new FolderDefinition { Name = " folder with spaces " }); ;

            validationService.DeployModel(null, model);

            var validationResult = validationService.Result;

            foreach (var result in validationResult)
                Trace.WriteLine(result.Message);
        }

        [TestMethod]
        public void CanFindDuplicateFieldIds()
        {
            var validationService = new ModelValidationService();
            var fieldId = Guid.NewGuid();

            var model = SPMeta2Model
                .NewModel()
                .DummyWeb()
                .AddField(new FieldDefinition
                {
                    Id = fieldId
                })
               .AddField(new FieldDefinition
                {
                    Id = fieldId
                });

            validationService.DeployModel(null, model);

            var validationResult = validationService.Result;

            foreach (var result in validationResult)
                Trace.WriteLine(result.Message);
        }
    }
}
