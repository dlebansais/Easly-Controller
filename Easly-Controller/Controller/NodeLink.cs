using BaseNode;
using System.Diagnostics;

namespace EaslyController
{
    public interface INodeLink
    {
        INode Node { get; }
    }

    public abstract class NodeLink : INodeLink
    {
        #region Init
        public NodeLink(INode Node)
        {
            Debug.Assert(Node != null);

            this.Node = Node;
        }
        #endregion

        #region Properties
        public INode Node { get; private set; }
        #endregion
    }
}
