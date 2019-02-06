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
        new int Count { get; }
        new IFrameBlockState this[int index] { get; }
        bool Contains(IFrameBlockState value);
        int IndexOf(IFrameBlockState value);
        new IEnumerator<IFrameBlockState> GetEnumerator();
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
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        bool IReadOnlyBlockStateReadOnlyList.Contains(IReadOnlyBlockState value) { return Contains((IFrameBlockState)value); }
        int IReadOnlyBlockStateReadOnlyList.IndexOf(IReadOnlyBlockState value) { return IndexOf((IFrameBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateReadOnlyList.this[int index] { get { return this[index]; } }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return this[index]; } }
        bool IWriteableBlockStateReadOnlyList.Contains(IWriteableBlockState value) { return Contains((IFrameBlockState)value); }
        int IWriteableBlockStateReadOnlyList.IndexOf(IWriteableBlockState value) { return IndexOf((IFrameBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateReadOnlyList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
