using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyNodeIndex : INodeLink
    {
    }

    public abstract class ReadOnlyNodeIndex : NodeLink, IReadOnlyNodeIndex
    {
        #region Init
        public ReadOnlyNodeIndex(INode node)
            : base(node)
        {
        }
        #endregion
    }
}
