﻿namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for a node.
    /// </summary>
    public interface IFrameBrowsingPlaceholderNodeIndex : IWriteableBrowsingPlaceholderNodeIndex, IFrameBrowsingChildIndex, IFrameBrowsingInsertableIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Index for a node.
    /// </summary>
    internal class FrameBrowsingPlaceholderNodeIndex : WriteableBrowsingPlaceholderNodeIndex, IFrameBrowsingPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed node.</param>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.</param>
        public FrameBrowsingPlaceholderNodeIndex(Node parentNode, Node node, string propertyName)
            : base(parentNode, node, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameBrowsingPlaceholderNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FrameBrowsingPlaceholderNodeIndex AsBrowsingPlaceholderNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingPlaceholderNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionPlaceholderNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionPlaceholderNodeIndex CreateInsertionIndex(Node parentNode, Node node)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBrowsingPlaceholderNodeIndex>());
            return new FrameInsertionPlaceholderNodeIndex(parentNode, PropertyName, node);
        }
        #endregion
    }
}
