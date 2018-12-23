using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IReadOnlyNodeStateView
    {
        /// <summary>
        /// The node state.
        /// </summary>
        IReadOnlyNodeState State { get; }

        /// <summary>
        /// Compares two <see cref="IReadOnlyNodeStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        bool IsEqual(IReadOnlyNodeStateView other);
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    public class ReadOnlyNodeStateView : IReadOnlyNodeStateView
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
        public virtual bool IsEqual(IReadOnlyNodeStateView other)
        {
            if (State != other.State)
                return false;

            return true;
        }
        #endregion
    }
}
