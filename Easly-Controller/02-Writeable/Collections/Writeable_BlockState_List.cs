#pragma warning disable 1591

namespace EaslyController.Writeable
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.ReadOnly;

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public interface IWriteableBlockStateList : IReadOnlyBlockStateList, IList<IWriteableBlockState>, IReadOnlyList<IWriteableBlockState>
    {
        new int Count { get; }
        new IWriteableBlockState this[int index] { get; set; }
        new IEnumerator<IWriteableBlockState> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxBlockState
    /// </summary>
    public class WriteableBlockStateList : Collection<IWriteableBlockState>, IWriteableBlockStateList
    {
        #region ReadOnly
        bool ICollection<IReadOnlyBlockState>.IsReadOnly { get { return ((ICollection<IWriteableBlockState>)this).IsReadOnly; } }
        public void Add(IReadOnlyBlockState item) { base.Add((IWriteableBlockState)item); }
        public void Insert(int index, IReadOnlyBlockState item) { base.Insert(index, (IWriteableBlockState)item); }
        public new IReadOnlyBlockState this[int index] { get { return base[index]; } set { base[index] = (IWriteableBlockState)value; } }
        public bool Remove(IReadOnlyBlockState item) { return base.Remove((IWriteableBlockState)item); }
        public void CopyTo(IReadOnlyBlockState[] array, int index) { base.CopyTo((IWriteableBlockState[])array, index); }
        public bool Contains(IReadOnlyBlockState value) { return base.Contains((IWriteableBlockState)value); }
        public int IndexOf(IReadOnlyBlockState value) { return base.IndexOf((IWriteableBlockState)value); }
        public new IEnumerator<IReadOnlyBlockState> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
