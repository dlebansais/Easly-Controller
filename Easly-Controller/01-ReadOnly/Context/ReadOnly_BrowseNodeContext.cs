namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowseNodeContext : IReadOnlyBrowseContext
    {
    }

    public class ReadOnlyBrowseNodeContext : ReadOnlyBrowseContext, IReadOnlyBrowseNodeContext
    {
        #region Init
        public ReadOnlyBrowseNodeContext(IReadOnlyNodeState nodeState)
            : base(nodeState)
        {
        }
        #endregion

        #region Create Methods
        protected override IReadOnlyIndexCollectionList CreateChildNodeIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseNodeContext));
            return new ReadOnlyIndexCollectionList();
        }

        protected override IReadOnlyIndexCollectionReadOnlyList CreateChildNodeIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseNodeContext));
            return new ReadOnlyIndexCollectionReadOnlyList(list);
        }
        #endregion
    }
}
