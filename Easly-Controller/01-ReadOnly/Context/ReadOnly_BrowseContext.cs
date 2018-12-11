using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowseContext
    {
        IReadOnlyState State { get; }
        IReadOnlyIndexCollectionReadOnlyList ChildNodeIndexCollectionList { get; }
        void AddCollection(IReadOnlyIndexCollection Collection);
    }

    public abstract class ReadOnlyBrowseContext : IReadOnlyBrowseContext
    {
        #region Init
        public ReadOnlyBrowseContext(IReadOnlyState state)
        {
            Debug.Assert(state != null);

            State = state;
            _ChildNodeIndexCollectionList = CreateChildNodeIndexCollectionList();
            ChildNodeIndexCollectionList = CreateChildNodeIndexCollectionListReadOnly(_ChildNodeIndexCollectionList);
        }
        #endregion

        #region Properties
        public IReadOnlyState State { get; private set; }
        #endregion

        #region ChildNodeIndexCollectionList
        public virtual void AddCollection(IReadOnlyIndexCollection collection)
        {
            Debug.Assert(collection != null);
            Debug.Assert(!ChildNodeIndexCollectionList.Contains(collection));

            _ChildNodeIndexCollectionList.Add(collection);
        }

        public IReadOnlyIndexCollectionReadOnlyList ChildNodeIndexCollectionList { get; protected set; }
        protected IReadOnlyIndexCollectionList _ChildNodeIndexCollectionList { get; set; }
        #endregion

        #region Create Methods
        protected abstract IReadOnlyIndexCollectionList CreateChildNodeIndexCollectionList();
        protected abstract IReadOnlyIndexCollectionReadOnlyList CreateChildNodeIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list);
        #endregion
    }
}
