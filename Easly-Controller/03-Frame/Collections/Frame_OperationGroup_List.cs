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
        new IFrameOperationGroup this[int index] { get; set; }
        new int Count { get; }
        new IEnumerator<IFrameOperationGroup> GetEnumerator();
    }

    /// <summary>
    /// List of IxxxOperationGroup
    /// </summary>
    internal class FrameOperationGroupList : Collection<IFrameOperationGroup>, IFrameOperationGroupList
    {
        #region Writeable
        IWriteableOperationGroup IWriteableOperationGroupList.this[int index] { get { return this[index]; } set { this[index] = (IFrameOperationGroup)value; } }
        IWriteableOperationGroup IList<IWriteableOperationGroup>.this[int index] { get { return this[index]; } set { this[index] = (IFrameOperationGroup)value; } }
        int IList<IWriteableOperationGroup>.IndexOf(IWriteableOperationGroup value) { return IndexOf((IFrameOperationGroup)value); }
        void IList<IWriteableOperationGroup>.Insert(int index, IWriteableOperationGroup item) { Insert(index, (IFrameOperationGroup)item); }
        void ICollection<IWriteableOperationGroup>.Add(IWriteableOperationGroup item) { Add((IFrameOperationGroup)item); }
        bool ICollection<IWriteableOperationGroup>.Contains(IWriteableOperationGroup value) { return Contains((IFrameOperationGroup)value); }
        void ICollection<IWriteableOperationGroup>.CopyTo(IWriteableOperationGroup[] array, int index) { CopyTo((IFrameOperationGroup[])array, index); }
        bool ICollection<IWriteableOperationGroup>.IsReadOnly { get { return ((ICollection<IFrameOperationGroup>)this).IsReadOnly; } }
        bool ICollection<IWriteableOperationGroup>.Remove(IWriteableOperationGroup item) { return Remove((IFrameOperationGroup)item); }
        IEnumerator<IWriteableOperationGroup> IEnumerable<IWriteableOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        IWriteableOperationGroup IReadOnlyList<IWriteableOperationGroup>.this[int index] { get { return this[index]; } }
        #endregion

        public virtual IWriteableOperationGroupReadOnlyList ToReadOnly()
        {
            return new FrameOperationGroupReadOnlyList(this);
        }
    }
}
