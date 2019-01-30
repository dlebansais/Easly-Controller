namespace EaslyController.ReadOnly
{
    using System.Diagnostics;

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
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The child node state.</param>
        public ReadOnlyPlaceholderNodeStateView(IReadOnlyControllerView controllerView, IReadOnlyPlaceholderNodeState state)
            : base(controllerView, state)
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
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyPlaceholderNodeStateView AsPlaceholderNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPlaceholderNodeStateView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
