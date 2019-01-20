using BaseNode;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface IFrameBrowsingPatternIndex : IWriteableBrowsingPatternIndex, IFrameBrowsingChildIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public class FrameBrowsingPatternIndex : WriteableBrowsingPatternIndex, IFrameBrowsingPatternIndex
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
        /// Compares two <see cref="IFrameIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameBrowsingPatternIndex AsBrowsingPatternIndex))
                return false;

            if (!base.IsEqual(comparer, AsBrowsingPatternIndex))
                return false;

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionPlaceholderNodeIndex object.
        /// </summary>
        protected override IWriteableInsertionPlaceholderNodeIndex CreateInsertionIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowsingPatternIndex));
            return new FrameInsertionPlaceholderNodeIndex(parentNode, PropertyName, node);
        }
        #endregion
    }
}
