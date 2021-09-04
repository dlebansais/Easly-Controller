﻿namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Index for the source identifier node of a block.
    /// </summary>
    public interface IReadOnlyBrowsingSourceIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex, IEqualComparable
    {
        /// <summary>
        /// The indexed source identifier node.
        /// </summary>
        new Identifier Node { get; }
    }

    /// <inheritdoc/>
    public class ReadOnlyBrowsingSourceIndex : IReadOnlyBrowsingSourceIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingSourceIndex"/> class.
        /// </summary>
        /// <param name="block">The block containing the indexed source identifier node.</param>
        public ReadOnlyBrowsingSourceIndex(IBlock block)
        {
            Debug.Assert(block != null);

            Block = block;
        }

        private IBlock Block;
        #endregion

        #region Properties
        /// <inheritdoc/>
        public Identifier Node { get { return Block.SourceIdentifier; } }
        Node IReadOnlyNodeIndex.Node { get { return Node; } }

        /// <inheritdoc/>
        public string PropertyName { get { return nameof(IBlock.SourceIdentifier); } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyBrowsingSourceIndex AsSourceIndex))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsSourceIndex.Node))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsSourceIndex.PropertyName))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
