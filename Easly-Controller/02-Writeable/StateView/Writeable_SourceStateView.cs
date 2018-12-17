using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IWriteableSourceStateView : IReadOnlySourceStateView, IWriteablePlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IWriteableSourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    public class WriteableSourceStateView : ReadOnlySourceStateView, IWriteableSourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableSourceStateView"/> class.
        /// </summary>
        /// <param name="state">The source state.</param>
        public WriteableSourceStateView(IWriteableSourceState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IWriteableSourceState State { get { return (IWriteableSourceState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateView.State { get { return State; } }
        #endregion
    }
}
