namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowseBlockContext : IReadOnlyBrowseContext
    {
    }

    public class ReadOnlyBrowseBlockContext : ReadOnlyBrowseContext, IReadOnlyBrowseBlockContext
    {
        #region Init
        public ReadOnlyBrowseBlockContext(IReadOnlyBlockState blockState)
            : base(blockState)
        {
        }
        #endregion

        #region Create Methods
        protected override IReadOnlyIndexCollectionList CreateChildNodeIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseBlockContext));
            return new ReadOnlyIndexCollectionList();
        }

        protected override IReadOnlyIndexCollectionReadOnlyList CreateChildNodeIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseBlockContext));
            return new ReadOnlyIndexCollectionReadOnlyList(list);
        }
        #endregion
    }
}
