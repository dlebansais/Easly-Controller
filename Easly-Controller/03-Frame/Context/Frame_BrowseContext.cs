namespace EaslyController.Frame
{
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal interface IFrameBrowseContext : IWriteableBrowseContext
    {
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        new IFrameNodeState State { get; }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        new IFrameIndexCollectionReadOnlyList IndexCollectionList { get; }
    }

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal class FrameBrowseContext : WriteableBrowseContext, IFrameBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowseContext"/> class.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public FrameBrowseContext(IFrameNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public new IFrameNodeState State { get { return (IFrameNodeState)base.State; } }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public new IFrameIndexCollectionReadOnlyList IndexCollectionList { get { return (IFrameIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCollectionList object.
        /// </summary>
        private protected override IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowseContext));
            return new FrameIndexCollectionList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollectionReadOnlyList object.
        /// </summary>
        private protected override IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowseContext));
            return new FrameIndexCollectionReadOnlyList((IFrameIndexCollectionList)list);
        }
        #endregion
    }
}
