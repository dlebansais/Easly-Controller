using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IWriteableSourceStateView : IReadOnlySourceStateView, IWriteablePlaceholderNodeStateView
    {
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
    }
}
