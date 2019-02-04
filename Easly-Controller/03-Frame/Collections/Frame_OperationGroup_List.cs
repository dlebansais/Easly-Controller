#pragma warning disable 1591

namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    public interface IFrameOperationGroupList : IWriteableOperationGroupList, IList<IFrameOperationGroup>, IReadOnlyList<IFrameOperationGroup>
    {
        new int Count { get; }
        new IFrameOperationGroup this[int index] { get; set; }
        new IEnumerator<IFrameOperationGroup> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    internal class FrameOperationGroupList : Collection<IFrameOperationGroup>, IFrameOperationGroupList
    {
        #region Writeable
        bool ICollection<IWriteableOperationGroup>.IsReadOnly { get { return ((ICollection<IFrameOperationGroup>)this).IsReadOnly; } }
        public void Add(IWriteableOperationGroup item) { base.Add((IFrameOperationGroup)item); }
        public void Insert(int index, IWriteableOperationGroup item) { base.Insert(index, (IFrameOperationGroup)item); }
        public new IWriteableOperationGroup this[int index] { get { return base[index]; } set { base[index] = (IFrameOperationGroup)value; } }
        public bool Remove(IWriteableOperationGroup item) { return base.Remove((IFrameOperationGroup)item); }
        public void CopyTo(IWriteableOperationGroup[] array, int index) { base.CopyTo((IFrameOperationGroup[])array, index); }
        public bool Contains(IWriteableOperationGroup value) { return base.Contains((IFrameOperationGroup)value); }
        public int IndexOf(IWriteableOperationGroup value) { return base.IndexOf((IFrameOperationGroup)value); }
        IEnumerator<IWriteableOperationGroup> IWriteableOperationGroupList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableOperationGroup> IEnumerable<IWriteableOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
