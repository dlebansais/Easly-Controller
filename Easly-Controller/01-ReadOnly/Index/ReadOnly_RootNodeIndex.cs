using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyRootNodeIndex : IReadOnlyNodeIndex
    {
    }

    public class ReadOnlyRootNodeIndex : IReadOnlyRootNodeIndex
    {
        #region Init
        public ReadOnlyRootNodeIndex(INode node)
        {
            Debug.Assert(node != null);

            Node = node;
        }
        #endregion

        #region Properties
        public INode Node { get; private set; }
        #endregion
    }
}
