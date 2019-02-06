#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    public interface IFrameCellViewList : IList<IFrameCellView>, IReadOnlyList<IFrameCellView>, IEqualComparable
    {
        new IFrameCellView this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxCellView
    /// </summary>
    internal class FrameCellViewList : Collection<IFrameCellView>, IFrameCellViewList
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

            if (!comparer.IsSameType(other, out FrameCellViewList AsCellViewList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsCellViewList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsCellViewList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
