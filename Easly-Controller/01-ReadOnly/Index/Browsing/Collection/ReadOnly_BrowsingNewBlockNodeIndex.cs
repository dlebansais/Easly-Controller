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
        Node ParentNode { get; }

        /// <summary>
        /// Gets the index for this node in an existing block.
        /// </summary>
        IReadOnlyBrowsingExistingBlockNodeIndex ToExistingBlockIndex();
    }

    /// <inheritdoc/>
    internal class ReadOnlyBrowsingNewBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IReadOnlyBrowsingNewBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="ReadOnlyBrowsingNewBlockNodeIndex"/> object.
        /// </summary>
        public static new ReadOnlyBrowsingNewBlockNodeIndex Empty { get; } = new ReadOnlyBrowsingNewBlockNodeIndex();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        protected ReadOnlyBrowsingNewBlockNodeIndex()
        {
            ParentNode = Node.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public ReadOnlyBrowsingNewBlockNodeIndex(Node parentNode, Node node, string propertyName, int blockIndex)
            : base(node, propertyName, blockIndex)
        {
            Debug.Assert(!string.IsNullOrEmpty(propertyName));
            Debug.Assert(blockIndex >= 0);
            Debug.Assert(NodeTreeHelperBlockList.IsBlockChildNode(parentNode, propertyName, blockIndex, 0, node));

            ParentNode = parentNode;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public Node ParentNode { get; }
        #endregion

        #region Client Interface
        /// <inheritdoc/>
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
