using System;

namespace SPMeta2.Attributes.Regression
{
    /// <summary>
    /// Used by regression testing infrastructure to indicate properties which have to be changed with a new provision.
    /// </summary>
    public class ExpectUpdate : Attribute
    {

    }

    public class ExpectUpdatAsToolbarType : ExpectUpdate
    {

    }

    public class ExpectUpdateAsRichTextMode : ExpectUpdate
    {

    }

    public class ExpectUpdateAsChoiceFieldEditFormat : ExpectUpdate
    {

    }

    public class ExpectUpdateAsStandalone : ExpectUpdate
    {

    }
    public class ExpectUpdateAsLCID : ExpectUpdate
    {

    }

    public class ExpectUpdateDeveloperDashboardSettings : ExpectUpdate
    {

    }

    public class ExpectUpdateAsCamlQuery : ExpectUpdate
    {

    }
    public class ExpectUpdateAsInternalFieldName : ExpectUpdate
    {

    }
    public class ExpectUpdateAsUser : ExpectUpdate
    {

    }

    public class ExpectUpdateAsBasePermission : ExpectUpdate
    {

    }

    public class ExpectUpdateAsUIVersion : ExpectUpdate
    {

    }

    public class ExpectUpdateAsWebPartPageLayoutTemplate : ExpectUpdate
    {

    }
    public class ExpectUpdateAsFileName : ExpectUpdate
    {
        public string Extension { get; set; }
    }

    public class ExpectUpdateAsPageLayoutFileName : ExpectUpdate
    {

    }

    public class ExpectUpdateAsPublishingPageContentType : ExpectUpdate
    {

    }

    public class ExpectUpdateAsByte : ExpectUpdate
    {

    }

    public class ExpectUpdateAsLookupField : ExpectUpdate
    {

    }

    public class ExpectUpdateAsChromeState : ExpectUpdate
    {

    }

    public class ExpectUpdateAsViewScope : ExpectUpdate
    {

    }

    public class ExpectUpdateAsChromeType : ExpectUpdate
    {

    }

    public class ExpectUpdateAsBooleanFieldDefaultValue : ExpectUpdate
    {

    }

    public class ExpectUpdateAsIntRange : ExpectUpdate
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }

    public class ExpectUpdateAsNumberFieldDisplayFormat : ExpectUpdate
    {

    }

    public class ExpectUpdateAsUrlFieldFormat : ExpectUpdate
    {
    }

    public class ExpectUpdateAsEmailAddress : ExpectUpdate
    {

    }

    public class ExpectUpdateAsUrl : ExpectUpdate
    {
        public ExpectUpdateAsUrl()
        {
            Extension = "txt";
        }

        public string Extension { get; set; }
    }


    public class ExpectUpdateAsServerRelativeUrl : ExpectUpdateAsUrl
    {

    }

    public class ExpectUpdateAsCalculatedFieldFormula : ExpectUpdate
    {

    }

    public class ExpectUpdateAssCalculatedFieldOutputType : ExpectUpdate
    {

    }

    public class ExpectUpdateAssCalculatedFieldReferences : ExpectUpdate
    {

    }


    public class ExpectUpdateAsDateTimeFieldCalendarType : ExpectUpdate
    {

    }

    public class ExpectUpdateAsDateTimeFieldDisplayFormat : ExpectUpdate
    {

    }

    public class ExpectUpdateAsDateTimeFieldFriendlyDisplayFormat : ExpectUpdate
    {

    }

    public class ExpectUpdateAsFieldUserSelectionMode : ExpectUpdate
    {

    }

    public class ExpectUpdateAsTargetControlType : ExpectUpdate
    {

    }

    public class ExpectUpdateAsCompatibleSearchDataTypes : ExpectUpdate
    {

    }

    public abstract class ExpectUpdateAsXsltListView : ExpectUpdate
    {

    }

    public class ExpectUpdateAsXsltListViewXmlDefinition : ExpectUpdateAsXsltListView
    {

    }

    public class ExpectUpdateAsXsltListViewXsl : ExpectUpdateAsXsltListView
    {

    }

    public class ExpectUpdateAsXsltListViewXmlLinkUrl : ExpectUpdateAsXsltListView
    {

    }

    public class ExpectUpdateAsXsltListViewXslLinkUrl : ExpectUpdateAsXsltListView
    {

    }

    public class ExpectUpdateAsDateFormat : ExpectUpdate
    {

    }

    public class ExpectUpdateAsTestSecurityGroup : ExpectUpdate
    {

    }
}
