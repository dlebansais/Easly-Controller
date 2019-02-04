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
        new int Count { get; }
        new IWriteableBlockState this[int index] { get; set; }
        new IEnumerator<IWriteableBlockState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    internal class WriteableBlockStateList : Collection<IWriteableBlockState>, IWriteableBlockStateList
    {
        #region ReadOnly
        IReadOnlyBlockState IReadOnlyBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (IWriteableBlockState)value; } }
        IReadOnlyBlockState IList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } set { this[index] = (IWriteableBlockState)value; } }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        bool ICollection<IReadOnlyBlockState>.IsReadOnly { get { return ((ICollection<IWriteableBlockState>)this).IsReadOnly; } }
        public void Add(IReadOnlyBlockState item) { base.Add((IWriteableBlockState)item); }
        public void Insert(int index, IReadOnlyBlockState item) { base.Insert(index, (IWriteableBlockState)item); }
        public bool Remove(IReadOnlyBlockState item) { return base.Remove((IWriteableBlockState)item); }
        public void CopyTo(IReadOnlyBlockState[] array, int index) { base.CopyTo((IWriteableBlockState[])array, index); }
        public bool Contains(IReadOnlyBlockState value) { return base.Contains((IWriteableBlockState)value); }
        public int IndexOf(IReadOnlyBlockState value) { return base.IndexOf((IWriteableBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public virtual IReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new WriteableBlockStateReadOnlyList(this);
        }
    }
}
