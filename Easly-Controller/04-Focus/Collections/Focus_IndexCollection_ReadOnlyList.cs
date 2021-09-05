namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

    /// <inheritdoc/>
    public class FocusIndexCollectionReadOnlyList : FrameIndexCollectionReadOnlyList, IReadOnlyCollection<IFocusIndexCollection>, IReadOnlyList<IFocusIndexCollection>
    {
        /// <inheritdoc/>
        public FocusIndexCollectionReadOnlyList(FocusIndexCollectionList list)
            : base(list)
        {
        }

        #region IFocusIndexCollection
        IEnumerator<IFocusIndexCollection> IEnumerable<IFocusIndexCollection>.GetEnumerator() { return ((IList<IFocusIndexCollection>)this).GetEnumerator(); }
        IFocusIndexCollection IReadOnlyList<IFocusIndexCollection>.this[int index] { get { return (IFocusIndexCollection)this[index]; } }
        #endregion
    }
}
