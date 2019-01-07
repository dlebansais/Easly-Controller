﻿using EaslyController.ReadOnly;
using EaslyController.Writeable;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EaslyController.Frame
{
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
    public class FrameBlockStateReadOnlyList : ReadOnlyCollection<IFrameBlockState>, IFrameBlockStateReadOnlyList
    {
        public FrameBlockStateReadOnlyList(IFrameBlockStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        public new IReadOnlyBlockState this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyBlockState value) { return base.Contains((IFrameBlockState)value); }
        public int IndexOf(IReadOnlyBlockState value) { return base.IndexOf((IFrameBlockState)value); }
        public new IEnumerator<IReadOnlyBlockState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion

        #region Writeable
        IWriteableBlockState IWriteableBlockStateReadOnlyList.this[int index] { get { return base[index]; } }
        IWriteableBlockState IReadOnlyList<IWriteableBlockState>.this[int index] { get { return base[index]; } }
        public bool Contains(IWriteableBlockState value) { return base.Contains((IFrameBlockState)value); }
        public int IndexOf(IWriteableBlockState value) { return base.IndexOf((IFrameBlockState)value); }
        IEnumerator<IWriteableBlockState> IWriteableBlockStateReadOnlyList.GetEnumerator() { return base.GetEnumerator(); }
        IEnumerator<IWriteableBlockState> IEnumerable<IWriteableBlockState>.GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}