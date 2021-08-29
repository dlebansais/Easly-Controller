namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for inserting the first node of a new block.
    /// </summary>
    public interface ILayoutInsertionNewBlockNodeIndex : IFocusInsertionNewBlockNodeIndex, ILayoutInsertionBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for inserting the first node of a new block.
    /// </summary>
    public class LayoutInsertionNewBlockNodeIndex : FocusInsertionNewBlockNodeIndex, ILayoutInsertionNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertionNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="patternNode">Replication pattern in the block.</param>
        /// <param name="sourceNode">Source identifier in the block.</param>
        public LayoutInsertionNewBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, Pattern patternNode, Identifier sourceNode)
            : base(parentNode, propertyName, node, blockIndex, patternNode, sourceNode)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutInsertionNewBlockNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutInsertionNewBlockNodeIndex AsInsertionNewBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionNewBlockNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex.
        /// </summary>
        private protected override IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutInsertionNewBlockNodeIndex));
            return new LayoutBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
