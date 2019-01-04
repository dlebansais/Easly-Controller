using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IReadOnlyNodeStateView : IEqualComparable
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        IReadOnlyControllerView ControllerView { get; }

        /// <summary>
        /// The node state.
        /// </summary>
        IReadOnlyNodeState State { get; }
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    public abstract class ReadOnlyNodeStateView : IReadOnlyNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        public ReadOnlyNodeStateView(IReadOnlyControllerView controllerView, IReadOnlyNodeState state)
        {
            Debug.Assert(controllerView != null);
            Debug.Assert(state != null);

            ControllerView = controllerView;
            State = state;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public IReadOnlyControllerView ControllerView { get; }

        /// <summary>
        /// The node state.
        /// </summary>
        public IReadOnlyNodeState State { get; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyNodeStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyNodeStateView AsNodeStateView))
                return false;

            if (State != AsNodeStateView.State)
                return false;

            return true;
        }
        #endregion
    }
}
