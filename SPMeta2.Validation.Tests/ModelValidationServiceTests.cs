using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Syntax.Default;
using SPMeta2.Validation.Services;

namespace SPMeta2.Validation.Tests
{
    [TestClass]
    public class ModelValidationServiceTests
    {
        [TestMethod]
        public void CanFindDuplicateFolders()
        {
            var validationService = new ModelValidationService();

            var model = SPMeta2Model
                .NewModel()
                .DummyWeb()
                .AddFolder(new FolderDefinition { Name = "sdfs" })
                .AddFolder(new FolderDefinition { Name = "sdfs " });


            validationService.DeployModel(null, model);

            foreach (var v in validationService.Result)
            {
                Trace.WriteLine(v.Message);
            }
        }
    }
}
