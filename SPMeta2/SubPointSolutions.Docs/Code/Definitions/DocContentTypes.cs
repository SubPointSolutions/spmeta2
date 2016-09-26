using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Definitions;
using SPMeta2.Enumerations;
using SubPointSolutions.Docs.Code.Consts;

namespace SPMeta2.Docs.ProvisionSamples.Definitions
{
    public static class DocContentTypes
    {
        #region properties

        public static ContentTypeDefinition CustomerAccount = new ContentTypeDefinition
        {
            Name = "Customer Account",
            Id = new Guid("ddc46a66-19a0-460b-a723-c84d7f60a342"),
            ParentContentTypeId = BuiltInContentTypeId.Item,
            Group = DocConsts.DefaultContentTypeGroup
        };

        public static ContentTypeDefinition CustomerDocument = new ContentTypeDefinition
        {
            Name = "Customer Document",
            Id = new Guid("6e03a8a6-b680-4f08-96a2-e901360575cb"),
            ParentContentTypeId = BuiltInContentTypeId.Document,
            Group = DocConsts.DefaultContentTypeGroup
        };

        #endregion
    }
}
