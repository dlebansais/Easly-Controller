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
        bool ICollection<IReadOnlyBlockState>.IsReadOnly { get { return ((ICollection<IFocusBlockState>)this).IsReadOnly; } }
        public void Add(IReadOnlyBlockState item) { base.Add((IFocusBlockState)item); }
        public void Insert(int index, IReadOnlyBlockState item) { base.Insert(index, (IFocusBlockState)item); }
        IReadOnlyBlockState IReadOnlyBlockStateList.this[int index] { get { return base[index]; } set { base[index] = (IFocusBlockState)value; } }
        IReadOnlyBlockState IList<IReadOnlyBlockState>.this[int index] { get { return base[index]; } set { base[index] = (IFocusBlockState)value; } }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return base[index]; } }
        public bool Remove(IReadOnlyBlockState item) { return base.Remove((IFocusBlockState)item); }
        public void CopyTo(IReadOnlyBlockState[] array, int index) { base.CopyTo((IFocusBlockState[])array, index); }
        public bool Contains(IReadOnlyBlockState value) { return base.Contains((IFocusBlockState)value); }
        public int IndexOf(IReadOnlyBlockState value) { return base.IndexOf((IFocusBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        bool ICollection<IWriteableBlockState>.IsReadOnly { get { return ((ICollection<IFocusBlockState>)this).IsReadOnly; } }
        public void Add(IWriteableBlockState item) { base.Add((IFocusBlockState)item); }
        public void Insert(int index, IWriteableBlockState item) { base.Insert(index, (IFocusBlockState)item); }
        IWriteableBlockState IWriteableBlockStateList.this[int index] { get { return base[index]; } set { base[index] = (IFocusBlockState)value; } }
        IWriteableBlockState IList<IWriteableBlockState>.this[int index] { get { return base[index]; } set { base[index] = (IFocusBlockState)value; } }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return base[index]; } }
        public bool Remove(IWriteableBlockState item) { return base.Remove((IFocusBlockState)item); }
        public void CopyTo(IWriteableBlockState[] array, int index) { base.CopyTo((IFocusBlockState[])array, index); }
        public bool Contains(IWriteableBlockState value) { return base.Contains((IFocusBlockState)value); }
        public int IndexOf(IWriteableBlockState value) { return base.IndexOf((IFocusBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        bool ICollection<IFrameBlockState>.IsReadOnly { get { return ((ICollection<IFocusBlockState>)this).IsReadOnly; } }
        public void Add(IFrameBlockState item) { base.Add((IFocusBlockState)item); }
        public void Insert(int index, IFrameBlockState item) { base.Insert(index, (IFocusBlockState)item); }
        public new IFrameBlockState this[int index] { get { return base[index]; } set { base[index] = (IFocusBlockState)value; } }
        public bool Remove(IFrameBlockState item) { return base.Remove((IFocusBlockState)item); }
        public void CopyTo(IFrameBlockState[] array, int index) { base.CopyTo((IFocusBlockState[])array, index); }
        public bool Contains(IFrameBlockState value) { return base.Contains((IFocusBlockState)value); }
        public int IndexOf(IFrameBlockState value) { return base.IndexOf((IFocusBlockState)value); }
        IEnumerator<IFrameBlockState> IFrameBlockStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
