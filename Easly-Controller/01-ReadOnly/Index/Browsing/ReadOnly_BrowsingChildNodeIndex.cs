using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingChildNodeIndex : IReadOnlyChildNodeIndex
    {
    }

    public abstract class ReadOnlyBrowsingChildNodeIndex : ReadOnlyChildNodeIndex, IReadOnlyBrowsingChildNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingChildNodeIndex(INode node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
