using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public interface IReadOnlyBrowsingPatternIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex
    {
        /// <summary>
        /// The indexed replication pattern node.
        /// </summary>
        new IPattern Node { get; }
    }

    /// <summary>
    /// Index for the replication pattern node of a block.
    /// </summary>
    public class ReadOnlyBrowsingPatternIndex : IReadOnlyBrowsingPatternIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingPatternIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed replication pattern node.</param>
        public ReadOnlyBrowsingPatternIndex(IBlock block)
        {
            Debug.Assert(block != null);

            Block = block;
        }

        private IBlock Block;
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
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyBrowsingPatternIndex AsPatternIndex))
                return false;

            if (Node != AsPatternIndex.Node)
                return false;

            if (PropertyName != AsPatternIndex.PropertyName)
                return false;

            return true;
        }
        #endregion
    }
}
