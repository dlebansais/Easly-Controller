using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingOptionalNodeIndex : IReadOnlyBrowsingChildNodeIndex
    {
    }

    public class ReadOnlyBrowsingOptionalNodeIndex : ReadOnlyBrowsingChildNodeIndex, IReadOnlyBrowsingOptionalNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingOptionalNodeIndex(INode parentNode, INode node, string propertyName)
            : base(node, propertyName)
        {
            Debug.Assert(NodeTreeHelper.IsOptionalChildNode(parentNode, propertyName, node));
        }
        #endregion
    }
}
