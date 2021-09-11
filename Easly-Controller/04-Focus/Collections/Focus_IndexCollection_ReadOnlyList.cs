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

        /// <inheritdoc/>
        public new IFocusIndexCollection this[int index] { get { return (IFocusIndexCollection)base[index]; } }

        #region IFocusIndexCollection
        IEnumerator<IFocusIndexCollection> IEnumerable<IFocusIndexCollection>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusIndexCollection)iterator.Current; } }
        IFocusIndexCollection IReadOnlyList<IFocusIndexCollection>.this[int index] { get { return (IFocusIndexCollection)this[index]; } }
        #endregion
    }
}
