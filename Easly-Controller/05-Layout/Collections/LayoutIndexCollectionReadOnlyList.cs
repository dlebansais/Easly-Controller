namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.ReadOnly;

    /// <inheritdoc/>
    public class LayoutIndexCollectionReadOnlyList : FocusIndexCollectionReadOnlyList, IReadOnlyCollection<ILayoutIndexCollection>, IReadOnlyList<ILayoutIndexCollection>
    {
        /// <inheritdoc/>
        public LayoutIndexCollectionReadOnlyList(LayoutIndexCollectionList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new ILayoutIndexCollection this[int index] { get { return (ILayoutIndexCollection)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<ILayoutIndexCollection> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<IReadOnlyIndexCollection>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (ILayoutIndexCollection)iterator.Current; } }

        #region ILayoutIndexCollection
        IEnumerator<ILayoutIndexCollection> IEnumerable<ILayoutIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        ILayoutIndexCollection IReadOnlyList<ILayoutIndexCollection>.this[int index] { get { return (ILayoutIndexCollection)this[index]; } }
        #endregion
    }
}
