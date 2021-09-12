namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class LayoutOperationGroupList : FocusOperationGroupList, ICollection<LayoutOperationGroup>, IEnumerable<LayoutOperationGroup>, IList<LayoutOperationGroup>, IReadOnlyCollection<LayoutOperationGroup>, IReadOnlyList<LayoutOperationGroup>
    {
        /// <inheritdoc/>
        public new LayoutOperationGroup this[int index] { get { return (LayoutOperationGroup)base[index]; } set { base[index] = value; } }

        #region LayoutOperationGroup
        void ICollection<LayoutOperationGroup>.Add(LayoutOperationGroup item) { Add(item); }
        bool ICollection<LayoutOperationGroup>.Contains(LayoutOperationGroup item) { return Contains(item); }
        void ICollection<LayoutOperationGroup>.CopyTo(LayoutOperationGroup[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<LayoutOperationGroup>.Remove(LayoutOperationGroup item) { return Remove(item); }
        bool ICollection<LayoutOperationGroup>.IsReadOnly { get { return ((ICollection<FocusOperationGroup>)this).IsReadOnly; } }
        IEnumerator<LayoutOperationGroup> IEnumerable<LayoutOperationGroup>.GetEnumerator() { var iterator = ((List<WriteableOperationGroup>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (LayoutOperationGroup)iterator.Current; } }
        LayoutOperationGroup IList<LayoutOperationGroup>.this[int index] { get { return (LayoutOperationGroup)this[index]; } set { this[index] = value; } }
        int IList<LayoutOperationGroup>.IndexOf(LayoutOperationGroup item) { return IndexOf(item); }
        void IList<LayoutOperationGroup>.Insert(int index, LayoutOperationGroup item) { Insert(index, item); }
        LayoutOperationGroup IReadOnlyList<LayoutOperationGroup>.this[int index] { get { return (LayoutOperationGroup)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override WriteableOperationGroupReadOnlyList ToReadOnly()
        {
            return new LayoutOperationGroupReadOnlyList(this);
        }
    }
}
