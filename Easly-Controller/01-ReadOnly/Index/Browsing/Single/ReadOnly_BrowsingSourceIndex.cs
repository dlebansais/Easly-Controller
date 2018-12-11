using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingSourceIndex : IReadOnlyBrowsingChildNodeIndex
    {
    }

    public class ReadOnlyBrowsingSourceIndex : ReadOnlyBrowsingChildNodeIndex, IReadOnlyBrowsingSourceIndex
    {
        #region Init
        public ReadOnlyBrowsingSourceIndex(IBlock block, IIdentifier sourceIdentifier)
            : base(sourceIdentifier, nameof(IBlock.SourceIdentifier))
        {
            Debug.Assert(NodeTreeHelper.IsSourceNode(block, sourceIdentifier));
        }
        #endregion
    }
}
