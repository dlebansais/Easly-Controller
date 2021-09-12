namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutCycleManagerReadOnlyList : FocusCycleManagerReadOnlyList, IReadOnlyCollection<LayoutCycleManager>, IReadOnlyList<LayoutCycleManager>
    {
        /// <inheritdoc/>
        public LayoutCycleManagerReadOnlyList(LayoutCycleManagerList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new LayoutCycleManager this[int index] { get { return (LayoutCycleManager)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<LayoutCycleManager> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<FocusCycleManager>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (LayoutCycleManager)iterator.Current; } }

        #region LayoutCycleManager
        IEnumerator<LayoutCycleManager> IEnumerable<LayoutCycleManager>.GetEnumerator() { return GetEnumerator(); }
        LayoutCycleManager IReadOnlyList<LayoutCycleManager>.this[int index] { get { return (LayoutCycleManager)this[index]; } }
        #endregion
    }
}
