namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface IFocusBrowsingPatternIndex : IFrameBrowsingPatternIndex, IFocusBrowsingChildIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    internal class FocusBrowsingPatternIndex : FrameBrowsingPatternIndex, IFocusBrowsingPatternIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingPatternIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed replication pattern node.</param>
        public FocusBrowsingPatternIndex(IBlock block)
            : base(block)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusBrowsingPatternIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusBrowsingPatternIndex AsBrowsingPatternIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingPatternIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
