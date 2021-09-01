#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public class WriteableBlockStateList : ReadOnlyBlockStateList, ICollection<IWriteableBlockState>, IEnumerable<IWriteableBlockState>, IList<IWriteableBlockState>, IReadOnlyCollection<IWriteableBlockState>, IReadOnlyList<IWriteableBlockState>
    {
        #region IWriteableBlockState
        void ICollection<IWriteableBlockState>.Add(IWriteableBlockState item) { Add(item); }
        bool ICollection<IWriteableBlockState>.Contains(IWriteableBlockState item) { return Contains(item); }
        void ICollection<IWriteableBlockState>.CopyTo(IWriteableBlockState[] array, int arrayIndex) { for(int i = 0; i < Count; i++) array[arrayIndex + i] = (IWriteableBlockState)this[i]; }
        bool ICollection<IWriteableBlockState>.Remove(IWriteableBlockState item) { return Remove(item); }
        bool ICollection<IWriteableBlockState>.IsReadOnly { get { return false; } }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return new List<IWriteableBlockState>(this).GetEnumerator(); }
        IWriteableBlockState IList<IWriteableBlockState>.this[int index] { get { return (IWriteableBlockState)this[index]; } set { this[index] = value; } }
        int IList<IWriteableBlockState>.IndexOf(IWriteableBlockState item) { return IndexOf(item); }
        void IList<IWriteableBlockState>.Insert(int index, IWriteableBlockState item) { Insert(index, item); }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return (IWriteableBlockState)this[index]; } }
        #endregion

        public override ReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new WriteableBlockStateReadOnlyList(this);
        }
    }
}
