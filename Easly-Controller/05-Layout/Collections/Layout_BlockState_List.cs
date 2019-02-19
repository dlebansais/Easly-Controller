#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public interface ILayoutBlockStateList : IFocusBlockStateList, IList<ILayoutBlockState>, IReadOnlyList<ILayoutBlockState>
    {
        new ILayoutBlockState this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutBlockState> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    internal class LayoutBlockStateList : Collection<ILayoutBlockState>, ILayoutBlockStateList
    {
        #region ReadOnly
        IReadOnlyBlockState IReadOnlyBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBlockState)value; } }
        IReadOnlyBlockState IList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBlockState)value; } }
        int IList<IReadOnlyBlockState>.IndexOf(IReadOnlyBlockState value) { return IndexOf((ILayoutBlockState)value); }
        void IList<IReadOnlyBlockState>.Insert(int index, IReadOnlyBlockState item) { Insert(index, (ILayoutBlockState)item); }
        void ICollection<IReadOnlyBlockState>.Add(IReadOnlyBlockState item) { Add((ILayoutBlockState)item); }
        bool ICollection<IReadOnlyBlockState>.Contains(IReadOnlyBlockState value) { return Contains((ILayoutBlockState)value); }
        void ICollection<IReadOnlyBlockState>.CopyTo(IReadOnlyBlockState[] array, int index) { CopyTo((ILayoutBlockState[])array, index); }
        bool ICollection<IReadOnlyBlockState>.IsReadOnly { get { return ((ICollection<ILayoutBlockState>)this).IsReadOnly; } }
        bool ICollection<IReadOnlyBlockState>.Remove(IReadOnlyBlockState item) { return Remove((ILayoutBlockState)item); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBlockState)value; } }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateList.GetEnumerator() { return GetEnumerator(); }
        IWriteableBlockState IList<IWriteableBlockState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBlockState)value; } }
        int IList<IWriteableBlockState>.IndexOf(IWriteableBlockState value) { return IndexOf((ILayoutBlockState)value); }
        void IList<IWriteableBlockState>.Insert(int index, IWriteableBlockState item) { Insert(index, (ILayoutBlockState)item); }
        void ICollection<IWriteableBlockState>.Add(IWriteableBlockState item) { Add((ILayoutBlockState)item); }
        bool ICollection<IWriteableBlockState>.Contains(IWriteableBlockState value) { return Contains((ILayoutBlockState)value); }
        void ICollection<IWriteableBlockState>.CopyTo(IWriteableBlockState[] array, int index) { CopyTo((ILayoutBlockState[])array, index); }
        bool ICollection<IWriteableBlockState>.IsReadOnly { get { return ((ICollection<ILayoutBlockState>)this).IsReadOnly; } }
        bool ICollection<IWriteableBlockState>.Remove(IWriteableBlockState item) { return Remove((ILayoutBlockState)item); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameBlockState IFrameBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBlockState)value; } }
        IEnumerator<IFrameBlockState> IFrameBlockStateList.GetEnumerator() { return GetEnumerator(); }
        IFrameBlockState IList<IFrameBlockState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBlockState)value; } }
        int IList<IFrameBlockState>.IndexOf(IFrameBlockState value) { return IndexOf((ILayoutBlockState)value); }
        void IList<IFrameBlockState>.Insert(int index, IFrameBlockState item) { Insert(index, (ILayoutBlockState)item); }
        void ICollection<IFrameBlockState>.Add(IFrameBlockState item) { Add((ILayoutBlockState)item); }
        bool ICollection<IFrameBlockState>.Contains(IFrameBlockState value) { return Contains((ILayoutBlockState)value); }
        void ICollection<IFrameBlockState>.CopyTo(IFrameBlockState[] array, int index) { CopyTo((ILayoutBlockState[])array, index); }
        bool ICollection<IFrameBlockState>.IsReadOnly { get { return ((ICollection<ILayoutBlockState>)this).IsReadOnly; } }
        bool ICollection<IFrameBlockState>.Remove(IFrameBlockState item) { return Remove((ILayoutBlockState)item); }
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { return GetEnumerator(); }
        IFrameBlockState IReadOnlyList<IFrameBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusBlockState IFocusBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBlockState)value; } }
        IEnumerator<IFocusBlockState> IFocusBlockStateList.GetEnumerator() { return GetEnumerator(); }
        IFocusBlockState IList<IFocusBlockState>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutBlockState)value; } }
        int IList<IFocusBlockState>.IndexOf(IFocusBlockState value) { return IndexOf((ILayoutBlockState)value); }
        void IList<IFocusBlockState>.Insert(int index, IFocusBlockState item) { Insert(index, (ILayoutBlockState)item); }
        void ICollection<IFocusBlockState>.Add(IFocusBlockState item) { Add((ILayoutBlockState)item); }
        bool ICollection<IFocusBlockState>.Contains(IFocusBlockState value) { return Contains((ILayoutBlockState)value); }
        void ICollection<IFocusBlockState>.CopyTo(IFocusBlockState[] array, int index) { CopyTo((ILayoutBlockState[])array, index); }
        bool ICollection<IFocusBlockState>.IsReadOnly { get { return ((ICollection<ILayoutBlockState>)this).IsReadOnly; } }
        bool ICollection<IFocusBlockState>.Remove(IFocusBlockState item) { return Remove((ILayoutBlockState)item); }
        IEnumerator<IFocusBlockState> IEnumerable<IFocusBlockState>.GetEnumerator() { return GetEnumerator(); }
        IFocusBlockState IReadOnlyList<IFocusBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IReadOnlyBlockStateReadOnlyList ToReadOnly()
        {
            return new LayoutBlockStateReadOnlyList(this);
        }
    }
}
