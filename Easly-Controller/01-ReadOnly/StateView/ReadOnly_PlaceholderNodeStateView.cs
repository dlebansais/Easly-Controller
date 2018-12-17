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
    }
}
