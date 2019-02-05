#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public interface IFrameBlockStateList : IWriteableBlockStateList, IList<IFrameBlockState>, IReadOnlyList<IFrameBlockState>
    {
        new int Count { get; }
        new IFrameBlockState this[int index] { get; set; }
        new IEnumerator<IFrameBlockState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    internal class FrameBlockStateList : Collection<IFrameBlockState>, IFrameBlockStateList
    {
        #region ReadOnly
        IReadOnlyBlockState IReadOnlyBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (IFrameBlockState)value; } }
        IReadOnlyBlockState IList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } set { this[index] = (IFrameBlockState)value; } }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        bool ICollection<IReadOnlyBlockState>.IsReadOnly { get { return ((ICollection<IFrameBlockState>)this).IsReadOnly; } }
        void ICollection<IReadOnlyBlockState>.Add(IReadOnlyBlockState item) { Add((IFrameBlockState)item); }
        void IList<IReadOnlyBlockState>.Insert(int index, IReadOnlyBlockState item) { Insert(index, (IFrameBlockState)item); }
        bool ICollection<IReadOnlyBlockState>.Remove(IReadOnlyBlockState item) { return Remove((IFrameBlockState)item); }
        void ICollection<IReadOnlyBlockState>.CopyTo(IReadOnlyBlockState[] array, int index) { CopyTo((IFrameBlockState[])array, index); }
        bool ICollection<IReadOnlyBlockState>.Contains(IReadOnlyBlockState value) { return Contains((IFrameBlockState)value); }
        int IList<IReadOnlyBlockState>.IndexOf(IReadOnlyBlockState value) { return IndexOf((IFrameBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (IFrameBlockState)value; } }
        IWriteableBlockState IList<IWriteableBlockState>.this[int index] { get { return this[index]; } set { this[index] = (IFrameBlockState)value; } }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return this[index]; } }
        bool ICollection<IWriteableBlockState>.IsReadOnly { get { return ((ICollection<IFrameBlockState>)this).IsReadOnly; } }
        void ICollection<IWriteableBlockState>.Add(IWriteableBlockState item) { Add((IFrameBlockState)item); }
        void IList<IWriteableBlockState>.Insert(int index, IWriteableBlockState item) { Insert(index, (IFrameBlockState)item); }
        bool ICollection<IWriteableBlockState>.Remove(IWriteableBlockState item) { return Remove((IFrameBlockState)item); }
        void ICollection<IWriteableBlockState>.CopyTo(IWriteableBlockState[] array, int index) { CopyTo((IFrameBlockState[])array, index); }
        bool ICollection<IWriteableBlockState>.Contains(IWriteableBlockState value) { return Contains((IFrameBlockState)value); }
        int IList<IWriteableBlockState>.IndexOf(IWriteableBlockState value) { return IndexOf((IFrameBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public virtual IReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new FrameBlockStateReadOnlyList(this);
        }
    }
}
