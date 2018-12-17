using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IWriteableNodeStateView : IReadOnlyNodeStateView
    {
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    public class WriteableNodeStateView : ReadOnlyNodeStateView, IWriteableNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The node state.</param>
        public WriteableNodeStateView(IWriteableNodeState state)
            : base(state)
        {
        }
        #endregion
    }
}
