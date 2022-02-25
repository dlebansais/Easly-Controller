namespace EaslyController.Layout
{
    using BaseNode;
    using Contracts;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public interface ILayoutInsertionPlaceholderNodeIndex : IFocusInsertionPlaceholderNodeIndex, ILayoutInsertionChildNodeIndex, ILayoutNodeIndex
    {
    }

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public class LayoutInsertionPlaceholderNodeIndex : FocusInsertionPlaceholderNodeIndex, ILayoutInsertionPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertionPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the replaced node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.</param>
        /// <param name="node">The assigned node.</param>
        public LayoutInsertionPlaceholderNodeIndex(Node parentNode, string propertyName, Node node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutInsertionPlaceholderNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out LayoutInsertionPlaceholderNodeIndex AsInsertionPlaceholderNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionPlaceholderNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingPlaceholderNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutInsertionPlaceholderNodeIndex>());
            return new LayoutBrowsingPlaceholderNodeIndex(ParentNode, Node, PropertyName);
        }
        #endregion
    }
}
