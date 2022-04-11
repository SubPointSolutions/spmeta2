using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Services.Impl;

namespace SPMeta2.Regression.Tests.Impl.Services
{
    [TestClass]
    public class SerializationServicesTests
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Services.Serialization")]
        [TestCategory("CI.Core")]
        public void CanSerializeAndDeserializeJSON()
        {
            var obj = new FieldDefinition();
            var service = new DefaultJSONSerializationService();

            var strValue = service.Serialize(obj);
            service.Deserialize(obj.GetType(), strValue);

        }


        [TestMethod]
        [TestCategory("Regression.Services.Serialization")]
        [TestCategory("CI.Core")]
        public void CanSerializeAndDeserializeXML()
        {
            var obj = new FieldDefinition();
            var service = new DefaultXMLSerializationService();

            var strValue = service.Serialize(obj);
            service.Deserialize(obj.GetType(), strValue);

        }

        #endregion
    }
}
