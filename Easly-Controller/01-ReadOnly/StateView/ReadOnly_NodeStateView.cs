namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using Contracts;

    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IReadOnlyNodeStateView : IEqualComparable
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        ReadOnlyControllerView ControllerView { get; }

        /// <summary>
        /// The node state.
        /// </summary>
        IReadOnlyNodeState State { get; }
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    [DebuggerDisplay("View of: {State}")]
    public abstract class ReadOnlyNodeStateView : IReadOnlyNodeStateView, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="ReadOnlyNodeStateView"/> object.
        /// </summary>
        public static ReadOnlyNodeStateView Empty { get; } = new ReadOnlyEmptyNodeStateView();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        public ReadOnlyNodeStateView(ReadOnlyControllerView controllerView, IReadOnlyNodeState state)
        {
            Contract.RequireNotNull(controllerView, out ReadOnlyControllerView ControllerView);
            Contract.RequireNotNull(state, out IReadOnlyNodeState State);

            this.ControllerView = ControllerView;
            this.State = State;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public ReadOnlyControllerView ControllerView { get; }

        /// <summary>
        /// The node state.
        /// </summary>
        public IReadOnlyNodeState State { get; }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyNodeStateView AsNodeStateView))
                return comparer.Failed();

            if (!comparer.IsSameReference(State, AsNodeStateView.State))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
