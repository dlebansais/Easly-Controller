namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public interface ILayoutBrowsingSourceIndex : IFocusBrowsingSourceIndex, ILayoutBrowsingChildIndex, ILayoutNodeIndex
    {
    }

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    internal class LayoutBrowsingSourceIndex : FocusBrowsingSourceIndex, ILayoutBrowsingSourceIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingSourceIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed source identifier node.</param>
        public LayoutBrowsingSourceIndex(IBlock block)
            : base(block)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ILayoutIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutBrowsingSourceIndex AsBrowsingSourceIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingSourceIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
