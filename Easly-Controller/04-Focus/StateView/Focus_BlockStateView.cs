namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using Contracts;
    using EaslyController.Frame;

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class FocusBlockStateView : FrameBlockStateView
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusBlockStateView"/> object.
        /// </summary>
        public static new FocusBlockStateView Empty { get; } = new FocusBlockStateView(FocusControllerView.Empty, FocusBlockState<IFocusInner<IFocusBrowsingChildIndex>>.Empty, FocusTemplate.Empty);

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        /// <param name="template">The frame template.</param>
        protected FocusBlockStateView(FocusControllerView controllerView, IFocusBlockState blockState, IFocusTemplate template)
            : base(controllerView, blockState, template)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public FocusBlockStateView(FocusControllerView controllerView, IFocusBlockState blockState)
            : base(controllerView, blockState)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new FocusControllerView ControllerView { get { return (FocusControllerView)base.ControllerView; } }

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
            base.BuildRootCellView(context);
        }

        /// <summary>
        /// Updates the focus chain with cells in the tree.
        /// </summary>
        /// <param name="focusChain">The list of focusable cell views found in the tree.</param>
        /// <param name="focusedNode">The currently focused node.</param>
        /// <param name="focusedFrame">The currently focused frame in the template associated to <paramref name="focusedNode"/>.</param>
        /// <param name="matchingFocus">The focus in <paramref name="focusChain"/> that match <paramref name="focusedNode"/> and <paramref name="focusedFrame"/> upon return.</param>
        public virtual void UpdateFocusChain(FocusFocusList focusChain, Node focusedNode, IFocusFrame focusedFrame, ref IFocusFocus matchingFocus)
        {
            Debug.Assert(RootCellView != null);

            RootCellView.UpdateFocusChain(focusChain, focusedNode, focusedFrame, ref matchingFocus);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusBlockStateView AsBlockStateView))
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
        private protected override FrameAssignableCellViewDictionary<string> CreateCellViewTable()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBlockStateView));
            return new FocusAssignableCellViewDictionary<string>();
        }
        #endregion
    }
}
