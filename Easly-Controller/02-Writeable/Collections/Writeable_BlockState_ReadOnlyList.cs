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
        new IWriteableBlockState this[int index] { get; }
        new int Count { get; }
        bool Contains(IWriteableBlockState value);
        new IEnumerator<IWriteableBlockState> GetEnumerator();
        int IndexOf(IWriteableBlockState value);
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
        bool IReadOnlyBlockStateReadOnlyList.Contains(IReadOnlyBlockState value) { return Contains((IWriteableBlockState)value); }
        int IReadOnlyBlockStateReadOnlyList.IndexOf(IReadOnlyBlockState value) { return IndexOf((IWriteableBlockState)value); }
        IEnumerator<IReadOnlyBlockState> IEnumerable<IReadOnlyBlockState>.GetEnumerator() { return GetEnumerator(); }
        IReadOnlyBlockState IReadOnlyList<IReadOnlyBlockState>.this[int index] { get { return this[index]; } }
        #endregion
    }
}
