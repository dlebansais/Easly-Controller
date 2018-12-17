using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a child node.
    /// </summary>
    public interface IWriteablePlaceholderNodeStateView : IReadOnlyPlaceholderNodeStateView, IWriteableNodeStateView
    {
    }

    /// <summary>
    /// View of a child node.
    /// </summary>
    public class WriteablePlaceholderNodeStateView : ReadOnlyPlaceholderNodeStateView, IWriteablePlaceholderNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteablePlaceholderNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The child node state.</param>
        public WriteablePlaceholderNodeStateView(IWriteablePlaceholderNodeState state)
            : base(state)
        {
        }
        #endregion
    }
}
