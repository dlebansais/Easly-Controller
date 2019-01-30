namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public interface IFrameBrowsingOptionalNodeIndex : IWriteableBrowsingOptionalNodeIndex, IFrameBrowsingChildIndex
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
        public FrameBrowsingOptionalNodeIndex(INode parentNode, string propertyName)
            : base(parentNode, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameBrowsingOptionalNodeIndex AsBrowsingOptionalNodeIndex))
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
        private protected override IWriteableInsertionOptionalNodeIndex CreateInsertionOptionalNodeIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowsingOptionalNodeIndex));
            return new FrameInsertionOptionalNodeIndex(parentNode, PropertyName, node);
        }

        /// <summary>
        /// Creates a IxxxInsertionOptionalClearIndex object.
        /// </summary>
        private protected override IWriteableInsertionOptionalClearIndex CreateInsertionOptionalClearIndex(INode parentNode)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowsingOptionalNodeIndex));
            return new FrameInsertionOptionalClearIndex(parentNode, PropertyName);
        }
        #endregion
    }
}
