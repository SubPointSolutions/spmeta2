
namespace SPMeta2.Definitions
{
    public class ContentTypeLinkDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// This property is used by server side provision.
        /// Not supported in client side. Use "ContentTypeName" instead
        /// </summary>
        public string ContentTypeId { get; set; }

        /// <summary>
        /// This property is used by client side provision 
        /// </summary>
        public string ContentTypeName { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("ContentTypeName:[{0}] ContentTypeId:[{1}]", ContentTypeName, ContentTypeId);
        }

        #endregion
    }
}
