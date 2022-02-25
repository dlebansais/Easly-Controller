namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public interface IFrameBrowsingOptionalNodeIndex : IWriteableBrowsingOptionalNodeIndex, IFrameBrowsingChildIndex, IFrameBrowsingInsertableIndex
    {
    }

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    internal class FrameBrowsingOptionalNodeIndex : WriteableBrowsingOptionalNodeIndex, IFrameBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public FrameBrowsingOptionalNodeIndex(Node parentNode, string propertyName)
            : base(parentNode, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameBrowsingOptionalNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out FrameBrowsingOptionalNodeIndex AsBrowsingOptionalNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingOptionalNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionPlaceholderNodeIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalNodeIndex CreateInsertionOptionalNodeIndex(Node parentNode, Node node)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBrowsingOptionalNodeIndex>());
            return new FrameInsertionOptionalNodeIndex(parentNode, PropertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertionOptionalClearIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalClearIndex CreateInsertionOptionalClearIndex(Node parentNode)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FrameBrowsingOptionalNodeIndex>());
            return new FrameInsertionOptionalClearIndex(parentNode, PropertyName);
        }
        #endregion
    }
}
