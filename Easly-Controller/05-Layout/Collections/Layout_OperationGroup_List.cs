#pragma warning disable 1591

namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Focus;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    public interface ILayoutOperationGroupList : IFocusOperationGroupList, IList<ILayoutOperationGroup>, IReadOnlyList<ILayoutOperationGroup>
    {
        new ILayoutOperationGroup this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<ILayoutOperationGroup> GetEnumerator();
        new void Clear();
    }

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    internal class LayoutOperationGroupList : Collection<ILayoutOperationGroup>, ILayoutOperationGroupList
    {
        #region Writeable
        IWriteableOperationGroup IWriteableOperationGroupList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperationGroup)value; } }
        IWriteableOperationGroup IList<IWriteableOperationGroup>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperationGroup)value; } }
        int IList<IWriteableOperationGroup>.IndexOf(IWriteableOperationGroup value) { return IndexOf((ILayoutOperationGroup)value); }
        void IList<IWriteableOperationGroup>.Insert(int index, IWriteableOperationGroup item) { Insert(index, (ILayoutOperationGroup)item); }
        void ICollection<IWriteableOperationGroup>.Add(IWriteableOperationGroup item) { Add((ILayoutOperationGroup)item); }
        bool ICollection<IWriteableOperationGroup>.Contains(IWriteableOperationGroup value) { return Contains((ILayoutOperationGroup)value); }
        void ICollection<IWriteableOperationGroup>.CopyTo(IWriteableOperationGroup[] array, int index) { CopyTo((ILayoutOperationGroup[])array, index); }
        bool ICollection<IWriteableOperationGroup>.IsReadOnly { get { return ((ICollection<ILayoutOperationGroup>)this).IsReadOnly; } }
        bool ICollection<IWriteableOperationGroup>.Remove(IWriteableOperationGroup item) { return Remove((ILayoutOperationGroup)item); }
        IEnumerator<IWriteableOperationGroup> IEnumerable<IWriteableOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        IWriteableOperationGroup IReadOnlyList<IWriteableOperationGroup>.this[int index] { get { return this[index]; } }
        #endregion

        #region Frame
        IFrameOperationGroup IFrameOperationGroupList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperationGroup)value; } }
        IEnumerator<IFrameOperationGroup> IFrameOperationGroupList.GetEnumerator() { return GetEnumerator(); }
        IFrameOperationGroup IList<IFrameOperationGroup>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperationGroup)value; } }
        int IList<IFrameOperationGroup>.IndexOf(IFrameOperationGroup value) { return IndexOf((ILayoutOperationGroup)value); }
        void IList<IFrameOperationGroup>.Insert(int index, IFrameOperationGroup item) { Insert(index, (ILayoutOperationGroup)item); }
        void ICollection<IFrameOperationGroup>.Add(IFrameOperationGroup item) { Add((ILayoutOperationGroup)item); }
        bool ICollection<IFrameOperationGroup>.Contains(IFrameOperationGroup value) { return Contains((ILayoutOperationGroup)value); }
        void ICollection<IFrameOperationGroup>.CopyTo(IFrameOperationGroup[] array, int index) { CopyTo((ILayoutOperationGroup[])array, index); }
        bool ICollection<IFrameOperationGroup>.IsReadOnly { get { return ((ICollection<ILayoutOperationGroup>)this).IsReadOnly; } }
        bool ICollection<IFrameOperationGroup>.Remove(IFrameOperationGroup item) { return Remove((ILayoutOperationGroup)item); }
        IEnumerator<IFrameOperationGroup> IEnumerable<IFrameOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        IFrameOperationGroup IReadOnlyList<IFrameOperationGroup>.this[int index] { get { return this[index]; } }
        #endregion

        #region Focus
        IFocusOperationGroup IFocusOperationGroupList.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperationGroup)value; } }
        IEnumerator<IFocusOperationGroup> IFocusOperationGroupList.GetEnumerator() { return GetEnumerator(); }
        IFocusOperationGroup IList<IFocusOperationGroup>.this[int index] { get { return this[index]; } set { this[index] = (ILayoutOperationGroup)value; } }
        int IList<IFocusOperationGroup>.IndexOf(IFocusOperationGroup value) { return IndexOf((ILayoutOperationGroup)value); }
        void IList<IFocusOperationGroup>.Insert(int index, IFocusOperationGroup item) { Insert(index, (ILayoutOperationGroup)item); }
        void ICollection<IFocusOperationGroup>.Add(IFocusOperationGroup item) { Add((ILayoutOperationGroup)item); }
        bool ICollection<IFocusOperationGroup>.Contains(IFocusOperationGroup value) { return Contains((ILayoutOperationGroup)value); }
        void ICollection<IFocusOperationGroup>.CopyTo(IFocusOperationGroup[] array, int index) { CopyTo((ILayoutOperationGroup[])array, index); }
        bool ICollection<IFocusOperationGroup>.IsReadOnly { get { return ((ICollection<ILayoutOperationGroup>)this).IsReadOnly; } }
        bool ICollection<IFocusOperationGroup>.Remove(IFocusOperationGroup item) { return Remove((ILayoutOperationGroup)item); }
        IEnumerator<IFocusOperationGroup> IEnumerable<IFocusOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        IFocusOperationGroup IReadOnlyList<IFocusOperationGroup>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IWriteableOperationGroupReadOnlyList ToReadOnly()
        {
            return new LayoutOperationGroupReadOnlyList(this);
        }
    }
}
