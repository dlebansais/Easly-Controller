using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingExistingBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex
    {
        int BlockIndex { get; }
        int Index { get; }
    }

    public class ReadOnlyBrowsingExistingBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IReadOnlyBrowsingExistingBlockNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingExistingBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, int index)
            : base(node, propertyName)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(NodeTreeHelper.IsBlockChildNode(parentNode, propertyName, blockIndex, index, node));

            BlockIndex = blockIndex;
            Index = index;
        }
        #endregion

        #region Properties
        public int BlockIndex { get; }
        public int Index { get; }
        #endregion
    }
}
