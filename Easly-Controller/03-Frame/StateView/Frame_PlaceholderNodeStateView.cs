using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a child node.
    /// </summary>
    public interface IFramePlaceholderNodeStateView : IWriteablePlaceholderNodeStateView, IFrameNodeStateView
    {
        /// <summary>
        /// The child node.
        /// </summary>
        new IFramePlaceholderNodeState State { get; }
    }

    /// <summary>
    /// View of a child node.
    /// </summary>
    public class FramePlaceholderNodeStateView : WriteablePlaceholderNodeStateView, IFramePlaceholderNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FramePlaceholderNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The child node state.</param>
        public FramePlaceholderNodeStateView(IFramePlaceholderNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The child node.
        /// </summary>
        public new IFramePlaceholderNodeState State { get { return (IFramePlaceholderNodeState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }
        #endregion
    }
}
