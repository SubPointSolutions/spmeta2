using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPMeta2.Definitions;
using SPMeta2.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SPMeta2.Regression.Tests.Services
{
    [TestClass]
    public class SerializationServicesTests
    {
        #region tests

        [TestMethod]
        [TestCategory("Regression.Services.Serialization")]
        public void CanSerializeAndDeserializeJSON()
        {
            var obj = new FieldDefinition();
            var jsonService = new DefaultJSONSerializationService();

            var strValue = jsonService.Serialize(obj);
            jsonService.Deserialize(obj.GetType(), strValue);

        }


        [TestMethod]
        [TestCategory("Regression.Services.Serialization")]
        public void CanSerializeAndDeserializeXML()
        {
            var obj = new FieldDefinition();
            var jsonService = new DefaultJSONSerializationService();

            var strValue = jsonService.Serialize(obj);
            jsonService.Deserialize(obj.GetType(), strValue);

        }

        #endregion
    }
}
