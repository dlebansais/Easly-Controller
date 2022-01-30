namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public interface IFrameInsertionEmptyNodeIndex : IWriteableInsertionEmptyNodeIndex, IFrameInsertionChildNodeIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Index for replacing a child a node.
    /// </summary>
    public class FrameInsertionEmptyNodeIndex : WriteableInsertionEmptyNodeIndex, IFrameInsertionEmptyNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionEmptyNodeIndex"/> class.
        /// </summary>
        public FrameInsertionEmptyNodeIndex()
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="FrameInsertionEmptyNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FrameInsertionEmptyNodeIndex AsInsertionEmptyNodeIndex))
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
            ControllerTools.AssertNoOverride(this, typeof(FrameInsertionEmptyNodeIndex));
            return new FrameBrowsingPlaceholderNodeIndex(ParentNode, Node, PropertyName);
        }
        #endregion
    }
}
