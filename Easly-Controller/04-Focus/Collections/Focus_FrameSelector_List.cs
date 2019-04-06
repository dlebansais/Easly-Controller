#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    /// <summary>
    /// List of IxxxFrameSelector
    /// </summary>
    public interface IFocusFrameSelectorList : IList<IFocusFrameSelector>, IReadOnlyList<IFocusFrameSelector>, IEqualComparable
    {
        new IFocusFrameSelector this[int index] { get; set; }
        new int Count { get; }
    }

    /// <summary>
    /// List of IxxxFrameSelector
    /// </summary>
    internal class FocusFrameSelectorList : Collection<IFocusFrameSelector>, IFocusFrameSelectorList
    {
        #region Debugging
        /// <summary>
        /// Compares two <see cref="IFocusFrameSelectorList"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out IFocusFrameSelectorList AsFrameSelectorList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsFrameSelectorList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsFrameSelectorList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
