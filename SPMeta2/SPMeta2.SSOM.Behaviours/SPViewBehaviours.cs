using Microsoft.SharePoint;

namespace SPMeta2.SSOM.Behaviours
{
    public static class SPViewBehaviours
    {
        #region methods

        public static SPView MakeListViewScopeAsRecursiveAll(this SPView view)
        {
            return MakeListViewScope(view, SPViewScope.RecursiveAll);
        } 

        public static SPView MakeListViewScopeAsRecursive(this SPView view)
        {
            return MakeListViewScope(view, SPViewScope.Recursive);
        } 

        public static SPView MakeListViewScopeAsFilesOnly(this SPView view)
        {
            return MakeListViewScope(view, SPViewScope.FilesOnly);
        } 

        public static SPView MakeListViewScopeAsDefault(this SPView view)
        {
            return MakeListViewScope(view, SPViewScope.Default);
        } 

        public static SPView MakeListViewScope(this SPView view, SPViewScope scope)
        {
            view.Scope = scope;

            return view;
        } 

        #endregion
    }
}
