namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface ILayoutBrowsingListNodeIndex : IFocusBrowsingListNodeIndex, ILayoutBrowsingCollectionNodeIndex, ILayoutBrowsingInsertableIndex
    {
    }

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    internal class LayoutBrowsingListNodeIndex : FocusBrowsingListNodeIndex, ILayoutBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutBrowsingListNodeIndex"/> object.
        /// </summary>
        public static new LayoutBrowsingListNodeIndex Empty { get; } = new LayoutBrowsingListNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingListNodeIndex"/> class.
        /// </summary>
        protected LayoutBrowsingListNodeIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="node">Indexed node in the list</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="index">Position of the node in the list.</param>
        public LayoutBrowsingListNodeIndex(Node parentNode, Node node, string propertyName, int index)
            : base(parentNode, node, propertyName, index)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutBrowsingListNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out LayoutBrowsingListNodeIndex AsBrowsingListNodeIndex))
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
        private protected override IWriteableInsertionListNodeIndex CreateInsertionIndex(Node parentNode, Node node)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutBrowsingListNodeIndex>());
            return new LayoutInsertionListNodeIndex(parentNode, PropertyName, node, Index);
        }
        #endregion
    }
}
