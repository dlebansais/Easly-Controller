namespace EaslyController.Focus
{
    using Contracts;
    using EaslyController.Frame;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public interface IFocusInsertionEmptyNodeIndex : IFrameInsertionEmptyNodeIndex, IFocusInsertionChildNodeIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public class FocusInsertionEmptyNodeIndex : FrameInsertionEmptyNodeIndex, IFocusInsertionEmptyNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusInsertionEmptyNodeIndex"/> object.
        /// </summary>
        public static FocusInsertionEmptyNodeIndex Empty { get; } = new FocusInsertionEmptyNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInsertionEmptyNodeIndex"/> class.
        /// </summary>
        public FocusInsertionEmptyNodeIndex()
            : base()
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusInsertionEmptyNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusInsertionEmptyNodeIndex AsInsertionEmptyNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionEmptyNodeIndex))
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
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusInsertionEmptyNodeIndex>());
            return new FocusBrowsingPlaceholderNodeIndex(ParentNode, Node, PropertyName);
        }
        #endregion
    }
}
