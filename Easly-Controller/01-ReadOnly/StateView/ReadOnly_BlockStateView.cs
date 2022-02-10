namespace EaslyController.ReadOnly
{
    using System.Diagnostics;

    /// <summary>
    /// View of a block state.
    /// </summary>
    [DebuggerDisplay("View of: {BlockState}")]
    public class ReadOnlyBlockStateView : IEqualComparable
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="ReadOnlyBlockStateView"/> object.
        /// </summary>
        public static ReadOnlyBlockStateView Empty { get; } = new ReadOnlyBlockStateView();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBlockStateView"/> class.
        /// </summary>
        protected ReadOnlyBlockStateView()
            : this(ReadOnlyControllerView.Empty, ReadOnlyBlockState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public ReadOnlyBlockStateView(ReadOnlyControllerView controllerView, IReadOnlyBlockState blockState)
        {
            Debug.Assert(controllerView != null);
            Debug.Assert(blockState != null);

            ControllerView = controllerView;
            BlockState = blockState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public ReadOnlyControllerView ControllerView { get; }

        /// <summary>
        /// The block state.
        /// </summary>
        public IReadOnlyBlockState BlockState { get; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBlockStateView AsBlockStateView))
                return comparer.Failed();

            if (!comparer.IsSameReference(BlockState, AsBlockStateView.BlockState))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
