namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusPlaceholderNodeStateList : FramePlaceholderNodeStateList, ICollection<IFocusPlaceholderNodeState>, IEnumerable<IFocusPlaceholderNodeState>, IList<IFocusPlaceholderNodeState>, IReadOnlyCollection<IFocusPlaceholderNodeState>, IReadOnlyList<IFocusPlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public new IFocusPlaceholderNodeState this[int index] { get { return (IFocusPlaceholderNodeState)base[index]; } set { base[index] = value; } }

        #region IFocusPlaceholderNodeState
        void ICollection<IFocusPlaceholderNodeState>.Add(IFocusPlaceholderNodeState item) { Add(item); }
        bool ICollection<IFocusPlaceholderNodeState>.Contains(IFocusPlaceholderNodeState item) { return Contains(item); }
        void ICollection<IFocusPlaceholderNodeState>.CopyTo(IFocusPlaceholderNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFocusPlaceholderNodeState>.Remove(IFocusPlaceholderNodeState item) { return Remove(item); }
        bool ICollection<IFocusPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFramePlaceholderNodeState>)this).IsReadOnly; } }
        IEnumerator<IFocusPlaceholderNodeState> IEnumerable<IFocusPlaceholderNodeState>.GetEnumerator() { var iterator = ((List<IReadOnlyPlaceholderNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusPlaceholderNodeState)iterator.Current; } }
        IFocusPlaceholderNodeState IList<IFocusPlaceholderNodeState>.this[int index] { get { return (IFocusPlaceholderNodeState)this[index]; } set { this[index] = value; } }
        int IList<IFocusPlaceholderNodeState>.IndexOf(IFocusPlaceholderNodeState item) { return IndexOf(item); }
        void IList<IFocusPlaceholderNodeState>.Insert(int index, IFocusPlaceholderNodeState item) { Insert(index, item); }
        IFocusPlaceholderNodeState IReadOnlyList<IFocusPlaceholderNodeState>.this[int index] { get { return (IFocusPlaceholderNodeState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new FocusPlaceholderNodeStateReadOnlyList(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FocusPlaceholderNodeStateList AsOtherList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsOtherList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsOtherList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion
    }
}
