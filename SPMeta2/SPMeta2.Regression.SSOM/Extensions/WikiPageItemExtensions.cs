using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace SPMeta2.Regression.SSOM.Extensions
{
  internal static  class WikiPageItemExtensions
    {
      public static string GetWikiPageContent(this SPListItem pageItem)
      {
          return pageItem[SPBuiltInFieldId.WikiField] as string;
      }
    }
}
