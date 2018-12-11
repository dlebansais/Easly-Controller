using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingListNodeIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        int Index { get; }
    }

    public class ReadOnlyBrowsingListNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingListNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingListNodeIndex(INode parentNode, INode node, string propertyName, int index)
            : base(node, propertyName)
        {
            Debug.Assert(NodeTreeHelper.IsListChildNode(parentNode, propertyName, index, node));

            Index = index;
        }
        #endregion

        #region Properties
        public int Index { get; private set; }
        #endregion
    }
}
