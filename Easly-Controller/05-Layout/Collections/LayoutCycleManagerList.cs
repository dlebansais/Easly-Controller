namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutCycleManagerList : FocusCycleManagerList, ICollection<LayoutCycleManager>, IEnumerable<LayoutCycleManager>, IList<LayoutCycleManager>, IReadOnlyCollection<LayoutCycleManager>, IReadOnlyList<LayoutCycleManager>
    {
        /// <inheritdoc/>
        public new LayoutCycleManager this[int index] { get { return (LayoutCycleManager)base[index]; } set { base[index] = value; } }

        #region LayoutCycleManager
        void ICollection<LayoutCycleManager>.Add(LayoutCycleManager item) { Add(item); }
        bool ICollection<LayoutCycleManager>.Contains(LayoutCycleManager item) { return Contains(item); }
        void ICollection<LayoutCycleManager>.CopyTo(LayoutCycleManager[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<LayoutCycleManager>.Remove(LayoutCycleManager item) { return Remove(item); }
        bool ICollection<LayoutCycleManager>.IsReadOnly { get { return ((ICollection<FocusCycleManager>)this).IsReadOnly; } }
        IEnumerator<LayoutCycleManager> IEnumerable<LayoutCycleManager>.GetEnumerator() { var iterator = ((List<FocusCycleManager>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (LayoutCycleManager)iterator.Current; } }
        LayoutCycleManager IList<LayoutCycleManager>.this[int index] { get { return (LayoutCycleManager)this[index]; } set { this[index] = value; } }
        int IList<LayoutCycleManager>.IndexOf(LayoutCycleManager item) { return IndexOf(item); }
        void IList<LayoutCycleManager>.Insert(int index, LayoutCycleManager item) { Insert(index, item); }
        LayoutCycleManager IReadOnlyList<LayoutCycleManager>.this[int index] { get { return (LayoutCycleManager)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FocusCycleManagerReadOnlyList ToReadOnly()
        {
            return new LayoutCycleManagerReadOnlyList(this);
        }
    }
}
