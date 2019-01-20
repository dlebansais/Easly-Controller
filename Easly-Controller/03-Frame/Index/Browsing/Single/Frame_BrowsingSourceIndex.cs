using BaseNode;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public interface IFrameBrowsingSourceIndex : IWriteableBrowsingSourceIndex, IFrameBrowsingChildIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public class FrameBrowsingSourceIndex : WriteableBrowsingSourceIndex, IFrameBrowsingSourceIndex
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
        /// Compares two <see cref="IFrameIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameBrowsingSourceIndex AsBrowsingSourceIndex))
                return false;

            if (!base.IsEqual(comparer, AsBrowsingSourceIndex))
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
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowsingSourceIndex));
            return new FrameInsertionPlaceholderNodeIndex(parentNode, PropertyName, node);
        }
        #endregion
    }
}
