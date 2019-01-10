using EaslyController.ReadOnly;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591

namespace EaslyController.Writeable
{
    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    public interface IWriteableIndexCollectionList : IReadOnlyIndexCollectionList, IList<IWriteableIndexCollection>, IReadOnlyList<IWriteableIndexCollection>
    {
        new int Count { get; }
        new IWriteableIndexCollection this[int index] { get; set; }
        new IEnumerator<IWriteableIndexCollection> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxIndexCollection
    /// </summary>
    public class WriteableIndexCollectionList : Collection<IWriteableIndexCollection>, IWriteableIndexCollectionList
    {
        #region ReadOnly
        public new IReadOnlyIndexCollection this[int index] { get { return base[index]; } set { base[index] = (IWriteableIndexCollection)value; } }
        public void Add(IReadOnlyIndexCollection item) { base.Add((IWriteableIndexCollection)item); }
        public void Insert(int index, IReadOnlyIndexCollection item) { base.Insert(index, (IWriteableIndexCollection)item); }
        public bool Remove(IReadOnlyIndexCollection item) { return base.Remove((IWriteableIndexCollection)item); }
        public void CopyTo(IReadOnlyIndexCollection[] array, int index) { base.CopyTo((IWriteableIndexCollection[])array, index); }
        bool ICollection<IReadOnlyIndexCollection>.IsReadOnly { get { return ((ICollection<IWriteableIndexCollection>)this).IsReadOnly; } }
        public bool Contains(IReadOnlyIndexCollection value) { return base.Contains((IWriteableIndexCollection)value); }
        public int IndexOf(IReadOnlyIndexCollection value) { return base.IndexOf((IWriteableIndexCollection)value); }
        public new IEnumerator<IReadOnlyIndexCollection> GetEnumerator() { return base.GetEnumerator(); }
        #endregion
    }
}
