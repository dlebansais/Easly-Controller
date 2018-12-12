using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlySourceState : IReadOnlyNodeState
    {
        new IIdentifier Node { get; }
        IReadOnlyBlockState ParentBlockState { get; }
    }

    public class ReadOnlySourceState : ReadOnlyNodeState, IReadOnlySourceState
    {
        #region Init
        public ReadOnlySourceState(IReadOnlyBlockState parentBlockState, IIdentifier node)
            : base(node)
        {
            Debug.Assert(parentBlockState != null);

            ParentBlockState = parentBlockState;
        }
        #endregion

        #region Properties
        public IReadOnlyBlockState ParentBlockState { get; private set; }
        public new IIdentifier Node { get { return (IIdentifier)base.Node; } }
        #endregion
    }
}
