﻿namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface IFrameBrowsingPatternIndex : IWriteableBrowsingPatternIndex, IFrameBrowsingChildIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    internal class FrameBrowsingPatternIndex : WriteableBrowsingPatternIndex, IFrameBrowsingPatternIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingPatternIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed replication pattern node.</param>
        public FrameBrowsingPatternIndex(IBlock block)
            : base(block)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameBrowsingPatternIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FrameBrowsingPatternIndex AsBrowsingPatternIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingPatternIndex))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
