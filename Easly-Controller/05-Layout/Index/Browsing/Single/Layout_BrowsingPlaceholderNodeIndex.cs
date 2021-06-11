namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for a node.
    /// </summary>
    public interface ILayoutBrowsingPlaceholderNodeIndex : IFocusBrowsingPlaceholderNodeIndex, ILayoutBrowsingChildIndex, ILayoutBrowsingInsertableIndex, ILayoutNodeIndex
    {
    }

    /// <summary>
    /// Index for a node.
    /// </summary>
    internal class LayoutBrowsingPlaceholderNodeIndex : FocusBrowsingPlaceholderNodeIndex, ILayoutBrowsingPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed node.</param>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.</param>
        public LayoutBrowsingPlaceholderNodeIndex(INode parentNode, INode node, string propertyName)
            : base(parentNode, node, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutBrowsingPlaceholderNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutBrowsingPlaceholderNodeIndex AsBrowsingPlaceholderNodeIndex))
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
        private protected override IWriteableInsertionPlaceholderNodeIndex CreateInsertionIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBrowsingPlaceholderNodeIndex));
            return new LayoutInsertionPlaceholderNodeIndex(parentNode, PropertyName, node);
        }
        #endregion
    }
}
