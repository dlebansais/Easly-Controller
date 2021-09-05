namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;

    /// <inheritdoc/>
    public class LayoutOperationGroupReadOnlyList : FocusOperationGroupReadOnlyList, IReadOnlyCollection<LayoutOperationGroup>, IReadOnlyList<LayoutOperationGroup>
    {
        /// <inheritdoc/>
        public LayoutOperationGroupReadOnlyList(LayoutOperationGroupList list)
            : base(list)
        {
        }

        #region LayoutOperationGroup
        IEnumerator<LayoutOperationGroup> IEnumerable<LayoutOperationGroup>.GetEnumerator() { return ((IList<LayoutOperationGroup>)this).GetEnumerator(); }
        LayoutOperationGroup IReadOnlyList<LayoutOperationGroup>.this[int index] { get { return (LayoutOperationGroup)this[index]; } }
        #endregion
    }
}
