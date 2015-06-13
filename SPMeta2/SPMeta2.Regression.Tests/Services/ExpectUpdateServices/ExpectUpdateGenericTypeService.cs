using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using SPMeta2.Attributes.Regression;
using SPMeta2.Containers.Services;
using SPMeta2.Definitions.Webparts;
using SPMeta2.Enumerations;
using SPMeta2.Exceptions;
using SPMeta2.Utils;

namespace SPMeta2.Regression.Tests.Services.ExpectUpdateServices
{
    public class ExpectUpdateService : ExpectUpdateGenericService<ExpectUpdate>
    {
        public override object GetNewPropValue(ExpectUpdate attr, object obj, PropertyInfo prop)
        {
            object newValue = null;

            if (prop.PropertyType == typeof(byte[]))
            {
                if (obj is WebPartGalleryFileDefinition && prop.Name == "Content")
                {
                    // change web part
                    var webPartXmlString = Encoding.UTF8.GetString(prop.GetValue(obj) as byte[]);
                    var webPartXml = WebpartXmlExtensions.LoadWebpartXmlDocument(webPartXmlString);

                    webPartXml.SetTitleUrl(RndService.HttpUrl());

                    newValue = Encoding.UTF8.GetBytes(webPartXml.ToString());
                }
                else
                {
                    newValue = RndService.Content();
                }
            }
            else if (prop.PropertyType == typeof(string))
            {
                newValue = RndService.String();
            }
            else if (prop.PropertyType == typeof(bool))
            {
                newValue = RndService.Bool();
            }
            else if (prop.PropertyType == typeof(bool?))
            {
                var oldValue = prop.GetValue(obj) as bool?;

                if (oldValue == null)
                {
                    newValue = RndService.Bool();
                }
                else
                {
                    newValue = !oldValue.Value;
                }
            }
            else if (prop.PropertyType == typeof(int))
            {
                newValue = RndService.Int();
            }
            else if (prop.PropertyType == typeof(int?))
            {
                newValue = RndService.Bool()
                    ? (int?)null
                    : RndService.Int();
            }
            else if (prop.PropertyType == typeof(uint))
            {
                newValue = (uint)RndService.Int();
            }
            else if (prop.PropertyType == typeof(uint?))
            {
                newValue = RndService.Bool()
                            ? null
                            : (uint?)RndService.Int();
            }
            else if (prop.PropertyType == typeof(Collection<string>))
            {
                var resultLength = RndService.Int(10);
                var result = new Collection<string>();

                for (var index = 0; index < resultLength; index++)
                    result.Add(RndService.String());

                newValue = result;
            }
            else if (prop.PropertyType == typeof(List<string>))
            {
                var resultLength = RndService.Int(10);
                var result = new List<string>();

                for (var index = 0; index < resultLength; index++)
                    result.Add(RndService.String());

                newValue = result;
            }
            else if (prop.PropertyType == typeof(double?) || prop.PropertyType == typeof(double))
            {
                newValue = (double)RndService.Int();
            }
            else
            {
                throw new SPMeta2NotImplementedException(string.Format("Update validation for type: [{0}] is not supported yet", prop.PropertyType));
            }

            return newValue;
        }
    }
}
