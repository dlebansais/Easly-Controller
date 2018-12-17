namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public interface IReadOnlyOptionalNodeStateView : IReadOnlyNodeStateView
    {
        /// <summary>
        /// The optional node state.
        /// </summary>
        new IReadOnlyOptionalNodeState State { get; }
    }

    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public class ReadOnlyOptionalNodeStateView : ReadOnlyNodeStateView, IReadOnlyOptionalNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyOptionalNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The optional node state.</param>
        public ReadOnlyOptionalNodeStateView(IReadOnlyOptionalNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The optional node state.
        /// </summary>
        public new IReadOnlyOptionalNodeState State { get { return (IReadOnlyOptionalNodeState)base.State; } }
        #endregion
    }
}
