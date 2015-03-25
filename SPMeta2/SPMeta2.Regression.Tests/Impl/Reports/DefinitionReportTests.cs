using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Attributes.Regression;
using SPMeta2.Regression.Tests.Impl.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Tests.Impl.Reports
{
    [TestClass]
    public class DefinitionReportTests : DefinitionTests
    {
        #region common

        [ClassInitializeAttribute]
        public static void Init(TestContext context)
        {
            LoadDefinitions();
        }

        #endregion

        #region reports

        [TestMethod]
        [TestCategory("Regression.Reports.Definitions")]
        public void CreateDefinitionCoverageReport()
        {

            var result = new StringBuilder();


            foreach (var definitionType in DefinitionTypes.OrderBy(d => d.Name))
            {
                var allProps = definitionType.GetProperties().OrderBy(p => p.Name);
                var validatedProps = allProps.Where(p => p.GetCustomAttributes(typeof(ExpectValidationAttribute)).Any());
                var updatableProps = allProps.Where(p => p.GetCustomAttributes(true).Any(a => (a as ExpectUpdate) != null));

                Trace.WriteLine(definitionType.Name);

                result.Append("<table class='table table-bordered table-striped'>");

                result.Append("<thead>");
                result.Append("<tr>");
                result.AppendFormat("<th style='width: 150px;'>{0}</th>", definitionType.Name);
                result.Append("<th>Property</th>");
                result.Append("<th>Validated</th>");
                result.Append("<th>Updated</th>");
                result.Append("</tr>");
                result.Append("</thead>");

                result.Append("<tr>");

                bool isFirst = true;

                foreach (var prop in allProps)
                {
                    if (!isFirst)
                    {
                        result.Append("<tr>");
                    }

                    var isValidated = validatedProps.Any(p => p.Name == prop.Name);
                    var isUpdated = updatableProps.Any(p => p.Name == prop.Name);

                    result.AppendFormat("<td></td>");
                    result.AppendFormat("<td>{0}</td>", prop.Name);
                    result.AppendFormat("<td>{0}</td>", isValidated);
                    result.AppendFormat("<td>{0}</td>", isUpdated);

                    Trace.WriteLine(string.Format("{0}{1} - validated:{2} updated:{3}", '\t', prop.Name, isValidated, isUpdated));

                    isFirst = false;

                    if (!isFirst)
                    {
                        result.Append("</tr>");
                    }
                }

                result.Append("</table>");
                result.Append("<br/>");
                result.Append("<br/>");
            }

            var reportHtml = result.ToString();
            Trace.WriteLine(reportHtml);
        }

        #endregion
    }
}
