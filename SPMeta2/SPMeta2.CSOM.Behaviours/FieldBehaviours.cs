using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace SPMeta2.CSOM.Behaviours
{
    public static class FieldBehaviours
    {
        #region common extensions

        public static Field MakeChoices(this Field field, IEnumerable<string> choices)
        {
            return MakeChoices(field, choices, true);
        }

        public static Field MakeChoices(this Field field, IEnumerable<string> choiceValues, bool cleanOldChoice = true)
        {
            var choiceField = field as FieldChoice;

            if (choiceField != null)
            {
                var tmpChoices = new List<string>();

                if (!cleanOldChoice)
                    tmpChoices.AddRange(choiceField.Choices);

                foreach (var choiceValue in choiceValues)
                    if (!tmpChoices.Contains(choiceValue))
                        tmpChoices.Add(choiceValue);

                choiceField.Choices = tmpChoices.ToArray();
            }

            return field;
        }

        public static Field MakeDefaultValue(this Field field, string value)
        {
            field.DefaultValue = value;

            return field;
        }

        public static Field MakeTitle(this Field field, string title)
        {
            field.Title = title;

            return field;
        }

        public static Field MakeLookupConnectionToList(this Field field, Guid webId, Guid listId)
        {
            return MakeLookupConnectionToList(field, webId, listId, "Title");
        }

        public static Field MakeLookupConnectionToList(this Field field, Guid webId, Guid listId, string showFieldName)
        {
            var context = field.Context;
            var lookupField = context.CastTo<FieldLookup>(field);

            context.Load(lookupField, f => f.LookupList);
            context.ExecuteQuery();

            if (lookupField != null && string.IsNullOrEmpty(lookupField.LookupList))
            {
                lookupField.LookupWebId = webId;
                lookupField.LookupList = listId.ToString();
                lookupField.LookupField = showFieldName;
            }

            return field;
        }

        public static Field MakeDependentLookupConnectionToList(this Field field, Guid webId, Guid listId, Guid primaryLookupFieldId, string showFieldName)
        {
            var context = field.Context;
            var lookupField = context.CastTo<FieldLookup>(field);

            context.Load(lookupField, f => f.LookupList);
            context.ExecuteQuery();

            if (lookupField != null && string.IsNullOrEmpty(lookupField.LookupList))
            {
                lookupField.LookupWebId = webId;
                lookupField.LookupList = listId.ToString();
                lookupField.LookupField = showFieldName;
                lookupField.PrimaryFieldId = primaryLookupFieldId.ToString();
            }

            return field;
        }

        public static Field MakeNotRequired(this Field field)
        {
            return MakeRequired(field, false);
        }

        public static Field MakeRequired(this Field field)
        {
            return MakeRequired(field, true);
        }

        public static Field MakeRequired(this Field field, bool isRequired)
        {
            field.Required = isRequired;

            return field;
        }

        public static Field MakeHidden(this Field field, bool isRequired)
        {
            field.MakeNotRequired();
            field.Hidden = true;

            return field;
        }

        #endregion
    }
}
