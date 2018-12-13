using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingPlaceholderNodeIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex
    {
    }

    public class ReadOnlyBrowsingPlaceholderNodeIndex : IReadOnlyBrowsingPlaceholderNodeIndex 
    {
        #region Init
        public ReadOnlyBrowsingPlaceholderNodeIndex(INode parentNode, INode node, string propertyName)
        {
            Debug.Assert(NodeTreeHelper.IsChildNode(parentNode, propertyName, node));

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
