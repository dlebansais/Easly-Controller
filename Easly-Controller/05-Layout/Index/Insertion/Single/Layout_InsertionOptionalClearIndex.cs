namespace EaslyController.Layout
{
    using BaseNode;
    using Contracts;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

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
        public LayoutInsertionOptionalClearIndex(Node parentNode, string propertyName)
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
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out LayoutInsertionOptionalClearIndex AsInsertionOptionalClearIndex))
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
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutInsertionOptionalClearIndex>());
            return new LayoutBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
