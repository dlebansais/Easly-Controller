namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public interface IFrameInsertionPlaceholderNodeIndex : IWriteableInsertionPlaceholderNodeIndex, IFrameInsertionChildNodeIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public class FrameInsertionPlaceholderNodeIndex : WriteableInsertionPlaceholderNodeIndex, IFrameInsertionPlaceholderNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionPlaceholderNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the replaced node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed node.</param>
        /// <param name="node">The assigned node.</param>
        public FrameInsertionPlaceholderNodeIndex(INode parentNode, string propertyName, INode node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameInsertionPlaceholderNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameInsertionPlaceholderNodeIndex AsInsertionPlaceholderNodeIndex))
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
            ControllerTools.AssertNoOverride(this, typeof(FrameInsertionPlaceholderNodeIndex));
            return new FrameBrowsingPlaceholderNodeIndex(ParentNode, Node, PropertyName);
        }
        #endregion
    }
}
