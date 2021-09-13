namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutCycleManagerList : FocusCycleManagerList, ICollection<ILayoutCycleManager>, IEnumerable<ILayoutCycleManager>, IList<ILayoutCycleManager>, IReadOnlyCollection<ILayoutCycleManager>, IReadOnlyList<ILayoutCycleManager>
    {
        /// <inheritdoc/>
        public new ILayoutCycleManager this[int index] { get { return (ILayoutCycleManager)base[index]; } set { base[index] = value; } }

        #region ILayoutCycleManager
        void ICollection<ILayoutCycleManager>.Add(ILayoutCycleManager item) { Add(item); }
        bool ICollection<ILayoutCycleManager>.Contains(ILayoutCycleManager item) { return Contains(item); }
        void ICollection<ILayoutCycleManager>.CopyTo(ILayoutCycleManager[] array, int arrayIndex) { ((System.Collections.ICollection)this).CopyTo(array, arrayIndex); }
        bool ICollection<ILayoutCycleManager>.Remove(ILayoutCycleManager item) { return Remove(item); }
        bool ICollection<ILayoutCycleManager>.IsReadOnly { get { return ((ICollection<IFocusCycleManager>)this).IsReadOnly; } }
        IEnumerator<ILayoutCycleManager> IEnumerable<ILayoutCycleManager>.GetEnumerator() { var iterator = ((List<IFocusCycleManager>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutCycleManager)iterator.Current; } }
        ILayoutCycleManager IList<ILayoutCycleManager>.this[int index] { get { return (ILayoutCycleManager)this[index]; } set { this[index] = value; } }
        int IList<ILayoutCycleManager>.IndexOf(ILayoutCycleManager item) { return IndexOf(item); }
        void IList<ILayoutCycleManager>.Insert(int index, ILayoutCycleManager item) { Insert(index, item); }
        ILayoutCycleManager IReadOnlyList<ILayoutCycleManager>.this[int index] { get { return (ILayoutCycleManager)this[index]; } }
        #endregion

        /// <inheritdoc/>
        public override FocusCycleManagerReadOnlyList ToReadOnly()
        {
            return new LayoutCycleManagerReadOnlyList(this);
        }
    }
}
