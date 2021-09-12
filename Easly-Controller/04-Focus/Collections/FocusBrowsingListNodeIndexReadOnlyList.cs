namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

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
        /// <inheritdoc/>
        public new IEnumerator<IFocusBrowsingListNodeIndex> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyBrowsingListNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusBrowsingListNodeIndex)iterator.Current; } }

        #region IFocusBrowsingListNodeIndex
        IEnumerator<IFocusBrowsingListNodeIndex> IEnumerable<IFocusBrowsingListNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFocusBrowsingListNodeIndex IReadOnlyList<IFocusBrowsingListNodeIndex>.this[int index] { get { return (IFocusBrowsingListNodeIndex)this[index]; } }
        #endregion
    }
}
