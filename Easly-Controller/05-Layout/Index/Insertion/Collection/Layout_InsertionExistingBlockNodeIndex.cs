namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for inserting a node in an existing block of a block list.
    /// </summary>
    public interface ILayoutInsertionExistingBlockNodeIndex : IFocusInsertionExistingBlockNodeIndex, ILayoutInsertionBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for inserting a node in an existing block of a block list.
    /// </summary>
    public class LayoutInsertionExistingBlockNodeIndex : FocusInsertionExistingBlockNodeIndex, ILayoutInsertionExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertionExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list..</param>
        /// <param name="node">Inserted node.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position where to insert <paramref name="node"/> in the block.</param>
        public LayoutInsertionExistingBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, int index)
            : base(parentNode, propertyName, node, blockIndex, index)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutInsertionExistingBlockNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutInsertionExistingBlockNodeIndex AsInsertionExistingBlockNodeIndex))
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
            ControllerTools.AssertNoOverride(this, typeof(LayoutInsertionExistingBlockNodeIndex));
            return new LayoutBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, Index);
        }
        #endregion
    }
}
