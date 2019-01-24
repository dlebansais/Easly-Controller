using BaseNode;
using BaseNodeHelper;
using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for inserting the first node of a new block.
    /// </summary>
    public interface IWriteableInsertionNewBlockNodeIndex : IWriteableInsertionBlockNodeIndex
    {
        /// <summary>
        /// Position of the inserted block in the block list.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// Replication pattern in the block.
        /// </summary>
        IPattern PatternNode { get; }

        /// <summary>
        /// Source identifier in the block.
        /// </summary>
        IIdentifier SourceNode { get; }
    }

    /// <summary>
    /// Index for inserting the first node of a new block.
    /// </summary>
    public class WriteableInsertionNewBlockNodeIndex : WriteableInsertionBlockNodeIndex, IWriteableInsertionNewBlockNodeIndex
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
        public WriteableInsertionNewBlockNodeIndex(INode parentNode, string propertyName, INode node, int blockIndex, IPattern patternNode, IIdentifier sourceNode)
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
        public IPattern PatternNode { get; }

        /// <summary>
        /// Source identifier in the block.
        /// </summary>
        public IIdentifier SourceNode { get; }
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
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableInsertionNewBlockNodeIndex AsInsertionNewBlockNodeIndex))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsInsertionNewBlockNodeIndex))
                return comparer.Failed();

            if (BlockIndex != AsInsertionNewBlockNodeIndex.BlockIndex)
                return comparer.Failed();

            if (PatternNode != AsInsertionNewBlockNodeIndex.PatternNode)
                return comparer.Failed();

            if (SourceNode != AsInsertionNewBlockNodeIndex.SourceNode)
                return comparer.Failed();

            return true;
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxBrowsingNewBlockNodeIndex object.
        /// </summary>
        protected virtual IWriteableBrowsingNewBlockNodeIndex CreateBrowsingIndex()
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertionNewBlockNodeIndex));
            return new WriteableBrowsingNewBlockNodeIndex(ParentNode, Node, PropertyName, BlockIndex, PatternNode, SourceNode);
        }
        #endregion
    }
}
