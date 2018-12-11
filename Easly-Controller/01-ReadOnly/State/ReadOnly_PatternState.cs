using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyPatternState : IReadOnlyNodeState
    {
        IReadOnlyBlockState ParentBlockState { get; }
    }

    public class ReadOnlyPatternState : ReadOnlyNodeState, IReadOnlyPatternState
    {
        #region Init
        public ReadOnlyPatternState(IReadOnlyBlockState parentBlockState, INode node)
            : base(node)
        {
            Debug.Assert(parentBlockState != null);

            ParentBlockState = parentBlockState;
        }
        #endregion

        #region Properties
        public IReadOnlyBlockState ParentBlockState { get; private set; }
        #endregion
    }
}
