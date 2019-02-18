namespace EaslyController.Focus
{
    using System.Diagnostics;
    using EaslyController.Frame;

    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IFocusBlockStateView : IFrameBlockStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new IFocusControllerView ControllerView { get; }

        /// <summary>
        /// The block state.
        /// </summary>
        new IFocusBlockState BlockState { get; }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        new IFocusTemplate Template { get; }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        new IFocusCellView RootCellView { get; }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        new IFocusCellViewCollection EmbeddingCellView { get; }

        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        void UpdateFocusChain(IFocusFocusList focusChain);
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    internal class FocusBlockStateView : FrameBlockStateView, IFocusBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public FocusBlockStateView(IFocusControllerView controllerView, IFocusBlockState blockState)
            : base(controllerView, blockState)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IFocusControllerView ControllerView { get { return (IFocusControllerView)base.ControllerView; } }

        /// <summary>
        /// The block state.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }

        /// <summary>
        /// The template used to display the block state.
        /// </summary>
        public new IFocusTemplate Template { get { return (IFocusTemplate)base.Template; } }

        /// <summary>
        /// Root cell for the view.
        /// </summary>
        public new IFocusCellView RootCellView { get { return (IFocusCellView)base.RootCellView; } }

        /// <summary>
        /// List of cell views for each child node.
        /// </summary>
        public new IFocusCellViewCollection EmbeddingCellView { get { return (IFocusCellViewCollection)base.EmbeddingCellView; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Builds the cell view tree for this view.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        public override void BuildRootCellView(IFrameCellViewTreeContext context)
        {
            if (((IFocusCellViewTreeContext)context).IsVisible)
                base.BuildRootCellView(context);
            else
                SetRootCellView(CreateEmptyCellView(((IFocusCellViewTreeContext)context).StateView, null));
        }

        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        public virtual void UpdateFocusChain(IFocusFocusList focusChain)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateFocusChain(focusChain);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusBlockStateView AsBlockStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBlockStateView))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxAssignableCellViewDictionary{string} object.
        /// </summary>
        private protected override IFrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockStateView));
            return new FocusAssignableCellViewDictionary<string>();
        }

        /// <summary>
        /// Creates a IxxxEmptyCellView object.
        /// </summary>
        private protected virtual IFocusEmptyCellView CreateEmptyCellView(IFocusNodeStateView stateView, IFocusCellViewCollection parentCellView)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockStateView));
            return new FocusEmptyCellView(stateView, parentCellView);
        }
        #endregion
    }
}
