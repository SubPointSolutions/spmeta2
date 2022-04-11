using Microsoft.SharePoint;
using SPMeta2.Definitions;
using SPMeta2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SPMeta2.SSOM.Extensions;

namespace SPMeta2.SSOM.Services
{
    public class SSOMListViewLookupService : LookupServiceBase
    {
        #region methods

        public virtual SPView FindView(SPList targetList, ListViewDefinition listViewModel)
        {
            // lookup by title
            TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving view by Title: [{0}]", listViewModel.Title);
            var currentView = FindByTitle(targetList, listViewModel.Title);

            // lookup by URL match
            if (currentView == null && !string.IsNullOrEmpty(listViewModel.Url))
            {
                TraceService.VerboseFormat((int)LogEventId.ModelProvisionCoreCall, "Resolving view by URL: [{0}]", listViewModel.Url);
                currentView = FindByUrl(targetList, listViewModel.Url);
            }

            return currentView;
        }

        public virtual SPView FindByTitle(SPList list, string title)
        {
            return list.Views.FindByName(title);
        }

        public virtual SPView FindByUrl(SPList list, string url)
        {
            // long story
            // Problem with webpart deployment to display form #891
            // https://github.com/SubPointSolutions/spmeta2/issues/891

            // seems that if you have a XsltListViewWebPart on list forms-pages
            // the result targetList.Views collection would have ALL list views + your binded view from the web part

            // 	targetList.RootFolder.Url	"Lists/TestList3"	string
            //  targetList.Views[0].ServerRelativeUrl	"/Lists/TestList3/AllItems.aspx"	string
            //  targetList.Views[1].ServerRelativeUrl	"/Lists/TestList1/DispForm.aspx"	string
            //  targetList.Views[2].ServerRelativeUrl	"/Lists/TestList3/AllItems3.aspx"	string

            // so we trim by the URL of the list to ensure lookup within 'current' list
            var safeUrl = url.ToUpper();
            var safeListUrl = list.RootFolder.Url.ToUpper();

            //var result = list.Views.OfType<SPView>()
            //                       .FirstOrDefault(w => w.Url.ToUpper().EndsWith(safeUrl));

            var result = list.Views.OfType<SPView>()
                             .FirstOrDefault(w =>
                                 // match by view url
                                             w.Url.ToUpper().EndsWith(safeUrl)
                                                 // and within the current list
                                             && w.Url.ToUpper().Trim('\\').Trim('/').StartsWith(safeListUrl.Trim('\\').Trim('/'))
                                             );

            return result;
        }

        #endregion
    }
}
