using BaseNode;
using BaseNodeHelper;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    public interface IReadOnlyBrowsingNewBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex
    {
        int BlockIndex { get; }
        IPattern PatternNode { get; }
        IIdentifier SourceNode { get; }
    }

    public class ReadOnlyBrowsingNewBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IReadOnlyBrowsingNewBlockNodeIndex
    {
        #region Init
        public ReadOnlyBrowsingNewBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
            : base(node, propertyName)
        {
            Debug.Assert(NodeTreeHelper.IsBlockChildNode(parentNode, propertyName, blockIndex, 0, node));
            Debug.Assert(NodeTreeHelper.IsBlockPatternNode(parentNode, propertyName, blockIndex, patternNode));
            Debug.Assert(NodeTreeHelper.IsBlockSourceNode(parentNode, propertyName, blockIndex, sourceNode));

            BlockIndex = blockIndex;
            PatternNode = patternNode;
            SourceNode = sourceNode;
        }
        #endregion

        #region Properties
        public int BlockIndex { get; private set; }
        public IPattern PatternNode { get; private set; }
        public IIdentifier SourceNode { get; private set; }
        #endregion
    }
}
