#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public interface IFocusBlockStateList : IFrameBlockStateList, IList<IFocusBlockState>, IReadOnlyList<IFocusBlockState>
    {
        new int Count { get; }
        new IFocusBlockState this[int index] { get; set; }
        new IEnumerator<IFocusBlockState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    internal class FocusBlockStateList : Collection<IFocusBlockState>, IFocusBlockStateList
    {
        #region ReadOnly
        IReadOnlyBlockState IReadOnlyBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBlockState)value; } }
        IReadOnlyBlockState IList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBlockState)value; } }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        bool ICollection<IReadOnlyBlockState>.IsReadOnly { get { return ((ICollection<IFocusBlockState>)this).IsReadOnly; } }
        void ICollection<IReadOnlyBlockState>.Add(IReadOnlyBlockState item) { Add((IFocusBlockState)item); }
        void IList<IReadOnlyBlockState>.Insert(int index, IReadOnlyBlockState item) { Insert(index, (IFocusBlockState)item); }
        bool ICollection<IReadOnlyBlockState>.Remove(IReadOnlyBlockState item) { return Remove((IFocusBlockState)item); }
        void ICollection<IReadOnlyBlockState>.CopyTo(IReadOnlyBlockState[] array, int index) { CopyTo((IFocusBlockState[])array, index); }
        bool ICollection<IReadOnlyBlockState>.Contains(IReadOnlyBlockState value) { return Contains((IFocusBlockState)value); }
        int IList<IReadOnlyBlockState>.IndexOf(IReadOnlyBlockState value) { return IndexOf((IFocusBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBlockState)value; } }
        IWriteableBlockState IList<IWriteableBlockState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBlockState)value; } }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return this[index]; } }
        bool ICollection<IWriteableBlockState>.IsReadOnly { get { return ((ICollection<IFocusBlockState>)this).IsReadOnly; } }
        void ICollection<IWriteableBlockState>.Add(IWriteableBlockState item) { Add((IFocusBlockState)item); }
        void IList<IWriteableBlockState>.Insert(int index, IWriteableBlockState item) { Insert(index, (IFocusBlockState)item); }
        bool ICollection<IWriteableBlockState>.Remove(IWriteableBlockState item) { return Remove((IFocusBlockState)item); }
        void ICollection<IWriteableBlockState>.CopyTo(IWriteableBlockState[] array, int index) { CopyTo((IFocusBlockState[])array, index); }
        bool ICollection<IWriteableBlockState>.Contains(IWriteableBlockState value) { return Contains((IFocusBlockState)value); }
        int IList<IWriteableBlockState>.IndexOf(IWriteableBlockState value) { return IndexOf((IFocusBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameBlockState IFrameBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (IFocusBlockState)value; } }
        IFrameBlockState IList<IFrameBlockState>.this[int index] { get { return this[index]; } set { this[index] = (IFocusBlockState)value; } }
        IFrameBlockState IReadOnlyList<IFrameBlockState>.this[int index] { get { return this[index]; } }
        bool ICollection<IFrameBlockState>.IsReadOnly { get { return ((ICollection<IFocusBlockState>)this).IsReadOnly; } }
        void ICollection<IFrameBlockState>.Add(IFrameBlockState item) { Add((IFocusBlockState)item); }
        void IList<IFrameBlockState>.Insert(int index, IFrameBlockState item) { Insert(index, (IFocusBlockState)item); }
        bool ICollection<IFrameBlockState>.Remove(IFrameBlockState item) { return Remove((IFocusBlockState)item); }
        void ICollection<IFrameBlockState>.CopyTo(IFrameBlockState[] array, int index) { CopyTo((IFocusBlockState[])array, index); }
        bool ICollection<IFrameBlockState>.Contains(IFrameBlockState value) { return Contains((IFocusBlockState)value); }
        int IList<IFrameBlockState>.IndexOf(IFrameBlockState value) { return IndexOf((IFocusBlockState)value); }
        IEnumerator<IFrameBlockState> IFrameBlockStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public virtual IReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new FocusBlockStateReadOnlyList(this);
        }
    }
}
