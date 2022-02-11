namespace EaslyController.ReadOnly
{
    using Contracts;

    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public class ReadOnlyOptionalNodeStateView : ReadOnlyNodeStateView, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyOptionalNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The optional node state.</param>
        public ReadOnlyOptionalNodeStateView(ReadOnlyControllerView controllerView, IReadOnlyOptionalNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The optional node state.
        /// </summary>
        public new IReadOnlyOptionalNodeState State { get { return (IReadOnlyOptionalNodeState)base.State; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyOptionalNodeStateView AsOptionalNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsOptionalNodeStateView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
