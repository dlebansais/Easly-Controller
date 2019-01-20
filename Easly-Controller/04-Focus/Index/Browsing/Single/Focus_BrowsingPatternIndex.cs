using BaseNode;
using EaslyController.Frame;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface IFocusBrowsingPatternIndex : IFrameBrowsingPatternIndex, IFocusBrowsingChildIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public class FocusBrowsingPatternIndex : FrameBrowsingPatternIndex, IFocusBrowsingPatternIndex
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
        /// Compares two <see cref="IFocusIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusBrowsingPatternIndex AsBrowsingPatternIndex))
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
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowsingPatternIndex));
            return new FocusInsertionPlaceholderNodeIndex(parentNode, PropertyName, node);
        }
        #endregion
    }
}
