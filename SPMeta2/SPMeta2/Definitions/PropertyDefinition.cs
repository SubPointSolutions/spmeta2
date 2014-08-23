namespace SPMeta2.Definitions
{
    /// <summary>
    /// Allows to define and deploy property value to the SharePoint property bags.
    /// </summary>
    public class PropertyDefinition : DefinitionBase
    {
        #region properties

        /// <summary>
        /// Name of the target property.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Value of the target property.
        /// </summary>
        public object Value { get; set; }

        #endregion
    }
}
