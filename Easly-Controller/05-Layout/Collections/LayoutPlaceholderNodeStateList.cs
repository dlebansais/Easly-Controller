namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class LayoutPlaceholderNodeStateList : FocusPlaceholderNodeStateList, ICollection<ILayoutPlaceholderNodeState>, IEnumerable<ILayoutPlaceholderNodeState>, IList<ILayoutPlaceholderNodeState>, IReadOnlyCollection<ILayoutPlaceholderNodeState>, IReadOnlyList<ILayoutPlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public new ILayoutPlaceholderNodeState this[int index] { get { return (ILayoutPlaceholderNodeState)base[index]; } set { base[index] = value; } }

        #region ILayoutPlaceholderNodeState
        void ICollection<ILayoutPlaceholderNodeState>.Add(ILayoutPlaceholderNodeState item) { Add(item); }
        bool ICollection<ILayoutPlaceholderNodeState>.Contains(ILayoutPlaceholderNodeState item) { return Contains(item); }
        void ICollection<ILayoutPlaceholderNodeState>.CopyTo(ILayoutPlaceholderNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutPlaceholderNodeState>.Remove(ILayoutPlaceholderNodeState item) { return Remove(item); }
        bool ICollection<ILayoutPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IFocusPlaceholderNodeState>)this).IsReadOnly; } }
        IEnumerator<ILayoutPlaceholderNodeState> IEnumerable<ILayoutPlaceholderNodeState>.GetEnumerator() { var iterator = ((List<IReadOnlyPlaceholderNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutPlaceholderNodeState)iterator.Current; } }
        ILayoutPlaceholderNodeState IList<ILayoutPlaceholderNodeState>.this[int index] { get { return (ILayoutPlaceholderNodeState)this[index]; } set { this[index] = value; } }
        int IList<ILayoutPlaceholderNodeState>.IndexOf(ILayoutPlaceholderNodeState item) { return IndexOf(item); }
        void IList<ILayoutPlaceholderNodeState>.Insert(int index, ILayoutPlaceholderNodeState item) { Insert(index, item); }
        ILayoutPlaceholderNodeState IReadOnlyList<ILayoutPlaceholderNodeState>.this[int index] { get { return (ILayoutPlaceholderNodeState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new LayoutPlaceholderNodeStateReadOnlyList(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutPlaceholderNodeStateList AsOtherList))
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
