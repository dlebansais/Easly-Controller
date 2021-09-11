namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusBrowsingListNodeIndexReadOnlyList : FrameBrowsingListNodeIndexReadOnlyList, IReadOnlyCollection<IFocusBrowsingListNodeIndex>, IReadOnlyList<IFocusBrowsingListNodeIndex>
    {
        /// <inheritdoc/>
        public FocusBrowsingListNodeIndexReadOnlyList(FocusBrowsingListNodeIndexList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFocusBrowsingListNodeIndex this[int index] { get { return (IFocusBrowsingListNodeIndex)base[index]; } }

        #region IFocusBrowsingListNodeIndex
        IEnumerator<IFocusBrowsingListNodeIndex> IEnumerable<IFocusBrowsingListNodeIndex>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusBrowsingListNodeIndex)iterator.Current; } }
        IFocusBrowsingListNodeIndex IReadOnlyList<IFocusBrowsingListNodeIndex>.this[int index] { get { return (IFocusBrowsingListNodeIndex)this[index]; } }
        #endregion
    }
}
