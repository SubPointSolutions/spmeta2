using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Containers.Services;

namespace SPMeta2.Regression.Tests.Services
{
    public class VSAssertService : AssertServiceBase
    {
        public override void IsNotNull(object value)
        {
            Assert.IsNotNull(value);
        }

        public override void IsInstanceOfType(object value, Type type)
        {
            Assert.IsInstanceOfType(value, type);
        }

        public override void IsFalse(bool value)
        {
            Assert.IsFalse(value);
        }

        public override void AreEqual(object expectedValue, bool actualValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}
