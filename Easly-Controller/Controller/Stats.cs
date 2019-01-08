using System.Diagnostics;

namespace EaslyController
{
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
            Debug.Assert(other != null);

            if (!(other is Stats AsStats))
                return false;

            if (NodeCount != AsStats.NodeCount)
                return false;

            if (PlaceholderNodeCount != AsStats.PlaceholderNodeCount)
                return false;

            if (OptionalNodeCount != AsStats.OptionalNodeCount)
                return false;

            if (AssignedOptionalNodeCount != AsStats.AssignedOptionalNodeCount)
                return false;

            if (ListCount != AsStats.ListCount)
                return false;

            if (BlockListCount != AsStats.BlockListCount)
                return false;

            if (BlockCount != AsStats.BlockCount)
                return false;

            return true;
        }
    }
}
