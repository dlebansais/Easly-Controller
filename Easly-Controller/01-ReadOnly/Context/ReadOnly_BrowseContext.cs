using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowseContext
    {
        IReadOnlyNodeState State { get; }
        IReadOnlyIndexCollectionReadOnlyList IndexCollectionList { get; }
        void AddIndexCollection(IReadOnlyIndexCollection Collection);
        IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get; }
        void AddValueProperty(string propertyName, ValuePropertyType type);
        int ListCount { get; }
        void IncrementListCount();
        int BlockListCount { get; }
        void IncrementBlockListCount();
    }

    public class ReadOnlyBrowseContext : IReadOnlyBrowseContext
    {
        #region Init
        public ReadOnlyBrowseContext(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);

            State = state;
            _IndexCollectionList = CreateIndexCollectionList();
            IndexCollectionList = CreateIndexCollectionListReadOnly(_IndexCollectionList);
            _ValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
        }
        #endregion

        #region Properties
        public IReadOnlyNodeState State { get; }
        public IReadOnlyIndexCollectionReadOnlyList IndexCollectionList { get; }
        private IReadOnlyIndexCollectionList _IndexCollectionList;
        public IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get { return _ValuePropertyTypeTable; } }
        private Dictionary<string, ValuePropertyType> _ValuePropertyTypeTable;
        public int ListCount { get; private set; }
        public int BlockListCount { get; private set; }
        #endregion

        #region Client Interface
        public virtual void AddIndexCollection(IReadOnlyIndexCollection collection)
        {
            Debug.Assert(collection != null);
            Debug.Assert(IsCollectionSeparate(collection, IndexCollectionList));

            _IndexCollectionList.Add(collection);
        }

        public void AddValueProperty(string propertyName, ValuePropertyType type)
        {
            Debug.Assert(propertyName != null);
            Debug.Assert(!ValuePropertyTypeTable.ContainsKey(propertyName));

            _ValuePropertyTypeTable.Add(propertyName, type);
        }

        public void IncrementListCount() { ListCount++; }
        public void IncrementBlockListCount() { BlockListCount++; }
        #endregion

        #region Debugging
        public static bool IsCollectionSeparate(IReadOnlyIndexCollection collection, IReadOnlyIndexCollectionReadOnlyList collectionList)
        {
            foreach (IReadOnlyBrowsingChildIndex Index0 in collection.NodeIndexList)
            {
                foreach (IReadOnlyIndexCollection Item in collectionList)
                    foreach (IReadOnlyBrowsingChildIndex Index1 in Item.NodeIndexList)
                        if (Index0.Equals(Index1))
                            return false;
            }

            return true;
        }
        #endregion

        #region Create Methods
        protected virtual IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseContext));
            return new ReadOnlyIndexCollectionList();
        }

        protected virtual IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseContext));
            return new ReadOnlyIndexCollectionReadOnlyList(list);
        }
        #endregion
    }
}
