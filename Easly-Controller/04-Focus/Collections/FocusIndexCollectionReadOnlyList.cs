namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.ReadOnly;

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
        /// <inheritdoc/>
        public new IEnumerator<IFocusIndexCollection> GetEnumerator() { var iterator = ((ReadOnlyCollection<IReadOnlyIndexCollection>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (IFocusIndexCollection)iterator.Current; } }

        #region IFocusIndexCollection
        IEnumerator<IFocusIndexCollection> IEnumerable<IFocusIndexCollection>.GetEnumerator() { return GetEnumerator(); }
        IFocusIndexCollection IReadOnlyList<IFocusIndexCollection>.this[int index] { get { return (IFocusIndexCollection)this[index]; } }
        #endregion
    }
}
