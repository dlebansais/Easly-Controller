using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingCollectionNodeIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex
    {
    }

    public abstract class ReadOnlyBrowsingCollectionNodeIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingCollectionNodeIndex(INode node, string propertyName)
        {
            Node = node;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public INode Node { get; private set; }
        public string PropertyName { get; private set; }
        #endregion
    }
}
