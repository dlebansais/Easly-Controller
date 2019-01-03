using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IReadOnlyNodeStateView : IEqualComparable
    {
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
        /// <param name="state">The node state.</param>
        public ReadOnlyNodeStateView(IReadOnlyNodeState state)
        {
            Debug.Assert(state != null);

            State = state;
        }
        #endregion

        #region Properties
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
