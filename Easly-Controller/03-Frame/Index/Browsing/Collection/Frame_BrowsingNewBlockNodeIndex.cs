namespace EaslyController.Frame
{
    using System;
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
        /// Gets the empty <see cref="FrameBrowsingNewBlockNodeIndex"/> object.
        /// </summary>
        public static new FrameBrowsingNewBlockNodeIndex Empty { get; } = new FrameBrowsingNewBlockNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        protected FrameBrowsingNewBlockNodeIndex()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public FrameBrowsingNewBlockNodeIndex(Node parentNode, Node node, string propertyName, int blockIndex)
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
            ControllerTools.AssertNoOverride(this, typeof(FrameBrowsingNewBlockNodeIndex));
            return new FrameBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
