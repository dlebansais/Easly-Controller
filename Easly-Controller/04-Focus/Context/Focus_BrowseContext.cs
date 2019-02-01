namespace EaslyController.Focus
{
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal interface IFocusBrowseContext : IFrameBrowseContext
    {
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        new IFocusNodeState State { get; }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        new IFocusIndexCollectionReadOnlyList IndexCollectionList { get; }
    }

    /// <summary>
    /// Context for browsing child nodes of a parent node.
    /// </summary>
    internal class FocusBrowseContext : FrameBrowseContext, IFocusBrowseContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowseContext"/> class.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public FocusBrowseContext(IFocusNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State this context is browsing.
        /// </summary>
        public new IFocusNodeState State { get { return (IFocusNodeState)base.State; } }

        /// <summary>
        /// List of index collections that have been added during browsing.
        /// </summary>
        public new IFocusIndexCollectionReadOnlyList IndexCollectionList { get { return (IFocusIndexCollectionReadOnlyList)base.IndexCollectionList; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxCollectionList object.
        /// </summary>
        private protected override IReadOnlyIndexCollectionList CreateIndexCollectionList()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowseContext));
            return new FocusIndexCollectionList();
        }

        /// <summary>
        /// Creates a IxxxIndexCollectionReadOnlyList object.
        /// </summary>
        private protected override IReadOnlyIndexCollectionReadOnlyList CreateIndexCollectionListReadOnly(IReadOnlyIndexCollectionList list)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowseContext));
            return new FocusIndexCollectionReadOnlyList((IFocusIndexCollectionList)list);
        }
        #endregion
    }
}
