namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public interface IFrameBrowsingSourceIndex : IWriteableBrowsingSourceIndex, IFrameBrowsingChildIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    internal class FrameBrowsingSourceIndex : WriteableBrowsingSourceIndex, IFrameBrowsingSourceIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingSourceIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed source identifier node.</param>
        public FrameBrowsingSourceIndex(IBlock block)
            : base(block)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameBrowsingSourceIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FrameBrowsingSourceIndex AsBrowsingSourceIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingSourceIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
