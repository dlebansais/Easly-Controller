using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyPatternState : IReadOnlyNodeState
    {
        new IPattern Node { get; }
        IReadOnlyBlockState ParentBlockState { get; }
    }

    public class ReadOnlyPatternState : ReadOnlyNodeState, IReadOnlyPatternState
    {
        #region Init
        public ReadOnlyPatternState(IReadOnlyBlockState parentBlockState, IPattern node)
            : base(node)
        {
            Debug.Assert(parentBlockState != null);

            ParentBlockState = parentBlockState;
        }
        #endregion

        #region Properties
        public IReadOnlyBlockState ParentBlockState { get; private set; }
        public new IPattern Node { get { return (IPattern)base.Node; } }
        #endregion
    }
}
