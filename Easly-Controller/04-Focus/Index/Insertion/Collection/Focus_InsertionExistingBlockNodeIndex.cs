namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for inserting a node in an existing block of a block list.
    /// </summary>
    public interface IFocusInsertionExistingBlockNodeIndex : IFrameInsertionExistingBlockNodeIndex, IFocusInsertionBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for inserting a node in an existing block of a block list.
    /// </summary>
    public class FocusInsertionExistingBlockNodeIndex : FrameInsertionExistingBlockNodeIndex, IFocusInsertionExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInsertionExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list..</param>
        /// <param name="node">Inserted node.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position where to insert <paramref name="node"/> in the block.</param>
        public FocusInsertionExistingBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, int index)
            : base(parentNode, propertyName, node, blockIndex, index)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusInsertionExistingBlockNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusInsertionExistingBlockNodeIndex AsInsertionExistingBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionExistingBlockNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingExistingBlockNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertionExistingBlockNodeIndex));
            return new FocusBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, Index);
        }
        #endregion
    }
}
