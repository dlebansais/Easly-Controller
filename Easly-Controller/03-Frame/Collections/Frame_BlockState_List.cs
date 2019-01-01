using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Frame
{
    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public interface IFrameBlockStateList : IWriteableBlockStateList, IList<IFrameBlockState>, IReadOnlyList<IFrameBlockState>
    {
        new int Count { get; }
        new IFrameBlockState this[int index] { get; set; }
    }

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public class FrameBlockStateList : Collection<IFrameBlockState>, IFrameBlockStateList
    {
        #region ReadOnly
        bool ICollection<IReadOnlyBlockState>.IsReadOnly { get { return ((ICollection<IFrameBlockState>)this).IsReadOnly; } }
        public void Add(IReadOnlyBlockState item) { base.Add((IFrameBlockState)item); }
        public void Insert(int index, IReadOnlyBlockState item) { base.Insert(index, (IFrameBlockState)item); }
        IReadOnlyBlockState IReadOnlyBlockStateList.this[int index] { get { return base[index]; } set { base[index] = (IFrameBlockState)value; } }
        IReadOnlyBlockState IList<IReadOnlyBlockState>.this[int index] { get { return base[index]; } set { base[index] = (IFrameBlockState)value; } }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return base[index]; } }
        public bool Remove(IReadOnlyBlockState item) { return base.Remove((IFrameBlockState)item); }
        public void CopyTo(IReadOnlyBlockState[] array, int index) { base.CopyTo((IFrameBlockState[])array, index); }
        public bool Contains(IReadOnlyBlockState value) { return base.Contains((IFrameBlockState)value); }
        public int IndexOf(IReadOnlyBlockState value) { return base.IndexOf((IFrameBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        bool ICollection<IWriteableBlockState>.IsReadOnly { get { return ((ICollection<IFrameBlockState>)this).IsReadOnly; } }
        public void Add(IWriteableBlockState item) { base.Add((IFrameBlockState)item); }
        public void Insert(int index, IWriteableBlockState item) { base.Insert(index, (IFrameBlockState)item); }
        public new IWriteableBlockState this[int index] { get { return base[index]; } set { base[index] = (IFrameBlockState)value; } }
        public bool Remove(IWriteableBlockState item) { return base.Remove((IFrameBlockState)item); }
        public void CopyTo(IWriteableBlockState[] array, int index) { base.CopyTo((IFrameBlockState[])array, index); }
        public bool Contains(IWriteableBlockState value) { return base.Contains((IFrameBlockState)value); }
        public int IndexOf(IWriteableBlockState value) { return base.IndexOf((IFrameBlockState)value); }
        public new IEnumerator<IWriteableBlockState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
