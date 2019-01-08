using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    public interface IWriteableBrowseContext : IReadOnlyBrowseContext
    {
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        new IWriteableNodeState State { get; }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        new IWriteableIndexCollectionReadOnlyList IndexCollectionList { get; }
    }

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    public class WriteableBrowseContext : ReadOnlyBrowseContext, IWriteableBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableBrowseContext"/>.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public WriteableBrowseContext(IWriteableNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public new IWriteableNodeState State { get { return (IWriteableNodeState)base.State; } }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public new IWriteableIndexCollectionReadOnlyList IndexCollectionList { get { return (IWriteableIndexCollectionReadOnlyList)base.IndexCollectionList; } }
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
