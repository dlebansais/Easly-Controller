using BaseNode;
using EaslyController.ReadOnly;
using System.Diagnostics;

/*
namespace EaslyController.Writeable
{
    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public interface IWriteableInsertionSourceIndex : IWriteableInsertionChildIndex, IWriteableNodeIndex
    {
        /// <summary>
        /// The indexed source identifier node.
        /// </summary>
        new IIdentifier Node { get; }
    }

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public class WriteableInsertionSourceIndex : IWriteableInsertionSourceIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertionSourceIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed source identifier node.</param>
        public WriteableInsertionSourceIndex(IBlock block)
        {
            Debug.Assert(block != null);

            Block = block;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The indexed source identifier node.
        /// </summary>
        public IIdentifier Node { get { return Block.SourceIdentifier; } }
        INode IReadOnlyNodeIndex.Node { get { return Node; } }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get { return nameof(IBlock.SourceIdentifier); } }
        private IBlock Block;
        #endregion
    }
}
*/