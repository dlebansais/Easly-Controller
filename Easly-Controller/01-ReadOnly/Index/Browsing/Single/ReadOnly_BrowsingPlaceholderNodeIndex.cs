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
            Debug.Assert(parentNode != null);
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(NodeTreeHelper.IsChildNode(parentNode, propertyName, node));

            Node = node;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        public INode Node { get; }
        public string PropertyName { get; }
        #endregion
    }
}
