using BaseNode;
using EaslyController.Writeable;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public interface IFrameBrowsingOptionalNodeIndex : IWriteableBrowsingOptionalNodeIndex, IFrameBrowsingChildIndex
    {
    }

    /// <summary>
    /// Index for an optional node.
    /// </summary>
    public class FrameBrowsingOptionalNodeIndex : WriteableBrowsingOptionalNodeIndex, IFrameBrowsingOptionalNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingOptionalNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">Node containing the indexed optional node.</param>
        /// <param name="propertyName">Property in <paramref name="parentNode"/> corresponding to the indexed optional node.</param>
        public FrameBrowsingOptionalNodeIndex(INode parentNode, string propertyName)
            : base(parentNode, propertyName)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameBrowsingOptionalNodeIndex AsBrowsingOptionalNodeIndex))
                return false;

            if (!base.IsEqual(comparer, AsBrowsingOptionalNodeIndex))
                return false;

            return true;
        }
        #endregion
    }
}
