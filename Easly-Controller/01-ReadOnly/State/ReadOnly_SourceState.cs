using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlySourceState : IReadOnlyNodeState
    {
        IReadOnlyBlockState ParentBlockState { get; }
    }

    public class ReadOnlySourceState : ReadOnlyNodeState, IReadOnlySourceState
    {
        #region Init
        public ReadOnlySourceState(IReadOnlyBlockState parentBlockState, INode node)
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
