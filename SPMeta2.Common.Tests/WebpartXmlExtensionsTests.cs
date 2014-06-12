using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Common.Utils;

namespace SPMeta2.Common.Tests
{
    //[TestClass]
    public class WebpartXmlExtensionsTests
    {
        [TestMethod]
        [TestCategory("Common")]
        public void CanGenerateV3CQWP()
        {
            var wpDoc = XDocument.Parse(Webparts.CQWPProjectStatus);

            wpDoc
                .SetTitle("SetTitleSetTitle")
                .SetWebUrl("SetWebUrlSetWebUrl")
                .SetFilterDisplayValue1("SetFilterDisplayValue1SetFilterDisplayValue1")
                .SetDataMappingViewFields("SetDataMappingViewFieldsSetDataMappingViewFields")
                .SetItemStyle("SetItemStyleSetItemStyle")
                .SetListGuid("SetListGuidSetListGuid")
                .SetDataMappings("SetDataMappingsSetDataMappings")
                .SetFilterValue1("SetFilterValue1SetFilterValue1");

            // TODO, some checks via reflection to make sure V3 works well

            var result = wpDoc.ToString();
        }

        [TestMethod]
        [TestCategory("Common")]
        public void CanGenerateV2SomethingHere()
        {
            // TODO, put some test generation for V2 web parts
        }
    }
}
