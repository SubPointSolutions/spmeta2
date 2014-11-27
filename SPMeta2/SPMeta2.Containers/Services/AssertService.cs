using System;

namespace SPMeta2.Containers.Services
{
    public abstract class AssertServiceBase
    {
        public abstract void IsNotNull(object value);
        public abstract void IsInstanceOfType(object value, Type type);
        public abstract void IsFalse(bool value);
        public abstract void AreEqual(object expectedValue, bool actualValue);
    }
}
