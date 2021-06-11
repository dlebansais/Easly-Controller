namespace EaslyController.Layout
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public interface ILayoutInsertionOptionalClearIndex : IFocusInsertionOptionalClearIndex, ILayoutInsertionChildIndex
    {
    }

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public class LayoutInsertionOptionalClearIndex : FocusInsertionOptionalClearIndex, ILayoutInsertionOptionalClearIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertionOptionalClearIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public LayoutInsertionOptionalClearIndex(INode parentNode, string propertyName)
            : base(parentNode, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="LayoutInsertionOptionalClearIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutInsertionOptionalClearIndex AsInsertionOptionalClearIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionOptionalClearIndex))
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
            ControllerTools.AssertNoOverride(this, typeof(LayoutInsertionOptionalClearIndex));
            return new LayoutBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
