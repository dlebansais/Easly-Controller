namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutCycleManagerReadOnlyList : FocusCycleManagerReadOnlyList, IReadOnlyCollection<ILayoutCycleManager>, IReadOnlyList<ILayoutCycleManager>
    {
        /// <inheritdoc/>
        public LayoutCycleManagerReadOnlyList(LayoutCycleManagerList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutCycleManager this[int index] { get { return (ILayoutCycleManager)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutCycleManager> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IFocusCycleManager>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutCycleManager)iterator.Current; } }

        #region ILayoutCycleManager
        IEnumerator<ILayoutCycleManager> IEnumerable<ILayoutCycleManager>.GetEnumerator() { return GetEnumerator(); }
        ILayoutCycleManager IReadOnlyList<ILayoutCycleManager>.this[int index] { get { return (ILayoutCycleManager)this[index]; } }
        #endregion
    }
}
