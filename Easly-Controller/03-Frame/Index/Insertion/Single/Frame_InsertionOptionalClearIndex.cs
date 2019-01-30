namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

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
        public FrameInsertionOptionalClearIndex(INode parentNode, string propertyName)
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

            if (!(other is IFrameInsertionOptionalClearIndex AsInsertionOptionalClearIndex))
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
        protected override IWriteableBrowsingOptionalNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameInsertionOptionalClearIndex));
            return new FrameBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
