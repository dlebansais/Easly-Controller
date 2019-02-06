#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;
    using EaslyController.Writeable;

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    public interface IFrameBlockStateReadOnlyList : IWriteableBlockStateReadOnlyList, IReadOnlyList<IFrameBlockState>
    {
        new IFrameBlockState this[int index] { get; }
        new int Count { get; }
        bool Contains(IFrameBlockState value);
        new IEnumerator<IFrameBlockState> GetEnumerator();
        int IndexOf(IFrameBlockState value);
    }

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    internal class FrameBlockStateReadOnlyList : ReadOnlyCollection<IFrameBlockState>, IFrameBlockStateReadOnlyList
    {
        public FrameBlockStateReadOnlyList(IFrameBlockStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        bool IReadOnlyBlockStateReadOnlyList.Contains(IReadOnlyBlockState value) { return Contains((IFrameBlockState)value); }
        int IReadOnlyBlockStateReadOnlyList.IndexOf(IReadOnlyBlockState value) { return IndexOf((IFrameBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateReadOnlyList.this[int index] { get { return this[index]; } }
        bool IWriteableBlockStateReadOnlyList.Contains(IWriteableBlockState value) { return Contains((IFrameBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        int IWriteableBlockStateReadOnlyList.IndexOf(IWriteableBlockState value) { return IndexOf((IFrameBlockState)value); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
