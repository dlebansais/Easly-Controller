using BaseNode;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyRootNodeIndex : IReadOnlyNodeIndex
    {
    }

    public class ReadOnlyRootNodeIndex : ReadOnlyNodeIndex, IReadOnlyRootNodeIndex
    {
        #region Init
        public ReadOnlyRootNodeIndex(INode node)
            : base(node)
        {
        }
        #endregion
    }
}
