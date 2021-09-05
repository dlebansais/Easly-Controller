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

        #region FocusOperationGroup
        IEnumerator<FocusOperationGroup> IEnumerable<FocusOperationGroup>.GetEnumerator() { return ((IList<FocusOperationGroup>)this).GetEnumerator(); }
        FocusOperationGroup IReadOnlyList<FocusOperationGroup>.this[int index] { get { return (FocusOperationGroup)this[index]; } }
        #endregion
    }
}
