namespace EaslyController.Layout
{
    using BaseNode;
    using Contracts;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public interface ILayoutInsertionListNodeIndex : IFocusInsertionListNodeIndex, ILayoutInsertionCollectionNodeIndex
    {
    }

    /// <summary>
    /// Index for inserting a node in a list of nodes.
    /// </summary>
    public class LayoutInsertionListNodeIndex : FocusInsertionListNodeIndex, ILayoutInsertionListNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertionListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="node">Inserted node.</param>
        /// <param name="index">Position where to insert <paramref name="node"/> in the list.</param>
        public LayoutInsertionListNodeIndex(Node parentNode, string propertyName, Node node, int index)
            : base(parentNode, propertyName, node, index)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutInsertionListNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out LayoutInsertionListNodeIndex AsInsertionListNodeIndex))
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
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutInsertionListNodeIndex>());
            return new LayoutBrowsingListNodeIndex(ParentNode, Node, PropertyName, Index);
        }
        #endregion
    }
}
