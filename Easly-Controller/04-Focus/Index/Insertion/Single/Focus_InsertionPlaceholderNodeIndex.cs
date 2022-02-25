namespace EaslyController.Focus
{
    using BaseNode;
    using Contracts;
    using EaslyController.Frame;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public interface IFocusInsertionPlaceholderNodeIndex : IFrameInsertionPlaceholderNodeIndex, IFocusInsertionChildNodeIndex, IFocusNodeIndex
    {
    }

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public class FocusInsertionPlaceholderNodeIndex : FrameInsertionPlaceholderNodeIndex, IFocusInsertionPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInsertionPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the replaced node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.</param>
        /// <param name="node">The assigned node.</param>
        public FocusInsertionPlaceholderNodeIndex(Node parentNode, string propertyName, Node node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FocusInsertionPlaceholderNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out FocusInsertionPlaceholderNodeIndex AsInsertionPlaceholderNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionPlaceholderNodeIndex))
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
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<FocusInsertionPlaceholderNodeIndex>());
            return new FocusBrowsingPlaceholderNodeIndex(ParentNode, Node, PropertyName);
        }
        #endregion
    }
}
