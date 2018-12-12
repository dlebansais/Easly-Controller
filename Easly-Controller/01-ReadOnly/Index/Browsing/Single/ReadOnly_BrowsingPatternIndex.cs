using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingPatternIndex : IReadOnlyBrowsingChildNodeIndex
    {
        new IPattern Node { get; }
    }

    public class ReadOnlyBrowsingPatternIndex : ReadOnlyBrowsingChildNodeIndex, IReadOnlyBrowsingPatternIndex
    {
        #region Init
        public ReadOnlyBrowsingPatternIndex(IBlock block)
            : base(block.ReplicationPattern, nameof(IBlock.ReplicationPattern))
        {
        }
        #endregion

        #region Properties
        public new IPattern Node { get { return (IPattern)base.Node; } }
        #endregion
    }
}
