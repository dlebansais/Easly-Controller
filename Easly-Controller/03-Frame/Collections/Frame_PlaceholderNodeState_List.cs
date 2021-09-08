namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Diagnostics;
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
        bool ICollection<IFramePlaceholderNodeState>.IsReadOnly { get { return ((ICollection<IReadOnlyPlaceholderNodeState>)this).IsReadOnly; } }
        IEnumerator<IFramePlaceholderNodeState> IEnumerable<IFramePlaceholderNodeState>.GetEnumerator() { return ((IList<IFramePlaceholderNodeState>)this).GetEnumerator(); }
        IFramePlaceholderNodeState IList<IFramePlaceholderNodeState>.this[int index] { get { return (IFramePlaceholderNodeState)this[index]; } set { this[index] = value; } }
        int IList<IFramePlaceholderNodeState>.IndexOf(IFramePlaceholderNodeState item) { return IndexOf(item); }
        void IList<IFramePlaceholderNodeState>.Insert(int index, IFramePlaceholderNodeState item) { Insert(index, item); }
        IFramePlaceholderNodeState IReadOnlyList<IFramePlaceholderNodeState>.this[int index] { get { return (IFramePlaceholderNodeState)this[index]; } }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out FramePlaceholderNodeStateList AsPlaceholderNodeStateList))
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
            return new FramePlaceholderNodeStateReadOnlyList(this);
        }
    }
}
