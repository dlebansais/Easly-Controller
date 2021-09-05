namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutPlaceholderNodeStateList : FocusPlaceholderNodeStateList, ICollection<ILayoutPlaceholderNodeState>, IEnumerable<ILayoutPlaceholderNodeState>, IList<ILayoutPlaceholderNodeState>, IReadOnlyCollection<ILayoutPlaceholderNodeState>, IReadOnlyList<ILayoutPlaceholderNodeState>, IEqualComparable
    {
        #region ILayoutPlaceholderNodeState
        void ICollection<ILayoutPlaceholderNodeState>.Add(ILayoutPlaceholderNodeState item) { Add(item); }
        bool ICollection<ILayoutPlaceholderNodeState>.Contains(ILayoutPlaceholderNodeState item) { return Contains(item); }
        void ICollection<ILayoutPlaceholderNodeState>.CopyTo(ILayoutPlaceholderNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutPlaceholderNodeState>.Remove(ILayoutPlaceholderNodeState item) { return Remove(item); }
        bool ICollection<ILayoutPlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IReadOnlyPlaceholderNodeState>)this).IsReadOnly; } }
        IEnumerator<ILayoutPlaceholderNodeState> IEnumerable<ILayoutPlaceholderNodeState>.GetEnumerator() { return ((IList<ILayoutPlaceholderNodeState>)this).GetEnumerator(); }
        ILayoutPlaceholderNodeState IList<ILayoutPlaceholderNodeState>.this[int index] { get { return (ILayoutPlaceholderNodeState)this[index]; } set { this[index] = value; } }
        int IList<ILayoutPlaceholderNodeState>.IndexOf(ILayoutPlaceholderNodeState item) { return IndexOf(item); }
        void IList<ILayoutPlaceholderNodeState>.Insert(int index, ILayoutPlaceholderNodeState item) { Insert(index, item); }
        ILayoutPlaceholderNodeState IReadOnlyList<ILayoutPlaceholderNodeState>.this[int index] { get { return (ILayoutPlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out LayoutPlaceholderNodeStateList AsPlaceholderNodeStateList))
                return comparer.Failed();

            if (!comparer.IsSameCount(Count, AsPlaceholderNodeStateList.Count))
                return comparer.Failed();

            for (int i = 0; i < Count; i++)
                if (!comparer.VerifyEqual(this[i], AsPlaceholderNodeStateList[i]))
                    return comparer.Failed();

            return true;
        }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new LayoutPlaceholderNodeStateReadOnlyList(this);
        }
    }
}
