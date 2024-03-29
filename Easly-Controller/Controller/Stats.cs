﻿namespace EaslyController
{
    using Contracts;

    /// <summary>
    /// Statistics for a node tree.
    /// </summary>
    public class Stats : IEqualComparable
    {
        /// <summary>
        /// Number of nodes.
        /// </summary>
        public int NodeCount { get; set; }

        /// <summary>
        /// Number of nodes that are not optional.
        /// </summary>
        public int PlaceholderNodeCount { get; set; }

        /// <summary>
        /// Number of optional nodes.
        /// </summary>
        public int OptionalNodeCount { get; set; }

        /// <summary>
        /// Number of optional nodes that are currently assigned.
        /// </summary>
        public int AssignedOptionalNodeCount { get; set; }

        /// <summary>
        /// Number of lists of nodes.
        /// </summary>
        public int ListCount { get; set; }

        /// <summary>
        /// Number of block lists.
        /// </summary>
        public int BlockListCount { get; set; }

        /// <summary>
        /// Number of blocks in block lists.
        /// </summary>
        public int BlockCount { get; set; }

        /// <summary>
        /// Compares two <see cref="Stats"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out Stats AsStats))
                return comparer.Failed();

            if (!comparer.IsSameCount(NodeCount, AsStats.NodeCount))
                return comparer.Failed();

            if (!comparer.IsSameCount(PlaceholderNodeCount, AsStats.PlaceholderNodeCount))
                return comparer.Failed();

            if (!comparer.IsSameCount(OptionalNodeCount, AsStats.OptionalNodeCount))
                return comparer.Failed();

            if (!comparer.IsSameCount(AssignedOptionalNodeCount, AsStats.AssignedOptionalNodeCount))
                return comparer.Failed();

            if (!comparer.IsSameCount(ListCount, AsStats.ListCount))
                return comparer.Failed();

            if (!comparer.IsSameCount(BlockListCount, AsStats.BlockListCount))
                return comparer.Failed();

            if (!comparer.IsSameCount(BlockCount, AsStats.BlockCount))
                return comparer.Failed();

            return true;
        }
    }
}
