using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public interface IFrameVisibleCellViewList : IList<IFrameVisibleCellView>, IReadOnlyList<IFrameVisibleCellView>, IEqualComparable
    {
        new int Count { get; }
        new IFrameVisibleCellView this[int index] { get; set; }
        new IEnumerator<IFrameVisibleCellView> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxVisibleCellView
    /// </summary>
    public class FrameVisibleCellViewList : Collection<IFrameVisibleCellView>, IFrameVisibleCellViewList
    {
        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameVisibleCellViewList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameVisibleCellViewList AsVisibleCellViewList))
                return false;

            if (Count != AsVisibleCellViewList.Count)
                return false;

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsVisibleCellViewList[i]))
                    return false;

            return true;
        }
        #endregion
    }
}
