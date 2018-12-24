using BaseNode;
using EaslyController.ReadOnly;
using System.Diagnostics;

/*
namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface IWriteableInsertionPatternIndex : IWriteableInsertionChildIndex, IWriteableNodeIndex
    {
        /// <summary>
        /// The indexed replication pattern node.
        /// </summary>
        new IPattern Node { get; }
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public class WriteableInsertionPatternIndex : IWriteableInsertionPatternIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionPatternIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed replication pattern node.</param>
        public WriteableInsertionPatternIndex(IBlock block)
        {
            Debug.Assert(block != null);

            Block = block;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The indexed replication pattern node.
        /// </summary>
        public IPattern Node { get { return Block.ReplicationPattern; } }
        INode IReadOnlyNodeIndex.Node { get { return Node; } }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get { return nameof(IBlock.ReplicationPattern); } }

        private IBlock Block;
        #endregion
    }
}
*/
