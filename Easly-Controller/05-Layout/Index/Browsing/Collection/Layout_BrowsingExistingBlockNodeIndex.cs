﻿namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public interface ILayoutBrowsingExistingBlockNodeIndex : IFocusBrowsingExistingBlockNodeIndex, ILayoutBrowsingBlockNodeIndex, ILayoutBrowsingInsertableIndex
    {
    }

    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    internal class LayoutBrowsingExistingBlockNodeIndex : FocusBrowsingExistingBlockNodeIndex, ILayoutBrowsingExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">Indexed node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        public LayoutBrowsingExistingBlockNodeIndex(Node parentNode, Node node, string propertyName, int blockIndex, int index)
            : base(parentNode, node, propertyName, blockIndex, index)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutBrowsingExistingBlockNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out LayoutBrowsingExistingBlockNodeIndex AsBrowsingExistingBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingExistingBlockNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionExistingBlockNodeIndex CreateInsertionIndex(Node parentNode, Node node)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBrowsingExistingBlockNodeIndex>());
            return new LayoutInsertionExistingBlockNodeIndex(parentNode, PropertyName, node, BlockIndex, Index);
        }
        #endregion
    }
}
