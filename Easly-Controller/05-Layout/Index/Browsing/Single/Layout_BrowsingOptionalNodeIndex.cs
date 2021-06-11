namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public interface ILayoutBrowsingOptionalNodeIndex : IFocusBrowsingOptionalNodeIndex, ILayoutBrowsingChildIndex, ILayoutBrowsingInsertableIndex
    {
    }

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    internal class LayoutBrowsingOptionalNodeIndex : FocusBrowsingOptionalNodeIndex, ILayoutBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public LayoutBrowsingOptionalNodeIndex(INode parentNode, string propertyName)
            : base(parentNode, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutBrowsingOptionalNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutBrowsingOptionalNodeIndex AsBrowsingOptionalNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingOptionalNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionPlaceholderNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalNodeIndex CreateInsertionOptionalNodeIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBrowsingOptionalNodeIndex));
            return new LayoutInsertionOptionalNodeIndex(parentNode, PropertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertionOptionalClearIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalClearIndex CreateInsertionOptionalClearIndex(INode parentNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutBrowsingOptionalNodeIndex));
            return new LayoutInsertionOptionalClearIndex(parentNode, PropertyName);
        }
        #endregion
    }
}
