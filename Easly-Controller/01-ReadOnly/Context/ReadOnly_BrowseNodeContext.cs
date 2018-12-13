using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowseNodeContext : IReadOnlyBrowseContext
    {
        new IReadOnlyNodeState State { get; }
        void IncrementListCount();
        int ListCount { get; }
        void IncrementBlockListCount();
        int BlockListCount { get; }
        void AddValueProperty(string propertyName, ValuePropertyType type);
        IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get; }
    }

    public class ReadOnlyBrowseNodeContext : ReadOnlyBrowseContext, IReadOnlyBrowseNodeContext
    {
        #region Init
        public ReadOnlyBrowseNodeContext(IReadOnlyNodeState nodeState)
            : base(nodeState)
        {
            _ValuePropertyTypeTable = new Dictionary<string, ValuePropertyType>();
        }
        #endregion

        #region Properties
        public new IReadOnlyNodeState State { get { return (IReadOnlyNodeState)base.State; } }
        public int ListCount { get; private set; }
        public int BlockListCount { get; private set; }
        public IReadOnlyDictionary<string, ValuePropertyType> ValuePropertyTypeTable { get { return _ValuePropertyTypeTable; } }
        private Dictionary<string, ValuePropertyType> _ValuePropertyTypeTable;
        #endregion

        #region Client Interface
        public void IncrementListCount() { ListCount++; }
        public void IncrementBlockListCount() { BlockListCount++; }

        public void AddValueProperty(string propertyName, ValuePropertyType type)
        {
            Debug.Assert(propertyName != null);
            Debug.Assert(!ValuePropertyTypeTable.ContainsKey(propertyName));

            _ValuePropertyTypeTable.Add(propertyName, type);
        }
        #endregion

        #region Create Methods
        protected override IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseNodeContext));
            return new ReadOnlyIndexCollectionList();
        }

        protected override IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseNodeContext));
            return new ReadOnlyIndexCollectionReadOnlyList(list);
        }
        #endregion
    }
}
