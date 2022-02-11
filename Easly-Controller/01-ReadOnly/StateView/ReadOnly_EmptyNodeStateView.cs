namespace EaslyController.ReadOnly
{
    using Contracts;

    /// <summary>
    /// View of a child node.
    /// </summary>
    public class ReadOnlyEmptyNodeStateView : ReadOnlyNodeStateView, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyEmptyNodeStateView"/> class.
        /// </summary>
        public ReadOnlyEmptyNodeStateView()
            : this(ReadOnlyControllerView.Empty, ReadOnlyNodeState<IReadOnlyInner<IReadOnlyBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyEmptyNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        protected ReadOnlyEmptyNodeStateView(ReadOnlyControllerView controllerView, IReadOnlyNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The child node.
        /// </summary>
        public new IReadOnlyEmptyNodeState State { get { return (IReadOnlyEmptyNodeState)base.State; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyEmptyNodeStateView AsEmptyNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsEmptyNodeStateView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
