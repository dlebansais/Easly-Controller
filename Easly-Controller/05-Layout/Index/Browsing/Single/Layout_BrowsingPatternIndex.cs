namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface ILayoutBrowsingPatternIndex : IFocusBrowsingPatternIndex, ILayoutBrowsingChildIndex, ILayoutNodeIndex
    {
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    internal class LayoutBrowsingPatternIndex : FocusBrowsingPatternIndex, ILayoutBrowsingPatternIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingPatternIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed replication pattern node.</param>
        public LayoutBrowsingPatternIndex(IBlock block)
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

            if (!comparer.IsSameType(other, out LayoutBrowsingPatternIndex AsBrowsingPatternIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingPatternIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
