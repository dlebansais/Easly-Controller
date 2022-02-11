namespace EaslyController.Layout
{
    using Contracts;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public interface ILayoutInsertionEmptyNodeIndex : IFocusInsertionEmptyNodeIndex, ILayoutInsertionChildNodeIndex, ILayoutNodeIndex
    {
    }

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public class LayoutInsertionEmptyNodeIndex : FocusInsertionEmptyNodeIndex, ILayoutInsertionEmptyNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutInsertionEmptyNodeIndex"/> object.
        /// </summary>
        public static new LayoutInsertionEmptyNodeIndex Empty { get; } = new LayoutInsertionEmptyNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertionEmptyNodeIndex"/> class.
        /// </summary>
        public LayoutInsertionEmptyNodeIndex()
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutInsertionEmptyNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out LayoutInsertionEmptyNodeIndex AsInsertionEmptyNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionEmptyNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingEmptyNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutInsertionEmptyNodeIndex));
            return new LayoutBrowsingPlaceholderNodeIndex(ParentNode, Node, PropertyName);
        }
        #endregion
    }
}
