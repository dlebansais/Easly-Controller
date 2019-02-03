namespace EaslyController.ReadOnly
{
    using System;
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    public interface IReadOnlyBrowsingNewBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex
    {
        /// <summary>
        /// The parent node.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Gets the index for this node in an existing block.
        /// </summary>
        IReadOnlyBrowsingExistingBlockNodeIndex ToExistingBlockIndex();
    }

    /// <summary>
    /// Index for the first node in a block.
    /// </summary>
    internal class ReadOnlyBrowsingNewBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IReadOnlyBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public ReadOnlyBrowsingNewBlockNodeIndex(INode parentNode, INode node, string propertyName, int blockIndex)
            : base(node, propertyName, blockIndex)
        {
            Debug.Assert(parentNode != null);
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(blockIndex >= 0);
            Debug.Assert(NodeTreeHelperBlockList.IsBlockChildNode(parentNode, propertyName, blockIndex, 0, node));

            ParentNode = parentNode;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The parent node.
        /// </summary>
        public INode ParentNode { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Gets the index for this node in an existing block.
        /// </summary>
        public virtual IReadOnlyBrowsingExistingBlockNodeIndex ToExistingBlockIndex()
        {
            return CreateExistingBlockIndex();
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex object.
        /// </summary>
        private protected virtual IReadOnlyBrowsingExistingBlockNodeIndex CreateExistingBlockIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(ReadOnlyBrowsingNewBlockNodeIndex));
            return new ReadOnlyBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
