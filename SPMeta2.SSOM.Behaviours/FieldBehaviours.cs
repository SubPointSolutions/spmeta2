using Microsoft.SharePoint;
using SPMeta2.Definitions;
using System.Collections.Generic;

namespace SPMeta2.SSOM.Behaviours
{
    public static class FieldBehaviours
    {
        #region common extensions

        public static SPField MakeChoices(this SPField field, IEnumerable<string> choices)
        {
            return MakeChoices(field, choices, true);
        }

        public static SPField MakeChoices(this SPField field, IEnumerable<string> choices, bool clean = false)
        {
            var choiceField = field as SPFieldChoice;

            if (choiceField != null)
            {
                if (clean)
                    choiceField.Choices.Clear();

                foreach (var choice in choices)
                    choiceField.Choices.Add(choice);
            }

            return field;
        }

        public static SPField MakeDefaultValues(this SPField field, string defValue)
        {
            field.DefaultValue = defValue;

            return field;
        }


        public static SPField MakeTitle(this SPField field, string title)
        {
            field.Title = title;

            return field;
        }

        public static SPField MakeNotRequired(this SPField field)
        {
            return MakeRequired(field, false);
        }

        public static SPField MakeItPretty(this SPField field)
        {
            field.Title = "sdfsdfsd";

            return field;
        }

        public static SPField MakeRequired(this SPField field)
        {
            return MakeRequired(field, true);
        }

        public static SPField MakeRequired(this SPField field, bool isRequired)
        {
            field.Required = isRequired;

            return field;
        }

        public static SPField MakeHidden(this SPField field, bool isRequired)
        {
            field.MakeNotRequired();
            field.Hidden = true;

            return field;
        }

        #endregion

        #region lookups

        public static SPField MakeLookupBinding(this SPField field, SPList targetList)
        {
            // (string) to call the right overload
            return MakeLookupBinding(field, targetList, (string)null);
        }

        public static SPField MakeLookupBinding(this SPField field, SPList targetList, FieldDefinition fieldModel)
        {
            return MakeLookupBinding(field, targetList, fieldModel.InternalName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="targetList"></param>
        /// <param name="fieldToShow">Internal name of the filed to be shown. If null/empty, "ID" field is used.</param>
        /// <returns></returns>
        public static SPField MakeLookupBinding(this SPField field, SPList targetList, string fieldToShow)
        {
            var lookupField = field as SPFieldLookup;

            if (lookupField != null)
            {
                // common properties
                lookupField.LookupList = targetList.ID.ToString();
                lookupField.LookupWebId = targetList.ParentWeb.ID;

                // ID by default
                lookupField.LookupField = !string.IsNullOrEmpty(fieldToShow) ? fieldToShow : "ID";
            }

            return field;
        }

        #endregion
    }
}
