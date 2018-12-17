using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public interface IWriteableOptionalNodeStateView : IReadOnlyOptionalNodeStateView, IWriteableNodeStateView
    {
        /// <summary>
        /// The optional node state.
        /// </summary>
        new IWriteableOptionalNodeState State { get; }
    }

    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public class WriteableOptionalNodeStateView : ReadOnlyOptionalNodeStateView, IWriteableOptionalNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOptionalNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The optional node state.</param>
        public WriteableOptionalNodeStateView(IWriteableOptionalNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The optional node state.
        /// </summary>
        public new IWriteableOptionalNodeState State { get { return (IWriteableOptionalNodeState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        #endregion
    }
}
