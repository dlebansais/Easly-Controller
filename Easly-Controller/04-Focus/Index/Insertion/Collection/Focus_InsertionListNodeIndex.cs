namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public interface IFocusInsertionListNodeIndex : IFrameInsertionListNodeIndex, IFocusInsertionCollectionNodeIndex
    {
    }

    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public class FocusInsertionListNodeIndex : FrameInsertionListNodeIndex, IFocusInsertionListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInsertionListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="node">Inserted node.</param>
        /// <param name="index">Position where to insert <paramref name="node"/> in the list.</param>
        public FocusInsertionListNodeIndex(INode parentNode, string propertyName, INode node, int index)
            : base(parentNode, propertyName, node, index)
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

            if (!(other is IFocusInsertionListNodeIndex AsInsertionListNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionListNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingListNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingListNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertionListNodeIndex));
            return new FocusBrowsingListNodeIndex(ParentNode, Node, PropertyName, Index);
        }
        #endregion
    }
}
