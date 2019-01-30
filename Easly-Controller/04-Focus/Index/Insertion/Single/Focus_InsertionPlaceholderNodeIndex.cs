namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

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
        public FocusInsertionPlaceholderNodeIndex(INode parentNode, string propertyName, INode node)
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

            if (!(other is IFocusInsertionPlaceholderNodeIndex AsInsertionPlaceholderNodeIndex))
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
        protected override IWriteableBrowsingPlaceholderNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusInsertionPlaceholderNodeIndex));
            return new FocusBrowsingPlaceholderNodeIndex(ParentNode, Node, PropertyName);
        }
        #endregion
    }
}
