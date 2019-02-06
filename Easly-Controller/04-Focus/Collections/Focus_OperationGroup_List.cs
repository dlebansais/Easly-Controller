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
        IWriteableOperationGroup IWriteableOperationGroupList.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperationGroup)value; } }
        IWriteableOperationGroup IList<IWriteableOperationGroup>.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperationGroup)value; } }
        IWriteableOperationGroup IReadOnlyList<IWriteableOperationGroup>.this[int index] { get { return this[index]; } }
        bool ICollection<IWriteableOperationGroup>.IsReadOnly { get { return ((ICollection<IFocusOperationGroup>)this).IsReadOnly; } }
        void ICollection<IWriteableOperationGroup>.Add(IWriteableOperationGroup item) { Add((IFocusOperationGroup)item); }
        void IList<IWriteableOperationGroup>.Insert(int index, IWriteableOperationGroup item) { Insert(index, (IFocusOperationGroup)item); }
        bool ICollection<IWriteableOperationGroup>.Remove(IWriteableOperationGroup item) { return Remove((IFocusOperationGroup)item); }
        void ICollection<IWriteableOperationGroup>.CopyTo(IWriteableOperationGroup[] array, int index) { CopyTo((IFocusOperationGroup[])array, index); }
        bool ICollection<IWriteableOperationGroup>.Contains(IWriteableOperationGroup value) { return Contains((IFocusOperationGroup)value); }
        int IList<IWriteableOperationGroup>.IndexOf(IWriteableOperationGroup value) { return IndexOf((IFocusOperationGroup)value); }
        IEnumerator<IWriteableOperationGroup> IWriteableOperationGroupList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IWriteableOperationGroup> IEnumerable<IWriteableOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Frame
        IFrameOperationGroup IFrameOperationGroupList.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperationGroup)value; } }
        IFrameOperationGroup IList<IFrameOperationGroup>.this[int index] { get { return this[index]; } set { this[index] = (IFocusOperationGroup)value; } }
        IFrameOperationGroup IReadOnlyList<IFrameOperationGroup>.this[int index] { get { return this[index]; } }
        bool ICollection<IFrameOperationGroup>.IsReadOnly { get { return ((ICollection<IFocusOperationGroup>)this).IsReadOnly; } }
        void ICollection<IFrameOperationGroup>.Add(IFrameOperationGroup item) { Add((IFocusOperationGroup)item); }
        void IList<IFrameOperationGroup>.Insert(int index, IFrameOperationGroup item) { Insert(index, (IFocusOperationGroup)item); }
        bool ICollection<IFrameOperationGroup>.Remove(IFrameOperationGroup item) { return Remove((IFocusOperationGroup)item); }
        void ICollection<IFrameOperationGroup>.CopyTo(IFrameOperationGroup[] array, int index) { CopyTo((IFocusOperationGroup[])array, index); }
        bool ICollection<IFrameOperationGroup>.Contains(IFrameOperationGroup value) { return Contains((IFocusOperationGroup)value); }
        int IList<IFrameOperationGroup>.IndexOf(IFrameOperationGroup value) { return IndexOf((IFocusOperationGroup)value); }
        IEnumerator<IFrameOperationGroup> IFrameOperationGroupList.GetEnumerator() { return GetEnumerator(); }
        IEnumerator<IFrameOperationGroup> IEnumerable<IFrameOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        #endregion

        public virtual IWriteableOperationGroupReadOnlyList ToReadOnly()
        {
            return new FocusOperationGroupReadOnlyList(this);
        }
    }
}
