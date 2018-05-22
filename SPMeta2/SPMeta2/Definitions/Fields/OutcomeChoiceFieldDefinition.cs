using System;
using System.Runtime.Serialization;
using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using System.Collections.ObjectModel;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy task outcome field.
    /// </summary>
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.OutcomeChoiceField", "Microsoft.SharePoint")]
    [SPObjectType(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.Field", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable]
    [DataContract]
    [ExpectArrayExtensionMethod]
    [ExpectManyInstances]
    public class OutcomeChoiceFieldDefinition : ChoiceFieldDefinition
    {
        #region constructors

        public OutcomeChoiceFieldDefinition()
        {
            this.FieldType = BuiltInFieldTypes.OutcomeChoice;
        }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResultRaw(base.ToString())

                          .ToString();
        }

        #endregion
    }
}
