using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingPlaceholderNodeIndex : IReadOnlyBrowsingChildNodeIndex
    {
    }

    public class ReadOnlyBrowsingPlaceholderNodeIndex : ReadOnlyBrowsingChildNodeIndex, IReadOnlyBrowsingPlaceholderNodeIndex 
    {
        #region Init
        public ReadOnlyBrowsingPlaceholderNodeIndex(INode parentNode, INode node, string propertyName)
            : base(node, propertyName)
        {
            Debug.Assert(NodeTreeHelper.IsChildNode(parentNode, propertyName, node));
        }
        #endregion
    }
}
