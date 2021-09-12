namespace EaslyController.Frame
{
    using System.Collections.Generic;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FrameOperationGroupList : WriteableOperationGroupList, ICollection<FrameOperationGroup>, IEnumerable<FrameOperationGroup>, IList<FrameOperationGroup>, IReadOnlyCollection<FrameOperationGroup>, IReadOnlyList<FrameOperationGroup>
    {
        /// <inheritdoc/>
        public new FrameOperationGroup this[int index] { get { return (FrameOperationGroup)base[index]; } set { base[index] = value; } }

        #region FrameOperationGroup
        void ICollection<FrameOperationGroup>.Add(FrameOperationGroup item) { Add(item); }
        bool ICollection<FrameOperationGroup>.Contains(FrameOperationGroup item) { return Contains(item); }
        void ICollection<FrameOperationGroup>.CopyTo(FrameOperationGroup[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<FrameOperationGroup>.Remove(FrameOperationGroup item) { return Remove(item); }
        bool ICollection<FrameOperationGroup>.IsReadOnly { get { return ((ICollection<WriteableOperationGroup>)this).IsReadOnly; } }
        IEnumerator<FrameOperationGroup> IEnumerable<FrameOperationGroup>.GetEnumerator() { var iterator = ((List<WriteableOperationGroup>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (FrameOperationGroup)iterator.Current; } }
        FrameOperationGroup IList<FrameOperationGroup>.this[int index] { get { return (FrameOperationGroup)this[index]; } set { this[index] = value; } }
        int IList<FrameOperationGroup>.IndexOf(FrameOperationGroup item) { return IndexOf(item); }
        void IList<FrameOperationGroup>.Insert(int index, FrameOperationGroup item) { Insert(index, item); }
        FrameOperationGroup IReadOnlyList<FrameOperationGroup>.this[int index] { get { return (FrameOperationGroup)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override WriteableOperationGroupReadOnlyList ToReadOnly()
        {
            return new FrameOperationGroupReadOnlyList(this);
        }
    }
}
