namespace EaslyController.Layout
{
    using System.Collections.Generic;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class LayoutOperationGroupReadOnlyList : FocusOperationGroupReadOnlyList, IReadOnlyCollection<LayoutOperationGroup>, IReadOnlyList<LayoutOperationGroup>
    {
        /// <inheritdoc/>
        public LayoutOperationGroupReadOnlyList(LayoutOperationGroupList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new LayoutOperationGroup this[int index] { get { return (LayoutOperationGroup)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<LayoutOperationGroup> GetEnumerator() { var iterator = ((System.Collections.ObjectModel.ReadOnlyCollection<WriteableOperationGroup>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (LayoutOperationGroup)iterator.Current; } }

        #region LayoutOperationGroup
        IEnumerator<LayoutOperationGroup> IEnumerable<LayoutOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        LayoutOperationGroup IReadOnlyList<LayoutOperationGroup>.this[int index] { get { return (LayoutOperationGroup)this[index]; } }
        #endregion
    }
}
