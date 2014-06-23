namespace SPMeta2.Definitions
{
    public class WebPartDefinition : DefinitionBase
    {
        #region contructors

        public WebPartDefinition()
        {

        }

        #endregion

        #region properties

        public string Title { get; set; }
        public string Id { get; set; }

        public string ZoneId { get; set; }
        public int ZoneIndex { get; set; }

        public string WebpartFileName { get; set; }
        public string WebpartType { get; set; }
        public string WebpartXmlTemplate { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] Id:[{1}] WebpartFileName:[{2}] WebpartType:[{3}] ZoneId:[{4}] ZoneIndex:[{5}]",
                new[] { Title, Id, WebpartFileName, WebpartType, ZoneId, ZoneIndex.ToString() });
        }

        #endregion
    }
}
