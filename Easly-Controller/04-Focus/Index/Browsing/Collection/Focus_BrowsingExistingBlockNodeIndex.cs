namespace EaslyController.Focus
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public interface IFocusBrowsingExistingBlockNodeIndex : IFrameBrowsingExistingBlockNodeIndex, IFocusBrowsingBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for a node in a block that is not the first.
    /// </summary>
    public class FocusBrowsingExistingBlockNodeIndex : FrameBrowsingExistingBlockNodeIndex, IFocusBrowsingExistingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingExistingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">Indexed node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="index">Position of the node in the block.</param>
        public FocusBrowsingExistingBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex, int index)
            : base(parentNode, node, propertyName, blockIndex, index)
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

            if (!(other is IFocusBrowsingExistingBlockNodeIndex AsBrowsingExistingBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsBrowsingExistingBlockNodeIndex))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertionExistingBlockNodeIndex object.
        /// </summary>
        protected override IWriteableInsertionExistingBlockNodeIndex CreateInsertionIndex(INode parentNode, INode node)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusBrowsingExistingBlockNodeIndex));
            return new FocusInsertionExistingBlockNodeIndex(parentNode, PropertyName, node, BlockIndex, Index);
        }
        #endregion
    }
}
