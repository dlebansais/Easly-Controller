using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public interface IWriteablePatternStateView : IReadOnlyPatternStateView, IWriteablePlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IWriteablePatternState State { get; }
    }

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public class WriteablePatternStateView : ReadOnlyPatternStateView, IWriteablePatternStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteablePatternStateView"/> class.
        /// </summary>
        /// <param name="state">The pattern state.</param>
        public WriteablePatternStateView(IWriteablePatternState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IWriteablePatternState State { get { return (IWriteablePatternState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateView.State { get { return State; } }
        #endregion
    }
}
