using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingSourceIndex : IReadOnlyBrowsingChildNodeIndex
    {
        new IIdentifier Node { get; }
    }

    public class ReadOnlyBrowsingSourceIndex : ReadOnlyBrowsingChildNodeIndex, IReadOnlyBrowsingSourceIndex
    {
        #region Init
        public ReadOnlyBrowsingSourceIndex(IBlock block)
            : base(block.SourceIdentifier, nameof(IBlock.SourceIdentifier))
        {
        }
        #endregion

        #region Properties
        public new IIdentifier Node { get { return (IIdentifier)base.Node; } }
        #endregion
    }
}
