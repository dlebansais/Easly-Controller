#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    public interface IFocusBlockStateReadOnlyList : IFrameBlockStateReadOnlyList, IReadOnlyList<IFocusBlockState>
    {
        new int Count { get; }
        new IFocusBlockState this[int index] { get; }
        bool Contains(IFocusBlockState value);
        int IndexOf(IFocusBlockState value);
        new IEnumerator<IFocusBlockState> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    internal class FocusBlockStateReadOnlyList : ReadOnlyCollection<IFocusBlockState>, IFocusBlockStateReadOnlyList
    {
        public FocusBlockStateReadOnlyList(IFocusBlockStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        public new IReadOnlyBlockState this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyBlockState value) { return base.Contains((IFocusBlockState)value); }
        public int IndexOf(IReadOnlyBlockState value) { return base.IndexOf((IFocusBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateReadOnlyList.this[int index] { get { return base[index]; } }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return base[index]; } }
        public bool Contains(IWriteableBlockState value) { return base.Contains((IFocusBlockState)value); }
        public int IndexOf(IWriteableBlockState value) { return base.IndexOf((IFocusBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameBlockState IFrameBlockStateReadOnlyList.this[int index] { get { return base[index]; } }
        IFrameBlockState IReadOnlyList<IFrameBlockState>.this[int index] { get { return base[index]; } }
        public bool Contains(IFrameBlockState value) { return base.Contains((IFocusBlockState)value); }
        public int IndexOf(IFrameBlockState value) { return base.IndexOf((IFocusBlockState)value); }
        IEnumerator<IFrameBlockState> IFrameBlockStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
