namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowseNodeContext : IReadOnlyBrowseContext
    {
        new IReadOnlyNodeState State { get; }
        void IncrementListCount();
        int ListCount { get; }
        void IncrementBlockListCount();
        int BlockListCount { get; }
    }

    public class ReadOnlyBrowseNodeContext : ReadOnlyBrowseContext, IReadOnlyBrowseNodeContext
    {
        #region Init
        public ReadOnlyBrowseNodeContext(IReadOnlyNodeState nodeState)
            : base(nodeState)
        {
        }
        #endregion

        #region Properties
        public new IReadOnlyNodeState State { get { return (IReadOnlyNodeState)base.State; } }
        public int ListCount { get; private set; }
        public int BlockListCount { get; private set; }
        #endregion

        #region Client Interface
        public void IncrementListCount() { ListCount++; }
        public void IncrementBlockListCount() { BlockListCount++; }
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
