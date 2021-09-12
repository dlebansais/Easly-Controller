namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FramePlaceholderNodeStateList : WriteablePlaceholderNodeStateList, ICollection<IFramePlaceholderNodeState>, IEnumerable<IFramePlaceholderNodeState>, IList<IFramePlaceholderNodeState>, IReadOnlyCollection<IFramePlaceholderNodeState>, IReadOnlyList<IFramePlaceholderNodeState>, IEqualComparable
    {
        /// <inheritdoc/>
        public new IFramePlaceholderNodeState this[int index] { get { return (IFramePlaceholderNodeState)base[index]; } set { base[index] = value; } }

        #region IFramePlaceholderNodeState
        void ICollection<IFramePlaceholderNodeState>.Add(IFramePlaceholderNodeState item) { Add(item); }
        bool ICollection<IFramePlaceholderNodeState>.Contains(IFramePlaceholderNodeState item) { return Contains(item); }
        void ICollection<IFramePlaceholderNodeState>.CopyTo(IFramePlaceholderNodeState[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<IFramePlaceholderNodeState>.Remove(IFramePlaceholderNodeState item) { return Remove(item); }
        bool ICollection<IFramePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IWriteablePlaceholderNodeState>)this).IsReadOnly; } }
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { var iterator = ((List<IReadOnlyPlaceholderNodeState>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFramePlaceholderNodeState)iterator.Current; } }
        IFramePlaceholderNodeState IList<IFramePlaceholderNodeState>.this[int index] { get { return (IFramePlaceholderNodeState)this[index]; } set { this[index] = value; } }
        int IList<IFramePlaceholderNodeState>.IndexOf(IFramePlaceholderNodeState item) { return IndexOf(item); }
        void IList<IFramePlaceholderNodeState>.Insert(int index, IFramePlaceholderNodeState item) { Insert(index, item); }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return (IFramePlaceholderNodeState)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override ReadOnlyPlaceholderNodeStateReadOnlyList ToReadOnly()
        {
            return new FramePlaceholderNodeStateReadOnlyList(this);
        }

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            System.Diagnostics.Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FramePlaceholderNodeStateList AsOtherList))
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
