﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.Attributes;
using SPMeta2.Attributes.Regression;
using SPMeta2.Enumerations;
using SPMeta2.Utils;
using System.Runtime.Serialization;

namespace SPMeta2.Definitions.Fields
{
    /// <summary>
    /// Allows to define and deploy text field.
    /// </summary>
    /// 
    [SPObjectType(SPObjectModelType.SSOM, "Microsoft.SharePoint.SPFieldText", "Microsoft.SharePoint")]
    [SPObjectTypeAttribute(SPObjectModelType.CSOM, "Microsoft.SharePoint.Client.FieldText", "Microsoft.SharePoint.Client")]

    [DefaultParentHost(typeof(SiteDefinition))]
    [DefaultRootHost(typeof(SiteDefinition))]

    [Serializable] 
    [DataContract]
    [ExpectArrayExtensionMethod]
    [ExpectManyInstances]

    public class TextFieldDefinition : FieldDefinition
    {
        #region constructors

        public TextFieldDefinition()
        {
            FieldType = BuiltInFieldTypes.Text;
        }

        #endregion

        #region properties

        [ExpectValidation]
        [ExpectUpdateAsIntRange(MinValue = 1, MaxValue = 100)]
        [DataMember]
        public int? MaxLength { get; set; }

        #endregion

        #region methods

        public override string ToString()
        {
            return new ToStringResult<TextFieldDefinition>(this, base.ToString())
                          .AddPropertyValue(p => p.MaxLength)
                          .ToString();
        }

        #endregion
    }
}
