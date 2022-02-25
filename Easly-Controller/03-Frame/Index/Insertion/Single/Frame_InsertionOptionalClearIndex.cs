namespace EaslyController.Frame
{
    using BaseNode;
    using Contracts;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public interface IFrameInsertionOptionalClearIndex : IWriteableInsertionOptionalClearIndex, IFrameInsertionChildIndex
    {
    }

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public class FrameInsertionOptionalClearIndex : WriteableInsertionOptionalClearIndex, IFrameInsertionOptionalClearIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionOptionalClearIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public FrameInsertionOptionalClearIndex(Node parentNode, string propertyName)
            : base(parentNode, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameInsertionOptionalClearIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FrameInsertionOptionalClearIndex AsInsertionOptionalClearIndex))
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
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameInsertionOptionalClearIndex>());
            return new FrameBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
