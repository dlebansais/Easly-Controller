namespace EaslyController.Focus
{
    using System.Collections.Generic;
    using EaslyController.Frame;

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

        #region FocusOperationGroup
        IEnumerator<FocusOperationGroup> IEnumerable<FocusOperationGroup>.GetEnumerator() { System.Collections.IEnumerator iterator = GetEnumerator(); while (iterator.MoveNext()) { yield return (FocusOperationGroup)iterator.Current; } }
        FocusOperationGroup IReadOnlyList<FocusOperationGroup>.this[int index] { get { return (FocusOperationGroup)this[index]; } }
        #endregion
    }
}
