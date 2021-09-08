namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FocusOperationGroupList : FrameOperationGroupList, ICollection<FocusOperationGroup>, IEnumerable<FocusOperationGroup>, IList<FocusOperationGroup>, IReadOnlyCollection<FocusOperationGroup>, IReadOnlyList<FocusOperationGroup>
    {
        /// <inheritdoc/>
        public new FocusOperationGroup this[int index] { get { return (FocusOperationGroup)base[index]; } set { base[index] = value; } }

        #region FocusOperationGroup
        void ICollection<FocusOperationGroup>.Add(FocusOperationGroup item) { Add(item); }
        bool ICollection<FocusOperationGroup>.Contains(FocusOperationGroup item) { return Contains(item); }
        void ICollection<FocusOperationGroup>.CopyTo(FocusOperationGroup[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<FocusOperationGroup>.Remove(FocusOperationGroup item) { return Remove(item); }
        bool ICollection<FocusOperationGroup>.IsReadOnly { get { return ((ICollection<FrameOperationGroup>)this).IsReadOnly; } }
        IEnumerator<FocusOperationGroup> IEnumerable<FocusOperationGroup>.GetEnumerator() { return ((IList<FocusOperationGroup>)this).GetEnumerator(); }
        FocusOperationGroup IList<FocusOperationGroup>.this[int index] { get { return (FocusOperationGroup)this[index]; } set { this[index] = value; } }
        int IList<FocusOperationGroup>.IndexOf(FocusOperationGroup item) { return IndexOf(item); }
        void IList<FocusOperationGroup>.Insert(int index, FocusOperationGroup item) { Insert(index, item); }
        FocusOperationGroup IReadOnlyList<FocusOperationGroup>.this[int index] { get { return (FocusOperationGroup)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override WriteableOperationGroupReadOnlyList ToReadOnly()
        {
            return new FocusOperationGroupReadOnlyList(this);
        }
    }
}
