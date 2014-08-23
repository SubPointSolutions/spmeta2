using System;

namespace SPMeta2.Definitions
{
    /// <summary>
    /// Base definition for all SharePoint artifacts to be defined and deployed.
    /// </summary>
    public abstract class DefinitionBase : ICloneable
    {
        #region contructors

        protected DefinitionBase()
        {
            //InitCollections();
            RequireSelfProcessing = true;
        }

        #endregion

        #region private

        //private void InitCollections()
        //{
        //    var childModels = new ObservableCollection<DefinitionBase>();

        //    childModels.CollectionChanged += ChildModelsCollectionChanged;

        //    ChildModels = childModels;94c94ca6

        //    ModelEvents = new Dictionary<string, List<object>>();
        //}

        //private void ChildModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    switch (e.Action)
        //    {
        //        case NotifyCollectionChangedAction.Add:
        //            {
        //                foreach (var item in e.NewItems)
        //                    ((DefinitionBase)item).ParentModel = this;
        //            }
        //            break;
        //    }
        //}

        #endregion

        #region properties

        public string ObjectType
        {
            get { return GetType().Name; }
            set
            {

            }
        }

        public virtual bool RequireSelfProcessing { get; set; }

        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
