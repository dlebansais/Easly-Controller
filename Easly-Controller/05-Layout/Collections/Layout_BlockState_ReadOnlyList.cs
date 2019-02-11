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
    /// Read-only list of IxxxBlockState
    /// </summary>
    public interface ILayoutBlockStateReadOnlyList : IFocusBlockStateReadOnlyList, IReadOnlyList<ILayoutBlockState>
    {
        new ILayoutBlockState this[int index] { get; }
        new int Count { get; }
        bool Contains(ILayoutBlockState value);
        new IEnumerator<ILayoutBlockState> GetEnumerator();
        int IndexOf(ILayoutBlockState value);
    }

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    internal class LayoutBlockStateReadOnlyList : ReadOnlyCollection<ILayoutBlockState>, ILayoutBlockStateReadOnlyList
    {
        public LayoutBlockStateReadOnlyList(ILayoutBlockStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyBlockStateReadOnlyList.Contains(IReadOnlyBlockState value) { return Contains((ILayoutBlockState)value); }
        int IReadOnlyBlockStateReadOnlyList.IndexOf(IReadOnlyBlockState value) { return IndexOf((ILayoutBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteableBlockStateReadOnlyList.Contains(IWriteableBlockState value) { return Contains((ILayoutBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteableBlockStateReadOnlyList.IndexOf(IWriteableBlockState value) { return IndexOf((ILayoutBlockState)value); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameBlockState IFrameBlockStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFrameBlockStateReadOnlyList.Contains(IFrameBlockState value) { return Contains((ILayoutBlockState)value); }
        IEnumerator<IFrameBlockState> IFrameBlockStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFrameBlockStateReadOnlyList.IndexOf(IFrameBlockState value) { return IndexOf((ILayoutBlockState)value); }
        IEnumerator<IFrameBlockState> IEnumerable<IFrameBlockState>.GetEnumerator() { return GetEnumerator(); }
        IFrameBlockState IReadOnlyList<IFrameBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusBlockState IFocusBlockStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IFocusBlockStateReadOnlyList.Contains(IFocusBlockState value) { return Contains((ILayoutBlockState)value); }
        IEnumerator<IFocusBlockState> IFocusBlockStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IFocusBlockStateReadOnlyList.IndexOf(IFocusBlockState value) { return IndexOf((ILayoutBlockState)value); }
        IEnumerator<IFocusBlockState> IEnumerable<IFocusBlockState>.GetEnumerator() { return GetEnumerator(); }
        IFocusBlockState IReadOnlyList<IFocusBlockState>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
