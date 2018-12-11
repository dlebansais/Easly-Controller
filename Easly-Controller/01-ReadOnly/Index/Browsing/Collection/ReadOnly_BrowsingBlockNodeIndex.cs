using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingBlockNodeIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
    }

    public abstract class ReadOnlyBrowsingBlockNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingBlockNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingBlockNodeIndex(INode node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
