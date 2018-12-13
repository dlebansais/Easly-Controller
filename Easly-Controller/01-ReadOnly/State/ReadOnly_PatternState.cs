using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyPatternState : IReadOnlyPlaceholderNodeState
    {
        new IPattern Node { get; }
        IReadOnlyBlockState ParentBlockState { get; }
        new IPattern CloneNode();
    }

    public class ReadOnlyPatternState : ReadOnlyPlaceholderNodeState, IReadOnlyPatternState
    {
        #region Init
        public ReadOnlyPatternState(IReadOnlyBlockState parentBlockState, IReadOnlyNodeIndex index)
            : base(index)
        {
            Debug.Assert(parentBlockState != null);

            ParentBlockState = parentBlockState;
        }
        #endregion

        #region Properties
        public IReadOnlyBlockState ParentBlockState { get; private set; }
        public new IPattern Node { get { return (IPattern)base.Node; } }
        #endregion

        #region Client Interface
        public new IPattern CloneNode() { return (IPattern)base.CloneNode(); }
        #endregion
    }
}
