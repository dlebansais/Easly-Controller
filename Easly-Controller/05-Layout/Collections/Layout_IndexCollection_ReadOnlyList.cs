namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

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

        #region ILayoutIndexCollection
        IEnumerator<ILayoutIndexCollection> IEnumerable<ILayoutIndexCollection>.GetEnumerator() { return ((IList<ILayoutIndexCollection>)this).GetEnumerator(); }
        ILayoutIndexCollection IReadOnlyList<ILayoutIndexCollection>.this[int index] { get { return (ILayoutIndexCollection)this[index]; } }
        #endregion
    }
}
