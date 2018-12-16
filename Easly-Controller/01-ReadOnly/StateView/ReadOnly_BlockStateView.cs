using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBlockStateView
    {
        IReadOnlyBlockState BlockState { get; }
    }

    public class ReadOnlyBlockStateView : IReadOnlyBlockStateView
    {
        #region Init
        public ReadOnlyBlockStateView(IReadOnlyBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }
        #endregion

        #region Properties
        public IReadOnlyBlockState BlockState { get; }
        #endregion
    }
}
