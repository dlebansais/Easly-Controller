namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public interface IFrameBrowsingNewBlockNodeIndex : IWriteableBrowsingNewBlockNodeIndex, IFrameBrowsingBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    internal class FrameBrowsingNewBlockNodeIndex : WriteableBrowsingNewBlockNodeIndex, IFrameBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public FrameBrowsingNewBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex)
            : base(parentNode, node, propertyName, blockIndex)
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

            if (!comparer.IsSameType(other, out FrameBrowsingNewBlockNodeIndex AsBrowsingNewBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingNewBlockNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowsingNewBlockNodeIndex));
            return new FrameBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
