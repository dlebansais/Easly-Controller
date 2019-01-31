namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for inserting the first node of a new block.
    /// </summary>
    public interface IFocusInsertionNewBlockNodeIndex : IFrameInsertionNewBlockNodeIndex, IFocusInsertionBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for inserting the first node of a new block.
    /// </summary>
    public class FocusInsertionNewBlockNodeIndex : FrameInsertionNewBlockNodeIndex, IFocusInsertionNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInsertionNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="patternNode">Replication pattern in the block.</param>
        /// <param name="sourceNode">Source identifier in the block.</param>
        public FocusInsertionNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
            : base(parentNode, propertyName, node, blockIndex, patternNode, sourceNode)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusInsertionNewBlockNodeIndex AsInsertionNewBlockNodeIndex))
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
        private protected override IWriteableBrowsingNewBlockNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertionNewBlockNodeIndex));
            return new FocusBrowsingNewBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex);
        }
        #endregion
    }
}
