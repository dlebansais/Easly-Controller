using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingPatternIndex : IReadOnlyBrowsingChildNodeIndex
    {
    }

    public class ReadOnlyBrowsingPatternIndex : ReadOnlyBrowsingChildNodeIndex, IReadOnlyBrowsingPatternIndex
    {
        #region Init
        public ReadOnlyBrowsingPatternIndex(IBlock block, IPattern replicationPattern)
            : base(replicationPattern, nameof(IBlock.ReplicationPattern))
        {
            Debug.Assert(NodeTreeHelper.IsPatternNode(block, replicationPattern));
        }
        #endregion
    }
}
