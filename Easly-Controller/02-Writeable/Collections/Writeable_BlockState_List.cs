#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public interface IWriteableBlockStateList : IReadOnlyBlockStateList, IList<IWriteableBlockState>, IReadOnlyList<IWriteableBlockState>
    {
        new IWriteableBlockState this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IWriteableBlockState> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    internal class WriteableBlockStateList : Collection<IWriteableBlockState>, IWriteableBlockStateList
    {
        #region ReadOnly
        IReadOnlyBlockState IReadOnlyBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (IWriteableBlockState)value; } }
        IReadOnlyBlockState IList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } set { this[index] = (IWriteableBlockState)value; } }
        int IList<IReadOnlyBlockState>.IndexOf(IReadOnlyBlockState value) { return IndexOf((IWriteableBlockState)value); }
        void IList<IReadOnlyBlockState>.Insert(int index, IReadOnlyBlockState item) { Insert(index, (IWriteableBlockState)item); }
        void ICollection<IReadOnlyBlockState>.Add(IReadOnlyBlockState item) { Add((IWriteableBlockState)item); }
        bool ICollection<IReadOnlyBlockState>.Contains(IReadOnlyBlockState value) { return Contains((IWriteableBlockState)value); }
        void ICollection<IReadOnlyBlockState>.CopyTo(IReadOnlyBlockState[] array, int index) { CopyTo((IWriteableBlockState[])array, index); }
        bool ICollection<IReadOnlyBlockState>.IsReadOnly { get { return ((ICollection<IWriteableBlockState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBlockState>.Remove(IReadOnlyBlockState item) { return Remove((IWriteableBlockState)item); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new WriteableBlockStateReadOnlyList(this);
        }
    }
}
