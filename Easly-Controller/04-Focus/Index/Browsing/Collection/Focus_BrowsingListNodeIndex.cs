using BaseNode;
using EaslyController.Frame;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Focus
{
    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IFocusBrowsingListNodeIndex : IFrameBrowsingListNodeIndex, IFocusBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public class FocusBrowsingListNodeIndex : FrameBrowsingListNodeIndex, IFocusBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="node">Indexed node in the list</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="index">Position of the node in the list.</param>
        public FocusBrowsingListNodeIndex(INode parentNode, INode node, string propertyName, int index)
            : base(parentNode, node, propertyName, index)
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

            if (!(other is IFocusBrowsingListNodeIndex AsBrowsingListNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingListNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionListNodeIndex object.
        /// </summary>
        protected override IWriteableInsertionListNodeIndex CreateInsertionIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowsingListNodeIndex));
            return new FocusInsertionListNodeIndex(parentNode, PropertyName, node, Index);
        }
        #endregion
    }
}
