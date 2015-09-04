using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.BuiltInDefinitions;
using SPMeta2.Definitions;
using SPMeta2.Extensions;
using SPMeta2.Regression.Tests.Base;
using SPMeta2.Syntax.Default;

namespace SPMeta2.Regression.Tests.Impl.DefinitionCoverage
{
    [TestClass]
    public class BuiltInListDefinitionsCoverage : SPMeta2RegresionTestBase
    {
        [TestMethod]
        [TestCategory("Regression.DefinitionCoverage.BuiltInListDefinitions")]
        public void CanDeploy_All_BuiltInListDefinitions()
        {
            var webModel = SPMeta2Model.NewWebModel(web =>
            {
                web.AddDefinitionsFromStaticClassType(typeof(BuiltInListDefinitions));
                web.AddDefinitionsFromStaticClassType(typeof(BuiltInListDefinitions.Catalogs));

                var clonedDefs = web.ChildModels
                                    .Select(s => (s.Value as ListDefinition).Inherit())
                                    .ToList();

                var removedDefs = new List<ListDefinition>();

                // TODO
                foreach (var listDef in clonedDefs)
                {
                    if (listDef.CustomUrl == "_catalogs/appdata")
                        removedDefs.Add(listDef);

                    if (listDef.CustomUrl == "_catalogs/wfpub")
                        removedDefs.Add(listDef);
                }

                foreach (var def in removedDefs)
                    clonedDefs.Remove(def);

                web.ChildModels.Clear();
                web.AddLists(clonedDefs);
            });

            WithDisabledPropertyUpdateValidation(() =>
            {
                TestModel(webModel);
            });
        }
    }
}
