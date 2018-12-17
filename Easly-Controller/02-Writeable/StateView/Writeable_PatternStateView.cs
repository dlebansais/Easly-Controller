using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public interface IWriteablePatternStateView : IReadOnlyPatternStateView, IWriteablePlaceholderNodeStateView
    {
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
    }
}
