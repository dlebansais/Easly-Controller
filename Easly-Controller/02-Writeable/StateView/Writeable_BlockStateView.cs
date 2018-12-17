using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IWriteableBlockStateView : IReadOnlyBlockStateView
    {
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class WriteableBlockStateView : ReadOnlyBlockStateView, IWriteableBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBlockStateView"/> class.
        /// </summary>
        /// <param name="blockState">The block state.</param>
        public WriteableBlockStateView(IWriteableBlockState blockState)
            : base(blockState)
        {
        }
        #endregion
    }
}
