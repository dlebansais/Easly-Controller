namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using BaseNode;
    using BaseNodeHelper;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Index for inserting the first node of a new block.
    /// </summary>
    public class WriteableInsertionNewBlockNodeIndex : WriteableInsertionBlockNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionNewBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the block list.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the block list.</param>
        /// <param name="node">First node in the block.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        /// <param name="patternNode">Replication pattern in the block.</param>
        /// <param name="sourceNode">Source identifier in the block.</param>
        public WriteableInsertionNewBlockNodeIndex(Node parentNode, string propertyName, Node node, int blockIndex, Pattern patternNode, Identifier sourceNode)
            : base(parentNode, propertyName, node)
        {
            Debug.Assert(blockIndex >= 0);
            Debug.Assert(NodeTreeHelperBlockList.GetLastBlockIndex(parentNode, propertyName, out int LastBlockIndex) && blockIndex <= LastBlockIndex);
            Debug.Assert(patternNode != null);
            Debug.Assert(sourceNode != null);

            BlockIndex = blockIndex;
            PatternNode = patternNode;
            SourceNode = sourceNode;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Position of the inserted block in the block list.
        /// </summary>
        public int BlockIndex { get;  }

        /// <summary>
        /// Replication pattern in the block.
        /// </summary>
        public Pattern PatternNode { get; }

        /// <summary>
        /// Source identifier in the block.
        /// </summary>
        public Identifier SourceNode { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Creates a browsing index from an insertion index.
        /// To call after the insertion operation has been completed.
        /// </summary>
        public override IWriteableBrowsingChildIndex ToBrowsingIndex()
        {
            return CreateBrowsingIndex();
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableInsertionNewBlockNodeIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableInsertionNewBlockNodeIndex AsInsertionNewBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionNewBlockNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameInteger(BlockIndex, AsInsertionNewBlockNodeIndex.BlockIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(PatternNode, AsInsertionNewBlockNodeIndex.PatternNode))
                return comparer.Failed();

            if (!comparer.IsSameReference(SourceNode, AsInsertionNewBlockNodeIndex.SourceNode))
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingExistingBlockNodeIndex.
        /// </summary>
        private protected virtual WriteableBrowsingExistingBlockNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertionNewBlockNodeIndex));
            return new WriteableBrowsingExistingBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, 0);
        }
        #endregion
    }
}
