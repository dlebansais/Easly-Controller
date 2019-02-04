#pragma warning disable 1591

namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    public interface IFocusOperationGroupList : IFrameOperationGroupList, IList<IFocusOperationGroup>, IReadOnlyList<IFocusOperationGroup>
    {
        new int Count { get; }
        new IFocusOperationGroup this[int index] { get; set; }
        new IEnumerator<IFocusOperationGroup> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    internal class FocusOperationGroupList : Collection<IFocusOperationGroup>, IFocusOperationGroupList
    {
        #region Writeable
        bool ICollection<IWriteableOperationGroup>.IsReadOnly { get { return ((ICollection<IFocusOperationGroup>)this).IsReadOnly; } }
        public void Add(IWriteableOperationGroup item) { base.Add((IFocusOperationGroup)item); }
        public void Insert(int index, IWriteableOperationGroup item) { base.Insert(index, (IFocusOperationGroup)item); }
        IWriteableOperationGroup IWriteableOperationGroupList.this[int index] { get { return base[index]; } set { base[index] = (IFocusOperationGroup)value; } }
        IWriteableOperationGroup IList<IWriteableOperationGroup>.this[int index] { get { return base[index]; } set { base[index] = (IFocusOperationGroup)value; } }
        IWriteableOperationGroup IReadOnlyList<IWriteableOperationGroup>.this[int index] { get { return base[index]; } }
        public bool Remove(IWriteableOperationGroup item) { return base.Remove((IFocusOperationGroup)item); }
        public void CopyTo(IWriteableOperationGroup[] array, int index) { base.CopyTo((IFocusOperationGroup[])array, index); }
        public bool Contains(IWriteableOperationGroup value) { return base.Contains((IFocusOperationGroup)value); }
        public int IndexOf(IWriteableOperationGroup value) { return base.IndexOf((IFocusOperationGroup)value); }
        IEnumerator<IWriteableOperationGroup> IWriteableOperationGroupList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableOperationGroup> IEnumerable<IWriteableOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        bool ICollection<IFrameOperationGroup>.IsReadOnly { get { return ((ICollection<IFocusOperationGroup>)this).IsReadOnly; } }
        public void Add(IFrameOperationGroup item) { base.Add((IFocusOperationGroup)item); }
        public void Insert(int index, IFrameOperationGroup item) { base.Insert(index, (IFocusOperationGroup)item); }
        public new IFrameOperationGroup this[int index] { get { return base[index]; } set { base[index] = (IFocusOperationGroup)value; } }
        public bool Remove(IFrameOperationGroup item) { return base.Remove((IFocusOperationGroup)item); }
        public void CopyTo(IFrameOperationGroup[] array, int index) { base.CopyTo((IFocusOperationGroup[])array, index); }
        public bool Contains(IFrameOperationGroup value) { return base.Contains((IFocusOperationGroup)value); }
        public int IndexOf(IFrameOperationGroup value) { return base.IndexOf((IFocusOperationGroup)value); }
        IEnumerator<IFrameOperationGroup> IFrameOperationGroupList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameOperationGroup> IEnumerable<IFrameOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        #endregion
    }
}
