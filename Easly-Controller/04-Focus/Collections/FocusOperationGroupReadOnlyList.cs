namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public class FocusOperationGroupReadOnlyList : FrameOperationGroupReadOnlyList, IReadOnlyCollection<FocusOperationGroup>, IReadOnlyList<FocusOperationGroup>
    {
        /// <inheritdoc/>
        public FocusOperationGroupReadOnlyList(FocusOperationGroupList list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public new FocusOperationGroup this[int index] { get { return (FocusOperationGroup)base[index]; } }
        /// <inheritdoc/>
        public new IEnumerator<FocusOperationGroup> GetEnumerator() { var iterator = ((ReadOnlyCollection<WriteableOperationGroup>)this).GetEnumerator(); while (iterator.MoveNext()) { yield return (FocusOperationGroup)iterator.Current; } }

        #region FocusOperationGroup
        IEnumerator<FocusOperationGroup> IEnumerable<FocusOperationGroup>.GetEnumerator() { return GetEnumerator(); }
        FocusOperationGroup IReadOnlyList<FocusOperationGroup>.this[int index] { get { return (FocusOperationGroup)this[index]; } }
        #endregion
    }
}
