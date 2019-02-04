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
        public void Add(IReadOnlyBlockState item) { base.Add((IFrameBlockState)item); }
        public void Insert(int index, IReadOnlyBlockState item) { base.Insert(index, (IFrameBlockState)item); }
        public bool Remove(IReadOnlyBlockState item) { return base.Remove((IFrameBlockState)item); }
        public void CopyTo(IReadOnlyBlockState[] array, int index) { base.CopyTo((IFrameBlockState[])array, index); }
        public bool Contains(IReadOnlyBlockState value) { return base.Contains((IFrameBlockState)value); }
        public int IndexOf(IReadOnlyBlockState value) { return base.IndexOf((IFrameBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateList.this[int index] { get { return this[index]; } set { this[index] = (IFrameBlockState)value; } }
        IWriteableBlockState IList<IWriteableBlockState>.this[int index] { get { return this[index]; } set { this[index] = (IFrameBlockState)value; } }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return this[index]; } }
        bool ICollection<IWriteableBlockState>.IsReadOnly { get { return ((ICollection<IFrameBlockState>)this).IsReadOnly; } }
        public void Add(IWriteableBlockState item) { base.Add((IFrameBlockState)item); }
        public void Insert(int index, IWriteableBlockState item) { base.Insert(index, (IFrameBlockState)item); }
        public bool Remove(IWriteableBlockState item) { return base.Remove((IFrameBlockState)item); }
        public void CopyTo(IWriteableBlockState[] array, int index) { base.CopyTo((IFrameBlockState[])array, index); }
        public bool Contains(IWriteableBlockState value) { return base.Contains((IFrameBlockState)value); }
        public int IndexOf(IWriteableBlockState value) { return base.IndexOf((IFrameBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
