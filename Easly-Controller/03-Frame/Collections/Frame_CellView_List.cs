using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#pragma warning disable 1591

namespace EaslyController.Frame
{
    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    public interface IFrameCellViewList : IList<IFrameCellView>, IReadOnlyList<IFrameCellView>, IEqualComparable
    {
        new int Count { get; }
        new IFrameCellView this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    public class FrameCellViewList : Collection<IFrameCellView>, IFrameCellViewList
    {
        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFrameCellViewList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IFrameCellViewList AsCellViewList))
                return false;

            if (Count != AsCellViewList.Count)
                return false;

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsCellViewList[i]))
                    return false;

            return true;
        }
        #endregion
    }
}
