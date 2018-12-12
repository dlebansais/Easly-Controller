using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowseContext
    {
        IReadOnlyState State { get; }
        IReadOnlyIndexCollectionReadOnlyList IndexCollectionList { get; }
        void AddIndexCollection(IReadOnlyIndexCollection Collection);
    }

    public abstract class ReadOnlyBrowseContext : IReadOnlyBrowseContext
    {
        #region Init
        public ReadOnlyBrowseContext(IReadOnlyState state)
        {
            Debug.Assert(state != null);

            State = state;
            _IndexCollectionList = CreateIndexCollectionList();
            IndexCollectionList = CreateIndexCollectionListReadOnly(_IndexCollectionList);
        }
        #endregion

        #region Properties
        public IReadOnlyState State { get; private set; }
        public IReadOnlyIndexCollectionReadOnlyList IndexCollectionList { get; protected set; }
        private IReadOnlyIndexCollectionList _IndexCollectionList;
        #endregion

        #region Client Interface
        public virtual void AddIndexCollection(IReadOnlyIndexCollection collection)
        {
            Debug.Assert(collection != null);
            Debug.Assert(IsCollectionSeparate(collection, IndexCollectionList));

            _IndexCollectionList.Add(collection);
        }
        #endregion

        #region Debugging
        public static bool IsCollectionSeparate(IReadOnlyIndexCollection collection, IReadOnlyIndexCollectionReadOnlyList collectionList)
        {
            foreach (IReadOnlyBrowsingChildNodeIndex Index0 in collection.NodeIndexList)
            {
                foreach (IReadOnlyIndexCollection Item in collectionList)
                    foreach (IReadOnlyBrowsingChildNodeIndex Index1 in Item.NodeIndexList)
                        if (Index0.Equals(Index1))
                            return false;
            }

            return true;
        }
        #endregion

        #region Create Methods
        protected abstract IReadOnlyIndexCollectionList CreateIndexCollectionList();
        protected abstract IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list);
        #endregion
    }
}
