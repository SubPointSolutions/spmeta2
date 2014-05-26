using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.CSOM.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.CSOM.Tests.Auto
{
    public class AutoTest : ClientOMSharePointTestBase
    {
        [TestMethod]
        [TestCategory("AUTO-CSOM")]
        public void CanMakeSureSPMeta2WorksWell()
        {

            WithStaticSharePointClientContext(context =>
            {

            });
        }
    }
}
