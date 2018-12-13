using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingSourceIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex
    {
        new IIdentifier Node { get; }
    }

    public class ReadOnlyBrowsingSourceIndex : IReadOnlyBrowsingSourceIndex
    {
        #region Init
        public ReadOnlyBrowsingSourceIndex(IBlock block)
        {
            Block = block;
        }
        #endregion

        #region Properties
        public IIdentifier Node { get { return Block.SourceIdentifier; } }
        INode IReadOnlyNodeIndex.Node { get { return Node; } }
        public string PropertyName { get { return nameof(IBlock.SourceIdentifier); } }
        private IBlock Block;
        #endregion
    }
}
