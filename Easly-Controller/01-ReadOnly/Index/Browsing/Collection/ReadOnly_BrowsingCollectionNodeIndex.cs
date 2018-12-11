using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingCollectionNodeIndex : IReadOnlyBrowsingChildNodeIndex
    {
    }

    public abstract class ReadOnlyBrowsingCollectionNodeIndex : ReadOnlyBrowsingChildNodeIndex, IReadOnlyBrowsingCollectionNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingCollectionNodeIndex(INode node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
