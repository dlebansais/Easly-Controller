using BaseNode;
using EaslyController.Frame;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public interface IFocusBrowsingSourceIndex : IFrameBrowsingSourceIndex, IFocusBrowsingChildIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public class FocusBrowsingSourceIndex : FrameBrowsingSourceIndex, IFocusBrowsingSourceIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingSourceIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed source identifier node.</param>
        public FocusBrowsingSourceIndex(IBlock block)
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

            if (!(other is IFocusBrowsingSourceIndex AsBrowsingSourceIndex))
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
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowsingSourceIndex));
            return new FocusInsertionPlaceholderNodeIndex(parentNode, PropertyName, node);
        }
        #endregion
    }
}
