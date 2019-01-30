namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public interface IFocusInsertionOptionalNodeIndex : IFrameInsertionOptionalNodeIndex, IFocusInsertionChildNodeIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Index for replacing an optional node.
    /// </summary>
    public class FocusInsertionOptionalNodeIndex : FrameInsertionOptionalNodeIndex, IFocusInsertionOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInsertionOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        /// <param name="node">The assigned node.</param>
        public FocusInsertionOptionalNodeIndex(INode parentNode, string propertyName, INode node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFocusInsertionOptionalNodeIndex AsInsertionOptionalNodeIndex))
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
        protected override IWriteableBrowsingOptionalNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertionOptionalNodeIndex));
            return new FocusBrowsingOptionalNodeIndex(ParentNode, PropertyName);
        }
        #endregion
    }
}
