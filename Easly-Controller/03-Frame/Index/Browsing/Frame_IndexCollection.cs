using EaslyController.Writeable;
using System.Collections.Generic;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface IFrameIndexCollection : IWriteableIndexCollection
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public interface IFrameIndexCollection<out IIndex> : IWriteableIndexCollection<IIndex>
        where IIndex : IFrameBrowsingChildIndex
    {
    }

    /// <summary>
    /// Collection of node indexes.
    /// </summary>
    public class FrameIndexCollection<IIndex> : WriteableIndexCollection<IIndex>, IFrameIndexCollection<IIndex>, IFrameIndexCollection
        where IIndex : IFrameBrowsingChildIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameIndexCollection{IIndex}"/> class.
        /// </summary>
        /// <param name="propertyName">Property indexed for all nodes in the collection.</param>
        /// <param name="nodeIndexList">Collection of node indexes.</param>
        public FrameIndexCollection(string propertyName, IReadOnlyList<IIndex> nodeIndexList)
            : base(propertyName, nodeIndexList)
        {
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameIndexCollection"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameIndexCollection<IIndex> AsIndexCollection))
                return false;

            if (!base.IsEqual(comparer, AsIndexCollection))
                return false;

            return true;
        }
        #endregion
    }
}
