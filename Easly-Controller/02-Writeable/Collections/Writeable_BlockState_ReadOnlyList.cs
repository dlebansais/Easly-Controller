#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    public interface IWriteableBlockStateReadOnlyList : IReadOnlyBlockStateReadOnlyList, IReadOnlyList<IWriteableBlockState>
    {
        new int Count { get; }
        new IWriteableBlockState this[int index] { get; }
        bool Contains(IWriteableBlockState value);
        int IndexOf(IWriteableBlockState value);
        new IEnumerator<IWriteableBlockState> GetEnumerator();
    }

    /// <summary>
    /// Read-only list of IxxxBlockState
    /// </summary>
    internal class WriteableBlockStateReadOnlyList : ReadOnlyCollection<IWriteableBlockState>, IWriteableBlockStateReadOnlyList
    {
        public WriteableBlockStateReadOnlyList(IWriteableBlockStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        bool IReadOnlyBlockStateReadOnlyList.Contains(IReadOnlyBlockState value) { return Contains((IWriteableBlockState)value); }
        int IReadOnlyBlockStateReadOnlyList.IndexOf(IReadOnlyBlockState value) { return IndexOf((IWriteableBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
