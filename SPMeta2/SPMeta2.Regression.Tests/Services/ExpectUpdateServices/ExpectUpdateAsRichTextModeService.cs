using System;
using System.Collections.Generic;
using System.Reflection;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Enumerations;
using SPMeta2.Definitions.Fields;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateAsRichTextModeService : ExpectUpdateGenericService<ExpectUpdateAsRichTextMode>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            var curentValue = prop.GetValue(obj) as string;

            var values = new List<string>();

            values.Add(BuiltInRichTextMode.Compatible);
            values.Add(BuiltInRichTextMode.FullHtml);
            values.Add(BuiltInRichTextMode.HtmlAsXml);
            //values.Add(BuiltInRichTextMode.ThemeHtml);

            if (!string.IsNullOrEmpty(curentValue))
                values.Remove(curentValue);

            return RndService.RandomFromArray(values);
        }
    }
}
