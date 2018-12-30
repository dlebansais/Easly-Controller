using System.Diagnostics;

namespace EaslyController
{
    public class Stats : IEqualComparable
    {
        public int NodeCount { get; set; }
        public int PlaceholderNodeCount { get; set; }
        public int OptionalNodeCount { get; set; }
        public int AssignedOptionalNodeCount { get; set; }
        public int ListCount { get; set; }
        public int BlockListCount { get; set; }

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

            return true;
        }
    }
}
