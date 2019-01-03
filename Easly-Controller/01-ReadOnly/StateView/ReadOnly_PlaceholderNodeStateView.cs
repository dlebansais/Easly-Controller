using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of a child node.
    /// </summary>
    public interface IReadOnlyPlaceholderNodeStateView : IReadOnlyNodeStateView
    {
        /// <summary>
        /// The child node.
        /// </summary>
        new IReadOnlyPlaceholderNodeState State { get; }
    }

    /// <summary>
    /// View of a child node.
    /// </summary>
    public class ReadOnlyPlaceholderNodeStateView : ReadOnlyNodeStateView, IReadOnlyPlaceholderNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPlaceholderNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The child node state.</param>
        public ReadOnlyPlaceholderNodeStateView(IReadOnlyPlaceholderNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The child node.
        /// </summary>
        public new IReadOnlyPlaceholderNodeState State { get { return (IReadOnlyPlaceholderNodeState)base.State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyPlaceholderNodeStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyPlaceholderNodeStateView AsPlaceholderNodeStateView))
                return false;

            if (!base.IsEqual(comparer, AsPlaceholderNodeStateView))
                return false;

            return true;
        }
        #endregion
    }
}
