﻿namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using Contracts;

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
            Contract.RequireNotNull(controllerView, out ReadOnlyControllerView ControllerView);
            Contract.RequireNotNull(blockState, out IReadOnlyBlockState BlockState);

            this.ControllerView = ControllerView;
            this.BlockState = BlockState;
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
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyBlockStateView AsBlockStateView))
                return comparer.Failed();

            if (!comparer.IsSameReference(BlockState, AsBlockStateView.BlockState))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
