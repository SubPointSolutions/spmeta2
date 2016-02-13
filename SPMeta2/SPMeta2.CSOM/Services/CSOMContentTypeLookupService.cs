using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.CSOM.Extensions;
using SPMeta2.Exceptions;

namespace SPMeta2.CSOM.Services
{
    public class CSOMContentTypeLookupService
    {
        public virtual ContentType LookupContentTypeByName(List list, string name)
        {
            var context = list.Context;

            var listContentTypes = list.ContentTypes;
            context.Load(listContentTypes);
            context.ExecuteQueryWithTrace();

            var listContentType = listContentTypes.ToList()
                                                  .FirstOrDefault(c => c.Name.ToUpper() == name.ToUpper());

            if (listContentType == null)
            {
                throw new ArgumentNullException(
                    string.Format("Cannot find content type with Name:[{0}] in List:[{1}]",
                        new string[]
                                    {
                                        name,
                                        list.Title
                                    }));
            }

            return listContentType;
        }

        public virtual ContentType LookupListContentTypeById(List targetList, string contentTypeId)
        {
            throw new SPMeta2NotImplementedException();
        }
    }
}
