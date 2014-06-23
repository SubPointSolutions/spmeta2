using System;
using SPMeta2.Definitions;
using SPMeta2.Regression.Const;

namespace SPMeta2.Regression.Models.DynamicModels
{
    public class DynamicContentTypeModels
    {
        #region properties

        public static ContentTypeDefinition ItemContentType = GetContentTypeTestTemplate(SPBuiltInContentTypeId.Item);
        public static ContentTypeDefinition DocumentContentType = GetContentTypeTestTemplate(SPBuiltInContentTypeId.Document);
        public static ContentTypeDefinition TaskContentType = GetContentTypeTestTemplate(SPBuiltInContentTypeId.Task);
        public static ContentTypeDefinition LinkContentType = GetContentTypeTestTemplate(SPBuiltInContentTypeId.Link);

        #endregion

        #region methods

        public static ContentTypeDefinition GetContentTypeTestTemplate(string parentContentTypeId)
        {
            return GetContentTypeTestTemplate(parentContentTypeId, null);
        }

        public static ContentTypeDefinition GetContentTypeTestTemplate(string parentContentTypeId, Action<ContentTypeDefinition> action)
        {
            return GetContentTypeTestTemplate(Guid.NewGuid().ToString("N"), parentContentTypeId, action);
        }

        public static ContentTypeDefinition GetContentTypeTestTemplate(string name, string parentContentTypeId, Action<ContentTypeDefinition> action)
        {
            var result = new ContentTypeDefinition
            {
                Id = Guid.NewGuid(),
                Name = name,
                ParentContentTypeId = parentContentTypeId,
                Description = Guid.NewGuid().ToString("N"),
                Group = ModelConsts.DefaultFieldGroup
            };

            if (action != null) action(result);

            return result;
        }

        #endregion
    }
}
