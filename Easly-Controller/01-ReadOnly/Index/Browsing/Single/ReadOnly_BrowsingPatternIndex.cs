using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingPatternIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex
    {
        new IPattern Node { get; }
    }

    public class ReadOnlyBrowsingPatternIndex : IReadOnlyBrowsingPatternIndex
    {
        #region Init
        public ReadOnlyBrowsingPatternIndex(IBlock block)
        {
            Block = block;
        }
        #endregion

        #region Properties
        public IPattern Node { get { return Block.ReplicationPattern; } }
        INode IReadOnlyNodeIndex.Node { get { return Node; } }
        public string PropertyName { get { return nameof(IBlock.ReplicationPattern); } }
        private IBlock Block;
        #endregion
    }
}
