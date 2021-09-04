namespace EaslyController.Writeable
{
    using System;
    using BaseNode;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public interface IWriteableBrowsingNewBlockNodeIndex : IReadOnlyBrowsingNewBlockNodeIndex, IWriteableBrowsingBlockNodeIndex
    {
    }

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    internal class WriteableBrowsingNewBlockNodeIndex : ReadOnlyBrowsingNewBlockNodeIndex, IWriteableBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public WriteableBrowsingNewBlockNodeIndex(Node parentNode, Node node, string propertyName, int blockIndex)
            : base(parentNode, node, propertyName, blockIndex)
        {
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected override IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableBrowsingNewBlockNodeIndex));
            return new WriteableBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
