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

        #region LayoutCycleManager
        IEnumerator<LayoutCycleManager> IEnumerable<LayoutCycleManager>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (LayoutCycleManager)iterator.Current; } }
        LayoutCycleManager IReadOnlyList<LayoutCycleManager>.this[int index] { get { return (LayoutCycleManager)this[index]; } }
        #endregion
    }
}
