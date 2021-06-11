namespace EaslyController.ReadOnly
{
    using System.Diagnostics;

    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IReadOnlyBlockStateView : IEqualComparable
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        IReadOnlyControllerView ControllerView { get; }

        /// <summary>
        /// The block state.
        /// </summary>
        IReadOnlyBlockState BlockState { get; }
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    internal class ReadOnlyBlockStateView : IReadOnlyBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBlockStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="blockState">The block state.</param>
        public ReadOnlyBlockStateView(IReadOnlyControllerView controllerView, IReadOnlyBlockState blockState)
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
        public IReadOnlyControllerView ControllerView { get; }

        /// <summary>
        /// The block state.
        /// </summary>
        public IReadOnlyBlockState BlockState { get; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyBlockStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBlockStateView AsBlockStateView))
                return comparer.Failed();

            if (!comparer.IsSameReference(BlockState, AsBlockStateView.BlockState))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"View of: {BlockState}";
        }
        #endregion
    }
}
