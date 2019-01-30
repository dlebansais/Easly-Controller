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
    public class WriteableBlockStateReadOnlyList : ReadOnlyCollection<IWriteableBlockState>, IWriteableBlockStateReadOnlyList
    {
        public WriteableBlockStateReadOnlyList(IWriteableBlockStateList list)
            : base(list)
        {
        }

        #region ReadOnly
        public new IReadOnlyBlockState this[int index] { get { return base[index]; } }
        public bool Contains(IReadOnlyBlockState value) { return base.Contains((IWriteableBlockState)value); }
        public int IndexOf(IReadOnlyBlockState value) { return base.IndexOf((IWriteableBlockState)value); }
        public new IEnumerator<IReadOnlyBlockState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
