using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of a block state.
    /// </summary>
    public interface IReadOnlyBlockStateView
    {
        /// <summary>
        /// The block state.
        /// </summary>
        IReadOnlyBlockState BlockState { get; }
    }

    /// <summary>
    /// View of a block state.
    /// </summary>
    public class ReadOnlyBlockStateView : IReadOnlyBlockStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBlockStateView"/> class.
        /// </summary>
        /// <param name="blockState">The block state.</param>
        public ReadOnlyBlockStateView(IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The block state.
        /// </summary>
        public IReadOnlyBlockState BlockState { get; }
        #endregion
    }
}
