using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPMeta2.Attributes.Identity;
using SPMeta2.Attributes.Regression;
using System.Runtime.Serialization;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;

namespace SPMeta2.Definitions.Base
{
    [DataContract]
    /// <summary>
    /// Base definitino for web part definitions - generic web part and all other 'typed' web parts.
    /// </summary>
    public abstract class WebPartDefinitionBase : DefinitionBase
    {
        #region constructors

        public WebPartDefinitionBase()
        {
            ChromeState = BuiltInPartChromeState.Normal;
            ChromeType = BuiltInPartChromeType.Default;

            ParameterBindings = new List<ParameterBindingValue>();
        }

        #endregion

        #region properties

        [ExpectValidation]
        [DataMember]
        [ExpectNullable]
        public string ExportMode { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsChromeState]
        public string ChromeState { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsChromeType]
        public string ChromeType { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 100, MaxValue = 500)]
        public int? Width { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsIntRange(MinValue = 100, MaxValue = 500)]
        public int? Height { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsUrl]
        public string TitleUrl { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdate]
        public string Description { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdate]
        public string ImportErrorMessage { get; set; }

        [ExpectValidation]
        [DataMember]
        [ExpectUpdateAsUrl]
        public string TitleIconImageUrl { get; set; }

        /// <summary>
        /// Title of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        [IdentityKey]
        public string Title { get; set; }

        /// <summary>
        /// ID of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]
        [IdentityKey]
        public virtual string Id { get; set; }

        /// <summary>
        /// ZoneId of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        public string ZoneId { get; set; }

        /// <summary>
        /// ZoneIndex of the target web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [DataMember]

        public int ZoneIndex { get; set; }

        [ExpectValidation]
        [DataMember]
        public List<ParameterBindingValue> ParameterBindings { get; set; }

        #endregion

        #region properties

        /// <summary>
        /// File name of the target web part definition from the web part gallery.
        /// 
        /// WebpartFileName is used for the first priority to deploy web part.
        /// </summary>
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Web part content")]
        [DataMember]
        public string WebpartFileName { get; set; }

        private string _webpartType;

        /// <summary>
        /// Type of the target web part.
        /// 
        /// WebpartType is used as a second priority to deploy web part.
        /// </summary>

        [ExpectValidation]
        [ExpectRequired(GroupName = "Web part content")]
        [DataMember]
        public virtual string WebpartType
        {
            get { return _webpartType; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                var parts = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                // expect 5 parts
                // for instance, 
                // System.Array, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
                // https://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname(v=vs.110).aspx
                if (parts.Length != 5)
                {
                    throw new SPMeta2InvalidDefinitionPropertyException(
                        "WebpartType must be in AssemblyQualifiedName format - https://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx");
                }

                _webpartType = value;
            }
        }

        /// <summary>
        /// XML definition of the target web part.
        /// Both V2 and V3 definition are supported.
        /// 
        /// WebpartXmlTemplate is used as the final step to deploy web part. 
        /// </summary>        
        /// 
        [ExpectValidation]
        [ExpectRequired(GroupName = "Web part content")]
        [DataMember]
        public string WebpartXmlTemplate { get; set; }

        /// <summary>
        /// Indicated if the web part should be added to the publishing or wiki page content area.
        /// </summary>
        [DataMember]
        public bool AddToPageContent { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return string.Format("Title:[{0}] Id:[{1}] WebpartFileName:[{2}] WebpartType:[{3}] ZoneId:[{4}] ZoneIndex:[{5}]",
                new[] { Title, Id, WebpartFileName, WebpartType, ZoneId, ZoneIndex.ToString() });
        }

        #endregion
    }

    [DataContract]
    public class ParameterBindingValue
    {
        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual string Location { get; set; }
    }
}
