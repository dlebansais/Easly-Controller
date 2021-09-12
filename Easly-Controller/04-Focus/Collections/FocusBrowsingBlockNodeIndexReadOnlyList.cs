namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class FocusBrowsingBlockNodeIndexReadOnlyList : FrameBrowsingBlockNodeIndexReadOnlyList, IReadOnlyCollection<IFocusBrowsingBlockNodeIndex>, IReadOnlyList<IFocusBrowsingBlockNodeIndex>
    {
        /// <inheritdoc/>
        public FocusBrowsingBlockNodeIndexReadOnlyList(FocusBrowsingBlockNodeIndexList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new IFocusBrowsingBlockNodeIndex this[int index] { get { return (IFocusBrowsingBlockNodeIndex)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<IFocusBrowsingBlockNodeIndex> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyBrowsingBlockNodeIndex>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusBrowsingBlockNodeIndex)iterator.Current; } }

        #region IFocusBrowsingBlockNodeIndex
        IEnumerator<IFocusBrowsingBlockNodeIndex> IEnumerable<IFocusBrowsingBlockNodeIndex>.GetEnumerator() { return GetEnumerator(); }
        IFocusBrowsingBlockNodeIndex IReadOnlyList<IFocusBrowsingBlockNodeIndex>.this[int index] { get { return (IFocusBrowsingBlockNodeIndex)this[index]; } }
        #endregion
    }
}
