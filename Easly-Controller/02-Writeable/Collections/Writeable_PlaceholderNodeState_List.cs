namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class WriteablePlaceholderNodeStateList : ReadOnlyPlaceholderNodeStateList, ICollection<IWriteablePlaceholderNodeState>, IEnumerable<IWriteablePlaceholderNodeState>, IList<IWriteablePlaceholderNodeState>, IReadOnlyCollection<IWriteablePlaceholderNodeState>, IReadOnlyList<IWriteablePlaceholderNodeState>, IEqualComparable
    {
        #region IWriteablePlaceholderNodeState
        void ICollection<IWriteablePlaceholderNodeState>.Add(IWriteablePlaceholderNodeState item) { Add(item); }
        bool ICollection<IWriteablePlaceholderNodeState>.Contains(IWriteablePlaceholderNodeState item) { return Contains(item); }
        void ICollection<IWriteablePlaceholderNodeState>.CopyTo(IWriteablePlaceholderNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IWriteablePlaceholderNodeState>.Remove(IWriteablePlaceholderNodeState item) { return Remove(item); }
        bool ICollection<IWriteablePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IReadOnlyPlaceholderNodeState>)this).IsReadOnly; } }
        IEnumerator<IWriteablePlaceholderNodeState> IEnumerable<IWriteablePlaceholderNodeState>.GetEnumerator() { return ((IList<IWriteablePlaceholderNodeState>)this).GetEnumerator(); }
        IWriteablePlaceholderNodeState IList<IWriteablePlaceholderNodeState>.this[int index] { get { return (IWriteablePlaceholderNodeState)this[index]; } set { this[index] = value; } }
        int IList<IWriteablePlaceholderNodeState>.IndexOf(IWriteablePlaceholderNodeState item) { return IndexOf(item); }
        void IList<IWriteablePlaceholderNodeState>.Insert(int index, IWriteablePlaceholderNodeState item) { Insert(index, item); }
        IWriteablePlaceholderNodeState IReadOnlyList<IWriteablePlaceholderNodeState>.this[int index] { get { return (IWriteablePlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteablePlaceholderNodeStateList AsPlaceholderNodeStateList))
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
            return new WriteablePlaceholderNodeStateReadOnlyList(this);
        }
    }
}
