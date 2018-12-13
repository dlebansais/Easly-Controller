using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyPlaceholderNodeState : IReadOnlyNodeState
    {
        new IReadOnlyNodeIndex ParentIndex { get; }
    }

    public class ReadOnlyPlaceholderNodeState : ReadOnlyNodeState, IReadOnlyPlaceholderNodeState
    {
        #region Init
        public ReadOnlyPlaceholderNodeState(IReadOnlyNodeIndex nodeIndex)
            : base(nodeIndex)
        {
        }
        #endregion

        #region Properties
        public override INode Node { get { return ParentIndex.Node; } }
        public new IReadOnlyNodeIndex ParentIndex { get { return (IReadOnlyNodeIndex)base.ParentIndex; } }
        #endregion
    }
}
