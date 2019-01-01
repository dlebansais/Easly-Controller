using BaseNode;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public interface IFrameRootNodeIndex : IWriteableRootNodeIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Index for the root node of the node tree.
    /// </summary>
    public class FrameRootNodeIndex : WriteableRootNodeIndex, IFrameRootNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameRootNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed root node.</param>
        public FrameRootNodeIndex(INode node)
            : base(node)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameIndex"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameRootNodeIndex AsRootNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsRootNodeIndex))
                return false;

            return true;
        }
        #endregion
    }
}
