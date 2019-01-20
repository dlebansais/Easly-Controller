using BaseNode;
using EaslyController.Frame;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Index for a node.
    /// </summary>
    public interface IFocusBrowsingPlaceholderNodeIndex : IFrameBrowsingPlaceholderNodeIndex, IFocusBrowsingChildIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Index for a node.
    /// </summary>
    public class FocusBrowsingPlaceholderNodeIndex : FrameBrowsingPlaceholderNodeIndex, IFocusBrowsingPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed node.</param>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.</param>
        public FocusBrowsingPlaceholderNodeIndex(INode parentNode, INode node, string propertyName)
            : base(parentNode, node, propertyName)
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

            if (!(other is IFocusBrowsingPlaceholderNodeIndex AsBrowsingPlaceholderNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsBrowsingPlaceholderNodeIndex))
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
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowsingPlaceholderNodeIndex));
            return new FocusInsertionPlaceholderNodeIndex(parentNode, PropertyName, node);
        }
        #endregion
    }
}
