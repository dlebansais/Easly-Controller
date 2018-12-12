namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowseBlockContext : IReadOnlyBrowseContext
    {
        new IReadOnlyBlockState State { get; }
    }

    public class ReadOnlyBrowseBlockContext : ReadOnlyBrowseContext, IReadOnlyBrowseBlockContext
    {
        #region Init
        public ReadOnlyBrowseBlockContext(IReadOnlyBlockState blockState)
            : base(blockState)
        {
        }
        #endregion

        #region Properties
        public new IReadOnlyBlockState State { get { return (IReadOnlyBlockState)base.State; } }
        #endregion

        #region Create Methods
        protected override IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseBlockContext));
            return new ReadOnlyIndexCollectionList();
        }

        protected override IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowseBlockContext));
            return new ReadOnlyIndexCollectionReadOnlyList(list);
        }
        #endregion
    }
}
