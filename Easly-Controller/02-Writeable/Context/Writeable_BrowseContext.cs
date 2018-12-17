using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    public interface IWriteableBrowseContext : IReadOnlyBrowseContext
    {
    }

    public class WriteableBrowseContext : ReadOnlyBrowseContext, IWriteableBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableBrowseContext "/>.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public WriteableBrowseContext(IReadOnlyNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCollectionList object.
        /// </summary>
        protected override IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowseContext));
            return new WriteableIndexCollectionList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollectionReadOnlyList object.
        /// </summary>
        /// <param name="list"></param>
        protected override IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowseContext));
            return new WriteableIndexCollectionReadOnlyList((IWriteableIndexCollectionList)list);
        }
        #endregion
    }
}
