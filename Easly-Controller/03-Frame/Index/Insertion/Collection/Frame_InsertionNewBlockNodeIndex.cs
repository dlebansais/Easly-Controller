using BaseNode;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Index for inserting the first node of a new block.
    /// </summary>
    public interface IFrameInsertionNewBlockNodeIndex : IWriteableInsertionNewBlockNodeIndex, IFrameInsertionBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for inserting the first node of a new block.
    /// </summary>
    public class FrameInsertionNewBlockNodeIndex : WriteableInsertionNewBlockNodeIndex, IFrameInsertionNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="patternNode">Replication pattern in the block.</param>
        /// <param name="sourceNode">Source identifier in the block.</param>
        public FrameInsertionNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
            : base(parentNode, propertyName, node, blockIndex, patternNode, sourceNode)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameInsertionNewBlockNodeIndex AsInsertionNewBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionNewBlockNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        protected override IWriteableBrowsingNewBlockNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameInsertionNewBlockNodeIndex));
            return new FrameBrowsingNewBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, PatternNode, SourceNode);
        }
        #endregion
    }
}
