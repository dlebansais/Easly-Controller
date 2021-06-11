namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public interface ILayoutInsertionOptionalNodeIndex : IFocusInsertionOptionalNodeIndex, ILayoutInsertionChildNodeIndex, ILayoutNodeIndex
    {
    }

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public class LayoutInsertionOptionalNodeIndex : FocusInsertionOptionalNodeIndex, ILayoutInsertionOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertionOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        /// <param name="node">The assigned node.</param>
        public LayoutInsertionOptionalNodeIndex(INode parentNode, string propertyName, INode node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutInsertionOptionalNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutInsertionOptionalNodeIndex AsInsertionOptionalNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionOptionalNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingOptionalNodeIndex object.
        /// </summary>
        private protected override IWriteableBrowsingOptionalNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(LayoutInsertionOptionalNodeIndex));
            return new LayoutBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
