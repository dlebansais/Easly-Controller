using EaslyController.Writeable;

namespace EaslyController.Frame
{
    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IFrameSourceStateView : IWriteableSourceStateView, IFramePlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IFrameSourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    public class FrameSourceStateView : WriteableSourceStateView, IFrameSourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameSourceStateView"/> class.
        /// </summary>
        /// <param name="state">The source state.</param>
        public FrameSourceStateView(IFrameSourceState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IFrameSourceState State { get { return (IFrameSourceState)base.State; } }
        IFrameNodeState IFrameNodeStateView.State { get { return State; } }
        IFramePlaceholderNodeState IFramePlaceholderNodeStateView.State { get { return State; } }
        #endregion
    }
}
