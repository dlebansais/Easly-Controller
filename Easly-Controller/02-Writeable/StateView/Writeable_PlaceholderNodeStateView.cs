using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a child node.
    /// </summary>
    public interface IWriteablePlaceholderNodeStateView : IReadOnlyPlaceholderNodeStateView, IWriteableNodeStateView
    {
        /// <summary>
        /// The child node.
        /// </summary>
        new IWriteablePlaceholderNodeState State { get; }
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

        #region Properties
        /// <summary>
        /// The child node.
        /// </summary>
        public new IWriteablePlaceholderNodeState State { get { return (IWriteablePlaceholderNodeState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        #endregion
    }
}
