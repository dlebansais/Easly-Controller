namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IFocusBrowsingListNodeIndex : IFrameBrowsingListNodeIndex, IFocusBrowsingCollectionNodeIndex, IFocusBrowsingInsertableIndex
    {
    }

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    internal class FocusBrowsingListNodeIndex : FrameBrowsingListNodeIndex, IFocusBrowsingListNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusBrowsingListNodeIndex"/> object.
        /// </summary>
        public static new FocusBrowsingListNodeIndex Empty { get; } = new FocusBrowsingListNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingListNodeIndex"/> class.
        /// </summary>
        protected FocusBrowsingListNodeIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingListNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the list.</param>
        /// <param name="node">Indexed node in the list</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the list.</param>
        /// <param name="index">Position of the node in the list.</param>
        public FocusBrowsingListNodeIndex(Node parentNode, Node node, string propertyName, int index)
            : base(parentNode, node, propertyName, index)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusBrowsingListNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FocusBrowsingListNodeIndex AsBrowsingListNodeIndex))
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
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusBrowsingListNodeIndex>());
            return new FocusInsertionListNodeIndex(parentNode, PropertyName, node, Index);
        }
        #endregion
    }
}
